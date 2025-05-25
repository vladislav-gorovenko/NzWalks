using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.Application.Regions.Mappers;
using NZWalks.Application.Regions.Repositories;
using NZWalks.Application.Walks.Mappers;
using NZWalks.Application.Walks.Repositories;
using NZWalks.Application.Walks.Validators;
using NZWalks.Infrastructure.Data;
using NZWalks.Infrastructure.Repositaries.Regions;
using NZWalks.Infrastructure.Repositaries.Walks;

var builder = WebApplication.CreateBuilder(args);

// Injecting services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NzWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddAutoMapper(typeof(WalksMapper));
builder.Services.AddAutoMapper(typeof(RegionsMapper));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddWalkRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateWalkRequestValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();