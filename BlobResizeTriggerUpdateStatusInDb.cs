using System;
using System.IO;
using System.Threading.Tasks;
using AzureTangyFunc.DataContext;
using AzureTangyFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureTangyFunc
{
    public class BlobResizeTriggerUpdateStatusInDb
    {
        private readonly AzureTangyDbContext _dbContext;

        public BlobResizeTriggerUpdateStatusInDb(AzureTangyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("BlobResizeTriggerUpdateStatusInDb")]
        public async Task Run([BlobTrigger("functionsalesrep-sm/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
            string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            string fileId = Path.GetFileNameWithoutExtension(name);
            SalesRequest salesRequestData = await _dbContext.SalesRequests.FirstOrDefaultAsync(x => x.Id == fileId);
            if (salesRequestData != null)
            {
                salesRequestData.Status = "Image Processed";
                _dbContext.SalesRequests.Update(salesRequestData);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
