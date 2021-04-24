using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitX
{
    public class ProgramA
    {
       

        public static Queuer MyQueue = new Queuer();
        public static List<Product> Products = new List<Product>();
        public static void Main(string[] args)
        {
            Products.Add(new Product(new Guid("cc69c7a0-72ca-4a48-9bbf-be4310ffe2a2")));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });   
        
    }
}
