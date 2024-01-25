using Carter;
using FluentValidation;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("Default")!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
