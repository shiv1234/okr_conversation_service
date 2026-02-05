using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OkrConversationService.Application.Extensions;
using OkrConversationService.Application.Filters;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.BaseAdapter;
using OkrConversationService.Infrastructure.AutoMapper;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OkrConversationService.Application
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public static IWebHostEnvironment AppEnvironment { get; private set; }
        public IConfiguration Configuration { get; }        
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            AppEnvironment = env;
            Configuration = configuration;
            var envName = env?.EnvironmentName;
            Console.WriteLine("envName: " + envName);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers().AddNewtonsoftJson(opt =>
                     opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );
            services.AddLogging();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("Redis:ConnectionString");
                options.InstanceName = Configuration.GetValue<string>("Redis:InstanceName");
            });

            var keyVault = new DatabaseVaultResponse();
            services.AddDbContext<ConversationContext>((serviceProvider, options) =>
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                if (httpContextAccessor != null)
                {
                    var httpContext = httpContextAccessor.HttpContext;
                    var hasTenant = httpContext.Request.Headers.TryGetValue("TenantId", out var tenantId);
                    if ((!hasTenant && httpContext.Request.Host.Value.Contains("localhost")))
                        tenantId = Configuration.GetValue<string>("TenantId");
                    if (!string.IsNullOrEmpty(tenantId))
                    {
                        var tenantString = Encryption.DecryptRijndael(tenantId, AppConstants.EncryptionPrivateKey);
                        var key = tenantString + "-Connection" + Configuration.GetValue<string>("KeyVaultConfig:PostFix");
                        keyVault.ConnectionString = Configuration.GetValue<string>(key);

                        if (keyVault.ConnectionString == null) return;
                        var retryPolicy = Policy.Handle<Exception>().Retry(2, (ex, count, context) =>
                        {
                            (Configuration as IConfigurationRoot)?.Reload();
                            keyVault.ConnectionString = Configuration.GetValue<string>(key);
                        });
                        retryPolicy.Execute(() =>
                        {
                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                .UseSqlServer(keyVault.ConnectionString);
                            options.EnableSensitiveDataLogging();
                        });
                    }
                    else
                    {
                        Console.WriteLine("Invalid tenant is received");
                    }
                }
            });
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddMediatR(typeof(Startup).Assembly);
            var assembly = AppDomain.CurrentDomain.Load("OkrConversationService.Infrastructure");
            services.AddMediatR(assembly);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddAutoMapper(Assembly.Load("OkrConversationService.Infrastructure"));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddAuthentication();

            services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilterAttribute)))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddMvc(options => options.Filters.Add(typeof(ResultFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc(options => options.Filters.Add(typeof(ActionFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<CustomHeaderSwaggerFilterAttribute>();
                c.SwaggerDoc("v2", new OpenApiInfo()
                {
                    Version = "v2",
                    Title = "Conversation-" + AppEnvironment?.EnvironmentName,
                    Description = "OKR Conversation Service APIs",
                    TermsOfService = new Uri(Configuration.GetSection("TermsAndConditionUrl").Value)
                });
                c.AddServer(new OpenApiServer()
                {
                    Url = "/conversation"
                });
            });

            services.AddScoped<IOperationStatus, OperationStatus>();
            services.AddScoped<IOkrAuthDbContext>(opt => opt.GetRequiredService<OkrAuthDbContext>());
            services.AddScoped<IUnitOfWorkAsync, UnitOfWork>();
            services.AddTransient<IServicesAggregator, ServicesAggregator>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<ICommonBase, CommonBase>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<IKeyVaultService, KeyVaultService>();
            services.AddTransient<IConversationService, ConversationService>();
            services.AddTransient<ISystemService, SystemService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<INotificationsEmailsService, NotificationsEmailsService>();
            services.AddTransient<IRecognitionService, RecognitionService>();           
            services.AddTransient<ICheckInService, CheckInService>();
            services.AddTransient<INoteService, NoteService>();            
            services.AddControllers();

            // Inject Application Insight Dependency
            services.AddApplicationInsightsTelemetry(Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));

            // Add health check dependency
            services.AddHealthChecks()
                .AddCheck<ApplicationHealthCheck>("Application Health Check");
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration.GetSection("SwaggerEndpoint").Value, "OKR Report APIs");
            });
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<TokenManagerMiddleware>();

            app.Map("/health", health =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health");
                });
            });
            app.Map("/conversation", health =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }
    }

}
