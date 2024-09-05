using ENSEK.Data;
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


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // This step is to register application services
        builder.Services.AddTransient<IMeterReadingRepository, MeterReadingRepository>();
        builder.Services.AddTransient<IMeterReadingValidator, MeterReadingValidator>();
        builder.Services.AddTransient<IMeterReadingParser, MeterReadingParser>();

        // Configure database context
        builder.Services.AddDbContext<MeterReadingContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        var app = builder.Build();



        // This is to configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();



        Log.Information("Starting up the application...");
        app.Run();
    }
}
