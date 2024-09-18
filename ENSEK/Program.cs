using ENSEK.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ENSEK.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuring logging
        var logEventLevel = LogEventLevel.Debug;
        var applicationName = "ENSEK";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(logEventLevel)
            .WriteTo.Console()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        // Adding services to the container
        builder.Services.AddControllers();        

        // enable API documentation generation for swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // This step is to register application services
        builder.Services.AddTransient<IMeterReadingRepository, MeterReadingRepository>();
        builder.Services.AddTransient<IMeterReadingValidator, MeterReadingValidator>();
        builder.Services.AddTransient<IMeterReadingParser, MeterReadingParser>();

        //build web app with all configured services
        var app = builder.Build();



        // This is to configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();


        app.MapControllers();
       

        Log.Information("Starting up the application...");
        //Starts the application and begins listening for HTTP requests.
        app.Run();
    }
}
