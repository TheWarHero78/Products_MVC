using Products_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products_MVC.Services
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(Products newProduct);
        Task<IEnumerable<Products>> SearchProductsAsync(string productName);
        Task<Products> DeleteProductAsync(Guid id);
        Task<Products> EditProductAsync(Products newProduct);
        Task<Products> GetProduct(Guid id);

    }
}
