using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductDemoFuncApp
{
    public class Product
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductEntity : TableEntity
    {
        public ProductEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public ProductEntity() { }

        public string Id { get; set; }
        public string ProductName { get; set; }
    }

    public static class ProductMappings
    {
        /*
        public static TodoTableEntity ToTableEntity(this Todo todo)
        {
            return new TodoTableEntity()
            {
                PartitionKey = "TODO",
                RowKey = todo.Id,
                CreatedTime = todo.CreatedTime,
                IsCompleted = todo.IsCompleted,
                TaskDescription = todo.TaskDescription
            };
        }
        */
        public static Product ToProduct(this ProductEntity pe)
        {
            return new Product()
            {
                Id = pe.Id,
                ProductName = pe.ProductName
            };
        }

        public static List<Product> ToProductList(List<ProductEntity> peList)
        {
            List<Product> pList = new List<Product>();
            foreach (ProductEntity pe in peList)
            {
                pList.Add(ProductMappings.ToProduct(pe));
            }
            return pList;
        }
    }
}
