using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace SocialMedia.Api
{
    public class Program
    {
        //https://github.com/lucassklp/Default.Architecture
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel((env, config) =>
            {
                var server = env.Configuration.GetSection("Server");

                //Configura o HTTP
                var http = server.GetSection("Http");
                var httpPort = Convert.ToInt32(http.GetSection("Port").Value);
                var httpIp = http.GetSection("ListenIp").Value;
                config.Listen(IPAddress.Parse(httpIp), httpPort);

                //Configura o HTTPS
                //var https = server.GetSection("Https");

                //if (http.GetValue<bool>("Enabled"))
                //{
                //    var httpsPort = Convert.ToInt32(https.GetSection("Port").Value);
                //    var httpsIp = https.GetSection("ListenIp").Value;
                //    var certificate = https.GetSection("Certificate").Value;
                //    config.Listen(IPAddress.Parse(httpsIp), httpsPort, opt =>
                //    {
                //        var directory = $@"{Directory.GetCurrentDirectory()}\{certificate}";
                //        var passwd = https.GetValue<string>("Password");
                //        opt.UseHttps(directory, passwd);
                //    });
                //}
            })
            .UseStartup<Startup>()
            .Build();

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
