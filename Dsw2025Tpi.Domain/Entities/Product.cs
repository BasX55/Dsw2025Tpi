using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product(string sku, string name, string description, decimal currentUnitPrice, int stockQuantity, bool isActive) : base()
        {
            Sku = sku;
            Name = name;
            Description = description;
            CurrentUnitPrice = currentUnitPrice;
            StockQuantity = stockQuantity;
            IsActive = isActive;
            
        }

        public Product() { }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CurrentUnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }


    }
}
