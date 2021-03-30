using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations.Schema;

namespace ProductDemoFuncApp
{
    public static class SaveProduct
    {
        /*
         * iBiz Task
         * expected JSON structure
         * { "id":"123", "productName":"Prepaid Card" }
         * use nuget package:: dotnet add ProductDemoFuncApp package Microsoft.Azure.WebJobs.Extensions.Storage
         * used chrome extension Restman to test post func
         *  https://visualstudiomagazine.com/articles/2018/02/21/testing-precompiled-azure-functions.aspx
         */
        [FunctionName("SaveProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Table("Product")] ICollector<ProductEntity> outTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger SaveProduct processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic productData = JsonConvert.DeserializeObject<Product>(requestBody);

            //define the row
            string sRow = DateTime.Now.Ticks.ToString();
            // Create the Entity and set the partition to catalog
            ProductEntity pe = new ProductEntity("catalog", sRow);

            pe.Id = productData.Id;
            pe.ProductName = productData.ProductName;

            outTable.Add(pe);
            //await outTable.AddAsync(pe);            
            return new OkObjectResult($"Your Product is entered with Id {pe.Id}, Thank you");
        }
    }

}