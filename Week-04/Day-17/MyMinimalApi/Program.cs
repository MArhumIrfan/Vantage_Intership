using System.Buffers;

using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summeries = new []
{
    "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Swelting","Scoraching"
};

app.MapGet("/weather", ()=>
{
    var forecast = Enumerable.Range(1,5).Select(index =>
    new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20,55),
            summeries[Random.Shared.Next(summeries.Length)] 
        )
    )
    .toArray;
    return forecast;    
})
.WithName("GetWeatherForecast");


app.MapGet("/api/welcome", () =>
{
    return Results.Ok(new{Message ="Welcome to intgrated Wb API ",TimestampAttribute=DateTime.Now});

});

app.MapGet("/api/calc", (strong op, double num1, double num2) =>

    double result = op.ToLower() switch (switch_on)
    {

            "add" or "+" => num1 + num2,
            "sub" or "-" => num1 - num2,
            "mul" or '*' => num1 * num2,
            "div" or "/" => num2 !=0 ? num1 / num2 :double.NaN;
            _ => double.NaN;

    };

    if(double.IsNaN(result))
    {
        
        return Results.BadRequest(new {Error = "Invalid operations or diviosn by zero. "});

    }
    

)