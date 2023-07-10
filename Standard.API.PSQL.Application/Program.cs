using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Standard.API.PSQL.Application.Middlewares;
using Standard.API.PSQL.Domain;
using Standard.API.PSQL.Domain.Repository;
using Standard.API.PSQL.Domain.Services;
using Standard.API.PSQL.Infra.Data;
using Standard.API.PSQL.Infra.Data.Context;
using Standard.API.PSQL.Infra.Data.Repositories;
using Standard.API.PSQL.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load));

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext")), ServiceLifetime.Scoped);

builder.Services.AddScoped<ISampleRepository, SampleRepository>(f =>
{
    var context = f.GetService<DatabaseContext>();
    return new SampleRepository(context.Samples);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(f =>
{
    var context = f.GetService<DatabaseContext>();
    var sampleRepository = f.GetRequiredService<ISampleRepository>();
    return new UnitOfWork(context, sampleRepository);
});


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load));

// Add services to the container.
builder.Services.AddScoped<ISampleService, SampleService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builderCors => builderCors
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionHandler>();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
