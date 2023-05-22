using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using WebApi.Core;
using WebApi.Utilities;
using WebApi;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ///可由第1個參數指定起始路徑
            if (args != null && args.Length > 0 && Directory.Exists(args[0].ToString()))
            {
                Helper.SetRootPath(args[0]);
            }

            Ioc.Register(args);
            var config = Ioc.GetConfig();

            var webHostBuilder = Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.Limits.MaxConcurrentConnections = 1000;
                        options.ListenAnyIP(config.Port);
                    });
                    //webBuilder.UseWebRoot(config.ContentsFolder);
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIISIntegration();
                    webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>();
                });


            try
            {
                var webHost = webHostBuilder.Build();
                if (config.LaunchBrowser)
                {
                    var ps = new ProcessStartInfo("chrome")
                    {
                        UseShellExecute = true,
                        Arguments = "http://localhost:" + config.Port.ToString(),
                        Verb = "open"
                    };
                    Process.Start(ps);
                }
                webHost.Run();
            }
            catch (Exception ex)
            {
                string msg = string.Format("啟動失敗, {0}", ex);
                Console.WriteLine(msg);
                Environment.Exit(-1);
            }
        }
    }
}
