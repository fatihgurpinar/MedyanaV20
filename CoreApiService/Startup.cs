using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using AutoMapper;
using CoreApiServiceToDB.Data.ClinicData;
using CoreApiServiceToDB.Data.EquipmentData;
using CoreApiServiceToDB.Logger.LoggerToTxt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CoreApiService
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {

        public static string CurrentDB = "";
        public static string ConnectionString = "";
        public static string SecretKey = "";


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();

            SecretKey = Configuration.GetSection("AppSettings:Token").Value;
            CurrentDB = Configuration.GetSection("AppSettings:CurrentDB").Value;

            if (CurrentDB == "MSSQL")
            {
                ConnectionString = Configuration.GetConnectionString("MSSQLDBConnectionString");

                //services.AddTransient<IAuthRepo>(s => new AuthRepoMSSQL(ConnectionString));

                services.AddTransient<IClinicRepo>(s => new ClinicRepoMSSQL(ConnectionString));
                services.AddTransient<IEquipmentRepo>(s => new EquipmentRepoMSSQL(ConnectionString));
                
            }
            else
            {
                //any other db can be assigned here
            }


            //--------------------------------------------------------

            string CurrentLogPath = Configuration.GetSection("LogFilePaths:CurrentLogPath").Value;
            

            if (CurrentLogPath == "LOCAL")
            {
                //-------------------------------                
                string LoggerToTxtLocalDiscFilePath = Configuration.GetSection("LogFilePaths:LoggerToTxtLocalDiscFilePath").Value;
                services.AddTransient<ILoggerToTxt>(s => new LoggerToTxtLocalDisc(LoggerToTxtLocalDiscFilePath));
                //-------------------------------
            }
            else // all other possibilities can be implemented ih this way
            {
                //-------------------------------                
                string LoggerToTxtExternalDiscFilePath = Configuration.GetSection("LogFilePaths:LoggerToTxtExternalDiscFilePath").Value;
                services.AddTransient<ILoggerToTxt>(s => new LoggerToTxtExternalDisc(LoggerToTxtExternalDiscFilePath));
                //-------------------------------
            }


            //--------------------------------------------------------


            //if it is necessary, db log implementation can be done here!

            //--------------------------------------------------------



            services.AddRazorPages(); //it was initially added
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvcCore().AddApiExplorer();
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Medyana Main Core API Docs - 1.0.0",
                    Description = "Produced by Swagger",
                    Version = "v1"
                });

                //in project properties-> build ---> check output -> xml doc file  -->  copy file name..  CoreApiService.xml
                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"CoreApiService.xml";
                opt.IncludeXmlComments(xmlPath);

            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //--- for global handler  ----

            app.UseExceptionHandler(builder => {
                builder.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null)
                    {
                        //context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);

                    }
                });
            });

            //--- /for global handler  ----
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.). specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medyana Core API Docs");
                    }
                );
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
