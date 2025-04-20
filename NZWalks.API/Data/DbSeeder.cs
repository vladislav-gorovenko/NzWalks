using Microsoft.AspNetCore.Identity;
using NZWalks.API.Constants;
using NZWalks.Core.Models.Domain;

namespace NZWalks.API.Data;

public class DbSeeder
{
    public static async Task SeedData(IApplicationBuilder app)
    {
        // для того, чтобы получить доступ к сервисам 
        // создаем scope 
        using var scope = app.ApplicationServices.CreateScope();

        // добавляем сервис для логгинга
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

        try
        {
            // добавляем сервисы для менеджмента ролей и юзеров 
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            // проверяем, есть ли какие-то user в бд и если нет, то создаем с ролью Admin
            if (userManager.Users.Any() == false)
            {
                var user = new ApplicationUser
                {
                    Name = "Admin",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                // если нет админа, то создаем его
                if ((await roleManager.RoleExistsAsync(Roles.Admin)) == false)
                {
                    logger.LogInformation("Admin role is creating");
                    var roleResult = await roleManager
                        .CreateAsync(new IdentityRole(Roles.Admin));

                    if (roleResult.Succeeded == false)
                    {
                        var roleErros = roleResult.Errors.Select(e => e.Description);
                        logger.LogError($"Failed to create admin role. Errors : {string.Join(",", roleErros)}");

                        return;
                    }

                    logger.LogInformation("Admin role is created");
                }

                // Attempt to create admin user
                var createUserResult = await userManager
                    .CreateAsync(user: user, password: "Admin@123");

                // Validate user creation
                if (createUserResult.Succeeded == false)
                {
                    var errors = createUserResult.Errors.Select(e => e.Description);
                    logger.LogError(
                        $"Failed to create admin user. Errors: {string.Join(", ", errors)}"
                    );
                    return;
                }

                // adding role to user
                var addUserToRoleResult = await userManager
                    .AddToRoleAsync(user: user, role: Roles.Admin);

                if (addUserToRoleResult.Succeeded == false)
                {
                    var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                    logger.LogError($"Failed to add admin role to user. Errors : {string.Join(",", errors)}");
                }

                logger.LogInformation("Admin user is created");
            }
        }

        catch (Exception ex)
        {
            logger.LogCritical(ex.Message);
        }
    }
}