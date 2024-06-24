using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var test = app.Logger;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//the API-endpoint of the validation
app.MapGet("/cardvalidation", (string? cardNumber) =>
{
    app.Logger.LogInformation("card number input: " + cardNumber);
    var json = new { message = "number to Validate: " +cardNumber, valid = luhnValidation.luhnValidation.isValidLuhn(cardNumber, app.Logger) };

    return json;
})
.WithName("GetCardValidation")
.WithOpenApi();

app.Run();

