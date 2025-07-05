using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product(string sku, string internalCode, string name, string description, decimal currentUnitPrice, int stockQuantity, bool isActive) : base()
        {
            InternalCode = internalCode;
            Sku = sku;
            Name = name;
            Description = description;
            CurrentUnitPrice = currentUnitPrice;
            StockQuantity = stockQuantity;
            IsActive = isActive;
        }

        public Product() { }
        public string InternalCode { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CurrentUnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }

       public void DecreaseStock(int quantity)
       {
            if (quantity <= 0) throw new ArgumentException("La cantidad a descontar debe ser mayor a cero");
            if (quantity > StockQuantity) throw new ArgumentException("No hay suficiente stock para descontar la cantidad solicitada");
            StockQuantity -= quantity;
       }
        // public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); //Opcional- Si querés navegación inversa desde Product hacia OrderItems


    }
}
