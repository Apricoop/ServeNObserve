using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ServeNObserve.Domain.Repositories;
using ServeNObserve.Domain.Services;
using ServeNObserve.Persistence.Contexts;
using ServeNObserve.Persistence.Repositories;
using ServeNObserve.Services;
using ServeNObserve.Shared;

namespace ServeNObserve
{
    public class Startup
    {
        private readonly string CorsAll = "all";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //Reads from appsettings.json
        public IConfiguration Configuration { get; }

        readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region AppSettings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            #endregion

            #region DbConnection
            // Should Be Your Own DB
            services.AddDbContext<DatabaseContext>
                (options => options.UseSqlServer(
                Configuration.GetConnectionString("LocalDb")));
            // appsettings.json dosyasindaki degerlerdedn db yolu degisitrilip herhangi bir db ile calisabilir
            #endregion

            #region NewtonsoftJson
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            #endregion

            #region Injections
            // JWT Generators
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            // Generic Repository Implementation for EF - DB Access with ORM
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Repo and service injections of our models
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IJobRepository, CommonRepository>();
            services.AddScoped<IEmployeeRepository, CommonRepository>();
            services.AddScoped<IServiceRepository, CommonRepository>();
            services.AddScoped<IJobEmployee, CommonRepository>();
            #endregion

            #region Cors Config
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsAll,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                      // .WithMethods("PUT", "DELETE", "GET");
                                      // To do it, Use [EnableCors("CorsPolicyName")] in Controller or endpoint
                                  });
            });
            #endregion

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            #region Authentication
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        //ClockSkew = TimeSpan.FromMinutes(30)
                    };
                });
            #endregion

            #region Swagger
            //services.AddTransient<IConfiguration<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ServeNObserve",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Eraslank & Burki"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter Bearer [space] and then user token in the text input below.\r\n\r\n Bearer yazýp boþluktan sonra tokeni giriniz\r\n\r\nExample: Bearer 12345abcdef",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });

                //c.DescribeAllEnumsAsStrings();
                // to provide comment support on swagger 
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
            #endregion

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServeNObserve v1 API"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServeNObserve v1 API"));
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(CorsAll);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Dashboard}/{id?}");
            });
        }
    }
}
