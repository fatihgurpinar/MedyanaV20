using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreApiService
{
 /// <summary>
 /// 
 /// </summary>
    public class Program
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddCommandLine(args)
                        .Build();

            string urls = config.GetSection("AppSettings:applicationUrl").Value;
            // string urls = "http://localhost:5060;http://localhost:5061;http://192.168.0.92:5055";

            return
                    WebHost.CreateDefaultBuilder(args)
                    .UseUrls(urls)
                    .UseStartup<Startup>();
        }
    }
}
