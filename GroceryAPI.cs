using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureTangyFunc.DataContext;
using AzureTangyFunc.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AzureTangyFunc
{
    public class GroceryAPI
    {
        private readonly AzureTangyDbContext _dbContext;

        public GroceryAPI(AzureTangyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("CreateGrocery")]
        public async Task<IActionResult> CreateGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GeoceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create Geocery Item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);
            GroceryItem groceryItem = new GroceryItem()
            {
                Name = data.Name
            };

            await _dbContext.AddAsync(groceryItem);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult(groceryItem);
        }

        [FunctionName("GetGrocery")]
        public async Task<IActionResult> GetGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GeoceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get Geocery All List Item.");

            var groceryList = await _dbContext.GroceryItems.ToListAsync();

            return new OkObjectResult(groceryList);
        }

        [FunctionName("GetGroceryById")]
        public async Task<IActionResult> GetGroceryById(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GeoceryList/{id}")] HttpRequest req,string id,
           ILogger log)
        {
            log.LogInformation("Get Geocery List Item by Id.");

            var groceryItem = await _dbContext.GroceryItems.FirstOrDefaultAsync(x => x.Id == id);
            if (groceryItem == null)
                return new NotFoundResult();

            return new OkObjectResult(groceryItem);
        }   

        [FunctionName("UpdateGrocery")]
        public async Task<IActionResult> UpdateGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "GeoceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Update Grocery List Item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<GroceryItem>(requestBody);

            var groceryItem = await _dbContext.GroceryItems.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (groceryItem == null)
                return new NotFoundResult();
            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                groceryItem.Name = data.Name;
                _dbContext.Update(groceryItem);
                await _dbContext.SaveChangesAsync();
            }
           

            return new OkObjectResult(groceryItem);
        }

        [FunctionName("DeleteGrocery")]
        public async Task<IActionResult> DeleteGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GeoceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Delete Grocery List Item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<GroceryItem>(requestBody);

            var groceryItem = await _dbContext.GroceryItems.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (groceryItem == null)
                return new NotFoundResult();
            
            _dbContext.Remove(groceryItem);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult(groceryItem);
        }
    }
}
