using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Repositaries.Regions;
using NZWalks.API.Repositaries.Walks;
using NZWalks.Core.Validators.Walk;
using NZWalks.Core.Mapping;
using NZWalks.Core.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

// Injecting services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtSettingsKey = builder.Configuration["JwtSettings:Key"] ?? "";
var jwtSettingsIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "";
var jwtSettingsAudience = builder.Configuration["JwtSettings:Audience"] ?? "";

var key = Encoding.ASCII.GetBytes(jwtSettingsKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = jwtSettingsAudience, // API's audience
        ValidIssuer = jwtSettingsIssuer, // API's URL
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddDbContext<NzWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<NzWalksDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();

builder.Services.AddAutoMapper(typeof(WalkProfile));
builder.Services.AddAutoMapper(typeof(RegionProfile));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddWalkRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateWalkRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TokenInfoValidator>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DbSeeder.SeedData(app);

app.Run();