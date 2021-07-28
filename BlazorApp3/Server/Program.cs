using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace BlazorApp3.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = Path.GetFullPath(Path.Combine("movie2-e3c7b-firebase-adminsdk-dk3zo-cbfa735233.json"));
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            CreateHostBuilder(args).Build().Run();



        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}