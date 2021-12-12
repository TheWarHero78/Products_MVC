using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Products_MVC.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Sku { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
     
        public string Images { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public List<IFormFile> file { get; set; }


    }
}
