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

//the API-endpoint of the validation
app.MapGet("/cardvalidation", (string? cardNumber) =>
{
    return isValidLuhn(cardNumber);
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
    int checkDigit = 0;

    for (int i = cardNumber.Length - 1; i >= 0; i--)
    {
        //return false if non-digit character is found
        if (!char.IsDigit(cardNumber[i]))
        {
            return false;
        }
        int n = int.Parse(cardNumber[i].ToString());


        //the last digit is used for validation.
        if (i == cardNumber.Length - 1)
        {
            checkDigit = n;
            continue;
        }

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
    //compare given check digit and the check digit by calculation
    //pass if results match
    return (checkDigit == (10 - (sum % 10)));
}