using System.Diagnostics;
using System.Reflection;
using Doco.Server.Gateway.Dal.Config;
using Doco.Server.Gateway.Dal.Services;
using Doco.Server.Gateway.Endpoints.Minimal.Auth;
using Doco.Server.Gateway.Endpoints.Minimal.Files;
using Doco.Server.Gateway.Endpoints.Minimal.Users;
using Doco.Server.Gateway.ExceptionHandlers;
using Doco.Server.Gateway.ExceptionHandlers.Impl;
using Doco.Server.Gateway.Options;
using Doco.Server.Gateway.Services.Auth;
using Doco.Server.Gateway.Services.Auth.Impl;
using Doco.Server.Gateway.Services.Daemons;
using Doco.Server.Gateway.Services.DatabaseAccess;
using Doco.Server.Gateway.Services.DatabaseAccess.Impl;
using Doco.Server.Gateway.Services.Files;
using Doco.Server.Gateway.Services.Files.Impl;
using Doco.Server.Gateway.Services.Repositories;
using Doco.Server.Gateway.Services.Repositories.Impl;
using Doco.Server.Gateway.Services.Users;
using Doco.Server.Gateway.Services.Users.Impl;
using Doco.Server.ServiceDiscovery;
using Microsoft.AspNetCore.Diagnostics;
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

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        if (builder.InMigratorMode())
            return builder;

        #region serviceDiscoveryTimeout

        {
            var serviceDiscoveryTimeoutSection = builder.Configuration.GetSection(ServiceDiscoveryTimeout.SectionName);
            if (serviceDiscoveryTimeoutSection.Exists() is false)
            {
                throw new Exception($"{ServiceDiscoveryTimeout.SectionName} section is not set");
            }

            builder.Services.Configure<ServiceDiscoveryTimeout>(serviceDiscoveryTimeoutSection);
        }

        #endregion

        #region connectionConfig

        {
            var connectionConfigSection = builder.Configuration.GetSection(PostgreSqlConnectionConfig.SectionName);
            if (connectionConfigSection.Exists() is false)
            {
                throw new Exception($"{PostgreSqlConnectionConfig.SectionName} section is not set");
            }

            builder.Services.Configure<PostgreSqlConnectionConfig>(connectionConfigSection);
        }

        #endregion

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        if (builder.InMigratorMode())
            return builder;

        var services = builder.Services;

        services
            .AddSingleton<IFileServiceUrlProvider, FileServiceUrlProvider>()
            .AddScoped<IUploadFileService, UploadFileService>()
            .AddScoped<IDownloadFileService, DownloadFileService>()
            .AddScoped<IGetFilesService, GetFilesService>()

            .AddScoped<IUserRepository, UserRepository>()
            .AddSingleton<IDbConnectionProvider, DbConnectionProvider>()

            .AddScoped<IGetUsersService, GetUsersService>()
            .AddScoped<ICreateUserService, CreateUserService>()
            
            .AddScoped<ILoginUserService, LoginUserService>();
        

        return builder;
    }

    public static WebApplicationBuilder AddGrpcServices(this WebApplicationBuilder builder)
    {
        if (builder.InMigratorMode())
            return builder;

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


    public static WebApplicationBuilder AddDaemons(this WebApplicationBuilder builder)
    {
        if (builder.InMigratorMode())
            return builder;

        builder.Services.AddHostedService<ServiceDiscoveryDaemon>();

        return builder;
    }
    
    public static WebApplicationBuilder AddGlobalExceptionHandler(this WebApplicationBuilder builder)
    {
        if (builder.InMigratorMode())
            return builder;

        builder.Services.AddSingleton<IGlobalExceptionHandler, GlobalExceptionHandler>();

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
            
            //allows jwt bearer auth
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header, 
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey 
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        } 
                    },
                    []
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
        var group = app.MapGroup("api").DisableAntiforgery();
        
        group
            .MapFileEndpoints()
            .MapUserEndpoints()
            .MapAuthEndpoints();
    }

    public static bool InMigratorMode(this WebApplicationBuilder builder)
    {
        var needMigration = builder.Configuration.GetValue<string>("MIGRATE") is "true";
        return needMigration;
    }

    public static void RunMigrations(this WebApplicationBuilder builder)
    {
        Debug.Assert(builder.InMigratorMode());
        
        var connectionConfigSection = builder.Configuration.GetSection(PostgreSqlConnectionConfig.SectionName);
        if (connectionConfigSection.Exists() is false)
        {
            throw new Exception($"{PostgreSqlConnectionConfig.SectionName} section is not set");
        }

        var connectionConfig = connectionConfigSection.Get<PostgreSqlConnectionConfig>();
        if (connectionConfig is null)
        {
            throw new Exception($"{PostgreSqlConnectionConfig.SectionName} section is invalid");
        }

        Console.WriteLine("Ensuring database created...");
        GatewayDbCreator.EnsureDatabaseCreated(connectionConfig);
        Console.WriteLine("Done.");
        
        Console.WriteLine("Starting migrations...");
        GatewayDbMigrator.Migrate(connectionConfig);
        Console.WriteLine("Done.");
    }

    public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature is not null)
                {
                    var handler = context.RequestServices.GetRequiredService<IGlobalExceptionHandler>();
                    await handler.HandleAsync(context, feature.Error);
                }
            });
        });

        return app;
    }
}