using DefaultCorsPolicyNugetPackage;
using EAppointment.Application;
using EAppointment.Domain.Entities;
using EAppointment.Infrastructure;
using EAppointment.WebApi;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultCors(); //taner saydam hocan�n cors k�t�phanesi 
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();
Helper.CreateUserAsync(app).Wait();

app.Run();
