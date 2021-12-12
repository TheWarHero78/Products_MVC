using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Products_MVC.Models;
using Products_MVC.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Products_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _proudctService;

        public ProductsController(IProductService productService) =>
            _proudctService = productService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("AddProduct")]
        public IActionResult AddProduct()
        {
            ViewData["Message"] = "Add Product";

            return View();
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(Products newProduct)
        {
            if (newProduct.file != null && newProduct.file.Count > 0)
            {
                foreach (var file in newProduct.file)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);

                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(myUniqueFileName, fileExtension);
                    newProduct.Images = newFileName + ",";
                    // Combines two strings into a path.
                    var filepath =
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newFileName}";

                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }                //.Path = filePath;
                }
            }
            var result = await _proudctService.AddProductAsync(newProduct);
            if (result)
                return RedirectToAction("Index");
            return BadRequest("Add product error");
        }

        [HttpGet]
        [Route("EditProduct/{id}")]
        public async Task<IActionResult> EditProduct([FromRoute] Guid id)
        {
            ViewData["Message"] = "Edit Product";
            var products = await _proudctService.GetProduct(id);
            return View(products);
        }

        [HttpGet]
        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct([FromQuery] Products newProduct)
        {
            var result = await _proudctService.EditProductAsync(newProduct);
            if (result != null)
                return RedirectToAction("Index");
            return BadRequest("Edit product error");
        }
    }
}
