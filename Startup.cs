using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLog;
using Seguridad.Datos.Context;
using Seguridad.Datos.NLog;
using Seguridad.Datos.Repositorio;
using Seguridad.Datos.Repositorio.Interfaces;
using Swashbuckle.AspNetCore.Swagger;



namespace ApplicacionSeguridad.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvcCore().
             AddJsonFormatters(J =>
             {
                 J.ContractResolver = new CamelCasePropertyNamesContractResolver();
                 J.Formatting = Newtonsoft.Json.Formatting.Indented;
             });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new  Info
                {
                    Version = "v1",
                    Title = "Swagger Demo",
                    Description = "Swagger Demo for ValuesController",
                    TermsOfService = "None",
                    Contact = new Contact()
                    {
                        Name = "Joydip Kanjilal",
                        Email = "joydipkanjilal@yahoo.com",
                        Url = "www.google.com"
                    }
                });
            });



            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader().
                    AllowAnyMethod().
                    AllowAnyOrigin().
                    AllowCredentials();

                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),

                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });



            services.AddDbContext<SeguridadDBContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("ConnectionSQL"))
            );

         
            services.AddScoped<ISesionRepo, SesionRepo>();
            services.AddScoped<IDetalleVisitaRepo, DetalleVisitaRepo>();

            services.AddSingleton<ILog, CoreNLogText>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(
             options => options.AllowAnyOrigin()//.WithOrigins("http://localhost:9000")
             .AllowAnyMethod()
             .AllowAnyHeader()
         );
            // app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseAuthentication();

            app.UseAuthentication();   //be sure to add this line
          

            //app.UseEndpoint(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});



            app.UseMvc();
        }
    }
}
