
using Microsoft.EntityFrameworkCore;
using Products_API.Models;
using Products_Repo.Models;
using Products_Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products_API.Repository
{

    public class ProductRepo :IProductRepo
    {
        private readonly XUnitProductsContext _context;
       
        public ProductRepo(XUnitProductsContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<Product>> GetCatalogPageAsync(GetCatalogDto getCatalog)
        //{
        //    var elems2Skip = (getCatalog.Page - 1) * getCatalog.MaxNumElem; // skip n pages
        //    var result = await _context.Products
        //                         .OrderByDescending(p => p.LastUpdated)
        //                         .Skip(elems2Skip)
        //                         .Take(getCatalog.MaxNumElem)
        //                         .ToArrayAsync();
        //    return _mapper.Map<IEnumerable<Product>>(result);
        //}

        //public async Task<IEnumerable<Product>> SearchProductsAsync(SearchDto search)
        //{
        //    if (!string.IsNullOrWhiteSpace(search.Name))
        //        search.Name = search.Name.Trim();
        //    var result = await _context.Products
        //                         .Where(prod => string.IsNullOrEmpty(search.Name) || prod.Name.Contains(search.Name))
        //                         .Where(prod => prod.Price >= search.MinPrice)
        //                         .Where(prod => search.MaxPrice.IsNull() || prod.Price <= search.MaxPrice.Value)
        //                         .OrderByDescending(p => p.LastUpdated)
        //                         .ToArrayAsync();
        //    return _mapper.Map<IEnumerable<Product>>(result);
        //}

        public async Task<Product> GetProduct(Guid id)
        {
            var result = await _context.Products
                                 .FirstOrDefaultAsync(prod => prod.Sku.Equals(id));
            return (result);
        }

        public async Task<Guid> AddProductAsync(Product newProduct)
        {
            try
            {
                newProduct.Id = 0;
                var productEntity = newProduct;
                productEntity.Sku = Guid.NewGuid();
                _context.Products.Add(productEntity);
                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1 ? productEntity.Sku : Guid.Empty;
            }
            catch (Exception ex)
            {
            }
            return  Guid.Empty;
        }

        public async Task<Product> EditProductAsync(Guid id, Product editProduct)
        {
            var productToEdit = _context.Products.FirstOrDefault(p => p.Sku.Equals(id));
            if (productToEdit!=null)
            {
                productToEdit.Name = editProduct.Name;
                productToEdit.Images = editProduct.Images;
                productToEdit.Sku = editProduct.Sku;
                productToEdit.Description = editProduct.Description;
                productToEdit.Price = editProduct.Price;
                productToEdit.DiscountPrice = editProduct.DiscountPrice;
                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1 ? editProduct : null;
            }
            return null;
        }

        public async Task<Product> DeleteProductAsync(Guid id)
        {
            var productToDelete = _context.Products.FirstOrDefault(p => p.Sku.Equals(id));
            if (productToDelete!=null)
            {
                _context.Products.Remove(productToDelete);
                int saveResult = await _context.SaveChangesAsync();
                return saveResult == 1 ? productToDelete : null;
            }

            return null;
        }
    }
}
