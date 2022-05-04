using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FinanceManagerData.Contexts;
using FinanceManagerData.Models;
using FinanceManagerData.Repositories;
using FinanceManagerServices.Authentication;

namespace FinanceManagerWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; } 

        public void ConfigureServices(IServiceCollection services)
        {
            bool isProduction = Environment.IsProduction();
            
            services.AddScoped<JwtTokenHelper>();

            services.AddDbContextPool<AppDbContext>(options =>
            {
                string connectionString = Environment.IsDevelopment()
                    ? Configuration["LocalConnectionString"]
                    : Configuration["RemoteConnectionString"];
                
                options.UseSqlServer(connectionString);
            });          

            services.AddScoped<IAppRepository, AppDbContextRepository>();

            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = isProduction;
                    options.Password.RequireUppercase = isProduction;
                    options.Password.RequireLowercase = isProduction;
                    options.Password.RequireNonAlphanumeric = isProduction;
                })
                .AddEntityFrameworkStores<AppDbContext>();

            string secretKeyPhrase = Configuration[JwtTokenHelper.SECRET_PHRASE_KEY];
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secretKeyPhrase));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = true;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new()
                    {
                        IssuerSigningKey = key,
                        ValidateAudience = isProduction,
                        ValidateIssuer = isProduction,
                        ValidateIssuerSigningKey = isProduction
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtTokenHelper.AUTH_POLICY_NAME, policy =>
                {
                    policy.RequireClaim(JwtRegisteredClaimNames.Typ, JwtTokenHelper.AUTH_POLICY_NAME);
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinanceManagerWebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment Environment)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanceManagerWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
