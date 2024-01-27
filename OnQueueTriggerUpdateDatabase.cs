using System;
using AzureTangyFunc.DataContext;
using AzureTangyFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureTangyFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureTangyDbContext _dbContext   ;
        public OnQueueTriggerUpdateDatabase(AzureTangyDbContext dbContext)
        {
            _dbContext = dbContext ;
        }
        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("salesrequestinbound", Connection = "AzureWebJobsStorage")]SalesRequest salesRequest, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {salesRequest}");
            salesRequest.Status = "Submitted";
            _dbContext.Add(salesRequest);
            _dbContext.SaveChanges();
        }
    }
}
