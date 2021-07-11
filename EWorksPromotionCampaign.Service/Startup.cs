using DbUp;
using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Services;
using EWorksPromotionCampaign.Service.Services.Admin;
using EWorksPromotionCampaign.Service.Services.External;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IPermissionService = EWorksPromotionCampaign.Service.Services.Admin.IPermissionService;
using IRoleService = EWorksPromotionCampaign.Service.Services.Admin.IRoleService;

namespace EWorksPromotionCampaign.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddControllers();
            services.AddHealthChecks();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EWorksPromotionCampaign.Service", Version = "v1" });
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<Services.IUserService>();
                        var adminService = context.HttpContext.RequestServices.GetRequiredService<Services.Admin.IUserService>();

                        var userEmail = context.Principal.Identity.Name;
                        var userRole = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                        var isAdminUser = userRole == "Admin";

                        dynamic user;
                        if (isAdminUser)
                            user = await adminService.GetByEmail(userEmail);
                        else
                            user = await userService.GetUserByEmail(userEmail);
                        if (user.Data is null)
                        {
                            context.Fail("User record not found; invalid user");
                            return;
                        }

                        if (user.Data.IsDisabled || user.Data.LockedOut || !user.Data.Status)
                        {
                            context.Fail("User is deactivated");
                            return;
                        }

                        if (isAdminUser)
                        {
                            if (user.Data.RoleId < 1)
                            {
                                context.Fail("Invalid user");
                                return;
                            }
                        }

                        var claims = new ClaimsIdentity(new List<Claim>{
                             new Claim("User", JsonConvert.SerializeObject(user.Data))
                        });
                        context.Principal.AddIdentity(claims);
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };
            });
            SetupDependencies(services);
            RunMigrations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EWorksPromotionCampaign.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
        private void SetupDependencies(IServiceCollection services)
        {
            // configure DI for application services
            #region Repositories
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IPasswordRepository, PasswordRepository>();
            services.AddSingleton<ITwoFactorRepository, TwoFactorRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<IPermissionRepository, PermissionRepository>();
            services.AddSingleton<IConfigurationRepository, ConfigurationRepository>();
            services.AddSingleton<ICampaignRepository, CampaignRepository>();
            services.AddSingleton<ICampaignRewardRepository, CampaignRewardRepository>();
            #endregion

            #region Services
            services.AddSingleton<Services.IUserService, Services.UserService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IPermissionService, PermissionService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<Services.Admin.IUserService, Services.Admin.UserService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<ICampaignRewardService, CampaignRewardService>();
            services.AddSingleton<ICampaignService, CampaignService>();
            #endregion

            #region Validators
            services.AddSingleton<IUserValidator, UserValidator>();
            services.AddSingleton<ILoginValidator, LoginValidator>();
            services.AddSingleton<IAdminUserValidator, AdminUserValidator>();
            services.AddSingleton<IPermissionValidator, PermissionValidator>();
            services.AddSingleton<IRoleValidator, RoleValidator>();
            services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();
            services.AddSingleton<ICampaignRewardValidator, CampaignRewardValidator>();
            services.AddSingleton<ICampaignValidator, CampaignValidator>();
            #endregion

            #region Configurations
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<ExternalServicesConfig>, ExternalServicesConfigurationValidation>());
            services.AddHostedService<ValidateOptionsService>();
            services.Configure<ExternalServicesConfig>(ExternalServicesConfig.QuickTellerServiceApi, Configuration.GetSection("ExternalServices:QuickTellerServiceApi"));
            services.Configure<ExternalServicesConfig>(ExternalServicesConfig.PaystackServiceApi, Configuration.GetSection("ExternalServices:PaystackServiceApi"));
            #endregion
        }
        public void RunMigrations()
        {
            bool isTestEnvironment = Environment.GetEnvironmentVariable("env") == "Test";
            if (!isTestEnvironment)
            {
                var connectionString = Configuration["ConnectionStrings:DefaultConnectionString"];
                EnsureDatabase.For.SqlDatabase(connectionString);
                var upgradeTran = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                        name => name.StartsWith("EWorksPromotionCampaign.Service.Migrations", StringComparison.CurrentCultureIgnoreCase))
                    .LogToConsole()
                    .WithTransactionPerScript()
                    .Build();

                if (!upgradeTran.IsUpgradeRequired())
                    return;

                var result = upgradeTran.PerformUpgrade();
                if (result.Successful)
                {
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }
            return;
        }
    }
}
