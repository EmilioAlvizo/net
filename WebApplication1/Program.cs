// WebApplication1/Program.cs
using System.Linq.Expressions;

Console.WriteLine(".............................................." );
Console.WriteLine("hola, mundo");
Console.WriteLine("The current time is " + DateTime.Now);
Console.WriteLine(".............................................." );
//unico caracter
Console.WriteLine('!');
//int
Console.WriteLine(123);
//float 6-9 digitos
Console.WriteLine(0.25f);
//double 15-17 digitos
Console.WriteLine(0.25);
//decimal 28-29 digitos
Console.WriteLine(0.25m);

var name = "John";
Console.WriteLine($"hola, {name}");

decimal gradePointAverage = 3.99872831m;
Console.WriteLine((int) gradePointAverage);

Console.WriteLine(5 / 10);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

ejercicio1.Correr();
ejercicio2.Correr();
ejercicio3.Correr();
switchCase.Correr();
prueba.Correr().Wait();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



