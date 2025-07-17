using CSI_GenerateCMInvoice;
using GenerateCMInvoice.Application.Interfaces;
using GenerateCMInvoice.Application.Services;
using GenerateCMInvoice.Infrastructure.Interfaces;
using GenerateCMInvoice.Infrastructure.Repositories;
using GenerateCMInvoice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GenerateCMInvoice.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.SetBasePath(AppContext.BaseDirectory) // Use the correct base path
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DatabaseConnection")));

        services.Configure<FileSettings>(context.Configuration.GetSection("FileSettings"));
        services.Configure<DateSettings>(context.Configuration.GetSection("DateSettings"));

        services.AddTransient<ICmTransactionService, CmTransactionService>();
        services.AddTransient<IStoredProcedureService, StoredProcedureService>();
        services.AddTransient<IFileService, FileService>();

        services.AddTransient<ICmTransactionRepository, CmTransactionRepository>();
        services.AddTransient<IStoredProcedureRepository, StoredProcedureRepository>();

        services.AddHostedService<Worker>();

        // Check if it's in development or production
        var env = context.HostingEnvironment;
        if (env.IsDevelopment())
        {
            // Development-specific services or configurations
        }
        else if (env.IsProduction())
        {
            // Production-specific services or configurations
        }
    })
    .Build();

await host.RunAsync();
