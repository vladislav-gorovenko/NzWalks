using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositaries.Regions;
using NZWalks.API.Repositaries.Walks;
using NZWalks.API.Validators.Walk;
using NZWalks.Core.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Injecting services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NzWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddAutoMapper(typeof(WalkProfile));
builder.Services.AddAutoMapper(typeof(RegionProfile));
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