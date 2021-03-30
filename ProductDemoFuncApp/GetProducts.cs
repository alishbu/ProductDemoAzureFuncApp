using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
//using Newtonsoft.Json;

namespace ProductDemoFuncApp
{
    public static class GetProducts
    {

        // https://markheath.net/post/azure-functions-rest-csharp-bindings
        // couldn't run with the ref using Microsoft.WindowsAzure.Storage.Table, so replaced that with 
        // using Microsoft.Azure.Cosmos.Table;
        // used chrome extension Restman to test get func
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        [Table("Product")] CloudTable prodTable,
        ILogger log)
        {
            log.LogInformation("Getting products .... ");
            var query = new TableQuery<ProductEntity>();
            var entities = new List<ProductEntity>();

            TableContinuationToken token = null;
            do
            {                
                TableQuerySegment<ProductEntity> segment = await prodTable.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                entities.AddRange(segment);

            } while (token != null && (query.TakeCount == null || entities.Count < query.TakeCount.Value));
            return new OkObjectResult(ProductMappings.ToProductList(entities));
        }
        
    }
}


