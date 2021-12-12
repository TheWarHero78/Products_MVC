using System;
using System.Collections.Generic;

#nullable disable

namespace Products_Repo.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Sku { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
    }
}
