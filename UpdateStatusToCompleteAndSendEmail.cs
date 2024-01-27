using System;
using System.Linq;
using System.Threading.Tasks;
using AzureTangyFunc.DataContext;
using AzureTangyFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureTangyFunc
{
    public class UpdateStatusToCompleteAndSendEmail
    {
        private readonly AzureTangyDbContext _dbContext;

        public UpdateStatusToCompleteAndSendEmail(AzureTangyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("UpdateStatusToCompleteAndSendEmail")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var salesRequestData = _dbContext.SalesRequests.Where(x => x.Status == "Image Processed").ToList();
            salesRequestData.ForEach(x => x.Status = "Completed");

            _dbContext.UpdateRange(salesRequestData);
           await _dbContext.SaveChangesAsync();

            //send mail functionality is pending
           
        }
    }
}
