using Products_API.Models;
using Products_Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products_Repo.Repository
{
  public interface IProductRepo
    {
        Task<Product> GetProduct(Guid id);
        //Task<IEnumerable<Product>> GetCatalogPageAsync(GetCatalogDto getCatalog);
        //Task<IEnumerable<Product>> SearchProductsAsync(SearchDto search);
        Task<Guid> AddProductAsync(Product newProduct);
        Task<Product> EditProductAsync(Guid id, Product newProduct);
        Task<Product> DeleteProductAsync(Guid id);
    }
}
