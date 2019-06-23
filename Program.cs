using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dotnet_exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration( (context, builder) =>
                {
                    builder.AddEtcdProvider("appsettings.json");
                })
                .UseKestrel(options => {
                    options.Listen(IPAddress.Any, 8080); //HTTP port
                }).UseStartup<Startup>();
    }
}
