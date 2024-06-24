var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/cardvalidation", () =>
{
    return isValidLuhn("17893729974");
})
.WithName("GetCardValidation")
.WithOpenApi();

app.Run();

bool isValidLuhn(string? cardNumber)
{
    // do not accept empty input
    if (string.IsNullOrWhiteSpace(cardNumber))
    {
        return false;
    }

    //trim spaces on both ends
    cardNumber = cardNumber.Trim();

    //luhn method
    //code based on the examples on wikipedia.
    int sum = 0;
    bool isDoublingValue = true;
    int checkDigit = int.Parse(cardNumber[cardNumber.Length - 1].ToString());

    for (int i = cardNumber.Length - 2; i >= 0; i--)
    {
        int n = int.Parse(cardNumber[i].ToString());

        if (isDoublingValue)
        {
            n *= 2;

            //if greater than 10 use the sum of two digits
            //since value range is 0~18
            //simplify this by -9
            if (n > 9)
            {
                n -= 9;
            }
        }

        sum += n;
        isDoublingValue = !isDoublingValue;
    }
    return (checkDigit == (10 - (sum % 10)));
}
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
