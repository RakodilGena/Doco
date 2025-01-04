using System.Reflection;
using Doco.Server.Gateway.Endpoints.Minimal.Files;
using Doco.Server.Gateway.Options;
using Doco.Server.Gateway.Services;
using Doco.Server.Gateway.Services.Impl;
using Doco.Server.ServiceDiscovery;
using Microsoft.OpenApi.Models;

namespace Doco.Server.Gateway.Extensions;

internal static class StartupExtensions
{
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

    public static WebApplicationBuilder AddGrpcServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        var serviceDiscoveryUrl = builder.Configuration
            .GetSection(ServiceDiscoveryUrl.SectionName)
            .Get<ServiceDiscoveryUrl>();

        builder.Services.AddGrpcClient<FileServicesDiscovery.FileServicesDiscoveryClient>(options =>
        {
            options.Address = new Uri(serviceDiscoveryUrl!.Value);
        });

        return builder;
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
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