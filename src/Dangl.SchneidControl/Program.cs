using Dangl.Data.Shared.AspNetCore;
using Dangl.SchneidControl.Configuration;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Services;
using Microsoft.EntityFrameworkCore;
using NSwag;
using System.Text.Json.Serialization;
using static Dangl.SchneidControl.Configuration.SchneidControlSettings;

namespace Dangl.SchneidControl
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration, out var shouldInitializeDatabase);

            var app = builder.Build();
            ConfigureApp(app, builder.Environment);

            if (shouldInitializeDatabase)
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DataLoggingContext>();
                await context.Database.MigrateAsync();
            }

            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services,
            ConfigurationManager configuration,
            out bool shouldInitializeDatabase)
        {
            var appConfig = configuration.Get<SchneidControlSettings>() ?? throw new AppConfigException("Failed to load configuration");
            appConfig.Validate();

            services.AddSingleton<ModbusConnectionManager>(_ => new ModbusConnectionManager(appConfig.SchneidModbusIpAddress, appConfig.SchneidModbusTcpPort));
            services.AddTransient<ISchneidReadRepository, SchneidReadRepository>();
            services.AddTransient<ISchneidWriteRepository, SchneidWriteRepository>();

            var statsLoggingIsEnabled = !string.IsNullOrWhiteSpace(appConfig.DatabaseLoggingFilePath);
            if (statsLoggingIsEnabled)
            {
                shouldInitializeDatabase = true;
                services.AddDbContext<DataLoggingContext>(o =>
                {
                    o.UseSqlite($"Data Source={appConfig.DatabaseLoggingFilePath}");
                });
                services.AddTransient<IHostedService, DataLoggingScheduler>();
                services.AddTransient<IDataLoggingService, DataLoggingService>();
                services.AddTransient<IStatsRepository, StatsRepository>();
                services.AddTransient<IConsumptionRepository, ConsumptionRepository>();
            }
            else
            {
                shouldInitializeDatabase = false;
            }
            services.AddTransient(_ => new StatsEnabledService(statsLoggingIsEnabled));

            services.AddControllers(o =>
            {
                // We want to return 400 Bad Request for missing but required form files in controllers
                o.Filters.Add(typeof(RequiredFormFileValidationFilter));
            });

            services.AddMvc()
                .AddJsonOptions(j =>
                {
                    j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddOpenApiDocument(c =>
            {
                c.Description = "Dangl.SchneidControl API Specification";
                c.Version = VersionsService.Version;
                c.Title = $"Dangl.SchneidControl API {VersionsService.Version}";

                c.PostProcess = (x) =>
                {
                    foreach (var path in x.Paths.SelectMany(p => p.Value))
                    {
                        var containsProblemJson = path.Value.Produces?.Any(p => p == "application/problem+json") ?? false;
                        if (!containsProblemJson)
                        {
                            path.Value.Produces ??= new List<string>();
                            path.Value.Produces.Add("application/problem+json");
                        }
                    }

                    // Some enum classes use multiple integer values for the same value, e.g.
                    // System.Net.HttpStatusCode uses these:
                    // RedirectKeepVerb = 307
                    // TemporaryRedirect = 307
                    // MVC is configured to use the StringEnumConverter, and NJsonSchema errorenously
                    // outputs the duplicates. For the example above, the value 'TemporaryRedirect' is
                    // serialized twice, 'RedirectKeepVerb' is missing.
                    // The following post process action should remove duplicated enums
                    // See https://github.com/RSuter/NJsonSchema/issues/800 for more information
                    foreach (var enumType in x.Definitions.Select(d => d.Value).Where(d => d.IsEnumeration))
                    {
                        var distinctValues = enumType.Enumeration.Distinct().ToList();
                        enumType.Enumeration.Clear();
                        foreach (var distinctValue in distinctValues)
                        {
                            enumType.Enumeration.Add(distinctValue);
                        }
                    }
                };
            });
        }

        private static void ConfigureApp(WebApplication app, IWebHostEnvironment environment)
        {
            app.UseForwardedHeaders();

            app.UseOpenApi(c =>
            {
                c.Path = "/swagger/swagger.json";
                c.PostProcess = (doc, _) =>
                {
                    // This makes sure that Azure warmup requests that are sent via Http instead of Https
                    // don't set the document schema to http only
                    doc.Schemes = new List<OpenApiSchema> { OpenApiSchema.Https, OpenApiSchema.Http };
                };
            });
            app.UseSwaggerUi(settings =>
            {
                settings.DocumentTitle = "Dangl.SchneidControl API Swagger UI";
                settings.DocumentPath = "/swagger/swagger.json";
                settings.Path = "/swagger";
            });

            app.UseHttpsRedirection();

            app.UseSchneidControlVersionHeader();

            app.UseHttpHeadToGetTransform();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

#pragma warning disable ASP0014
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpa(spaOptions =>
            {
                if (environment.IsDevelopment())
                {
                    spaOptions.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }

                spaOptions.Options.DefaultPage = "/dist/index.html";
            });
        }
    }
}
