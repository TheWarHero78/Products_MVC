using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Products_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Products_MVC.Services
{
    public class ProductService:IProductService
    {
        private readonly string _remoteServiceBaseUrl;
        private readonly HttpClient hc;

        public ProductService(IConfiguration configuration)
        {
            Configuration = configuration;
            _remoteServiceBaseUrl = Configuration.GetValue<string>("WebProducts.API_endpoint");
            hc = new HttpClient();
            hc.BaseAddress = new Uri(_remoteServiceBaseUrl);
            hc.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IConfiguration Configuration { get; }

        // GET api/Products?page=4
        public async Task<IEnumerable<Products>> GetProductsPageAsync(int page, int maxNumElem = 10)
        {
            string ProductsPageUri = $"?page={page}&maxNumElem={maxNumElem}";
            var response = await hc.GetAsync(ProductsPageUri);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsAsync<IEnumerable<Products>>();
            return products;
        }

        public async Task<Products> GetProduct(Guid id)
        {
            string getProductUri = $"/api/Products/GetProduct?id={id}";
            var response = await hc.GetAsync(getProductUri);
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadAsAsync<Products>();

            return product;
        }

        public async Task<List<Products>> GetProducts()
        {
            string searchProductUri = $"/api/Products/GetProducts";
            var response = await hc.GetAsync(searchProductUri);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsAsync<List<Products>>();
            return products;
        }

        public async Task<bool> AddProductAsync(Products newProduct)
        {
            string addProductUri = "/api/Products/AddProduct";

            string json = JsonConvert.SerializeObject(newProduct);
            var tmp = new StringContent(json, Encoding.UTF8);
            var response = await hc.PutAsJsonAsync(addProductUri, newProduct);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public async Task<Products> DeleteProductAsync(Guid id)
        {
            string deleteProductUri = $"/api/Products/DeleteProduct?id={id}";
            var response = await hc.DeleteAsync(deleteProductUri);
            response.EnsureSuccessStatusCode(); // Throw on error code.
            string dataString = await response.Content.ReadAsStringAsync();
            Products product = JsonConvert.DeserializeObject<Products>(dataString);

            return product;
        }

        public async Task<Products> EditProductAsync(Products editProduct)
        {
            string EditProductUri = $"/api/Products/EditProduct?id={editProduct.Sku}";
            string json = JsonConvert.SerializeObject(editProduct);
            var tmp = new StringContent(json, Encoding.UTF8);
            var response = await hc.PutAsync(EditProductUri,
                new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode(); // Throw on error code.

            string dataString = await response.Content.ReadAsStringAsync();
            Products product = JsonConvert.DeserializeObject<Products>(dataString);

            return product;
        }
    }
}

