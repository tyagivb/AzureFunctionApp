using AzureTangyFunc;
using AzureTangyFunc.DataContext;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: WebJobsStartup(typeof(Startup))]
namespace AzureTangyFunc
{
   
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string connString = Environment.GetEnvironmentVariable("AzureSqlDB");
            builder.Services.AddDbContext<AzureTangyDbContext>(options => options.UseSqlServer(connString));
            builder.Services.BuildServiceProvider();
        }
    }
}
