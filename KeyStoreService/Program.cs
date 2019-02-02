using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KeyStoreService.Mesg;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KeyStoreService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Task.WhenAll(
                MesgProvider.ExecuteAsync(),
                Task.Run(() => CreateWebHostBuilder(args).Build().Run())
            );
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5321")
                .UseStartup<Startup>();
    }
}
