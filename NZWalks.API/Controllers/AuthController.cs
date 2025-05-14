using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using NZWalks.API.Constants;
using NZWalks.API.Data;
using NZWalks.API.Services.TokenService;
using NZWalks.Core.Models.Domain;
using NZWalks.Core.Models.DTOs.Auth;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly NzWalksDbContext _context; // TODO: добавить отдельный репозиторий для AuthController

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        ILogger<AuthController> logger, ITokenService tokenService, NzWalksDbContext context)
    {
        this._userManager = userManager;
        this._roleManager = roleManager;
        this._logger = logger;
        this._tokenService = tokenService;
        this._context = context;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequestDto signupRequestDto)
    {
        try
        {
            // если такой user уже есть, то бросаем ошибку 
            var existingUser = await _userManager.FindByNameAsync(signupRequestDto.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            // если не существует роли обычного user, то создаем ее 
            if ((await _roleManager.RoleExistsAsync(Roles.User)) == false)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(Roles.User));
                if (roleResult.Succeeded == false)
                {
                    var roleErrors = roleResult.Errors.Select((error => error.Description));
                    _logger.LogError($"Failed to create user role. Errors : {string.Join(",", roleErrors)}");
                    return BadRequest($"Failed to create user role. Errors : {string.Join(",", roleErrors)}");
                }
            }

            ApplicationUser user = new()
            {
                Email = signupRequestDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = signupRequestDto.Email,
                Name = signupRequestDto.Name,
                EmailConfirmed = true
            };

            // создаем user 
            var createUserResult = await _userManager.CreateAsync(user, signupRequestDto.Password);
            // если не получилось создать user, выбрасываем ошибку
            if (createUserResult.Succeeded == false)
            {
                var errors = createUserResult.Errors.Select(error => error.Description);
                _logger.LogError($"Failed to create user. Errors : {string.Join(",", errors)}");
                return BadRequest($"Failed to create user. Errors : {string.Join(",", errors)}");
            }

            // добавляем роль юзеру 
            var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Roles.User);
            if (addUserToRoleResult.Succeeded == false)
            {
                var errors = addUserToRoleResult.Errors.Select(error => error.Description);
                _logger.LogError($"Failed to add user role. Errors : {string.Join(",", errors)}");
            }

            return CreatedAtAction(nameof(Signup), null);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return BadRequest("User with this username is not registered with us.");
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (isValidPassword == false)
            {
                return Unauthorized();
            }

            // создаем необходимые Сlaims
            List<Claim> authClaims =
            [
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // уникальный идентификатор для токена
            ];

            var userRoles = await _userManager.GetRolesAsync(user);

            // добавляем роли в Claims, чтобы мы могли получить роль пользователя из токена
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // генерируем токен доступа
            var token = _tokenService.GenerateAccessToken(authClaims);

            string refreshToken = _tokenService.GenerateRefreshToken();

            // сохраняем токен обновления с датой истечения срока в бд
            var tokenInfo = _context.TokenInfos.FirstOrDefault(a => a.Username == user.UserName);

            // если tokenInfo для пользователя отсутствует, создаем новый
            if (tokenInfo == null)
            {
                var ti = new TokenInfo
                {
                    Username = user.UserName,
                    RefreshToken = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddDays(7)
                };
                _context.TokenInfos.Add(ti);
            }
            // иначе обновляем токен обновления и срок действия
            else
            {
                tokenInfo.RefreshToken = refreshToken;
                tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
            }

            await _context.SaveChangesAsync();

            return Ok(new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Unauthorized();
        }
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenRequestDto.AccessToken);
            var username = principal.Identity.Name;

            var tokenInfo = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
            if (tokenInfo == null
                || tokenInfo.RefreshToken != refreshTokenRequestDto.RefreshToken
                || tokenInfo.ExpiredAt <= DateTime.UtcNow)
            {
                return BadRequest("Invalid refresh token. Please login again.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            tokenInfo.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new RefreshTokenRequestDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("token/revoke")]
    [Authorize]
    public async Task<IActionResult> Revoke()
    {
        try
        {
            var username = User.Identity.Name;

            var user = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                return BadRequest("Invalid refresh token. Please login again.");
            }

            user.RefreshToken = string.Empty;
            await _context.SaveChangesAsync();
            return Ok(true);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}