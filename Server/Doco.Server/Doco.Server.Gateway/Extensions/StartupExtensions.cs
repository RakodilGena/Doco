using System.Reflection;
using Doco.Server.Gateway.Endpoints.Minimal.Files;
using Doco.Server.Gateway.Options;
using Doco.Server.Gateway.Services;
using Doco.Server.Gateway.Services.Daemons;
using Doco.Server.Gateway.Services.Internal;
using Doco.Server.Gateway.Services.Internal.Impl;
using Doco.Server.ServiceDiscovery;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;

namespace Doco.Server.Gateway.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureMaxRequestSize(this WebApplicationBuilder builder)
    {
        const int requestBodySize = 1024 * 1024 * 1024; //1gb
        builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = requestBodySize; });
        builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = requestBodySize);

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services
            .AddSingleton<IFileServiceUrlProvider, FileServiceUrlProvider>()
            .AddScoped<IUploadFileService, UploadFileService>()
            .AddScoped<IDownloadFileService, DownloadFileService>()
            .AddScoped<IGetFilesService, GetFilesService>();

        return builder;
    }

    public static WebApplicationBuilder AddDaemons(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        var serviceDiscoveryTimeoutSection = builder.Configuration.GetSection(ServiceDiscoveryTimeout.SectionName);
        if (serviceDiscoveryTimeoutSection.Exists() is false)
        {
            throw new Exception($"{ServiceDiscoveryTimeout.SectionName} section is not set");
        }
        
        builder.Services.Configure<ServiceDiscoveryTimeout>(serviceDiscoveryTimeoutSection);

        services.AddHostedService<ServiceDiscoveryDaemon>();

        return builder;
    }

    public static WebApplicationBuilder AddGrpcServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();

        var serviceDiscoveryUrl = builder.Configuration
            .GetValue<string>("SERVICE_DISCOVERY");

        if (string.IsNullOrEmpty(serviceDiscoveryUrl))
            throw new Exception("SERVICE_DISCOVERY variable is empty");

        builder.Services.AddGrpcClient<FileServicesDiscovery.FileServicesDiscoveryClient>(options =>
        {
            options.Address = new Uri(serviceDiscoveryUrl);
        });

        return builder;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "DOCO API",
                Description = "An DOCO app gateway API",

                TermsOfService = new Uri("https://example.com/terms"),

                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },

                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
        return builder;
    }

    public static void UseSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            SwaggerBuilderExtensions.UseSwagger(app);
            app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });
        }
    }

    public static void MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/").DisableAntiforgery();
        group.MapFileEndpoints();
    }
}