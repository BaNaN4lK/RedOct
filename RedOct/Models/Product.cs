using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Models
{
    internal class Product
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public string Id { get; set; }
        public int Amout { get; set; }
        public decimal Price { get; set; }
        public string Expiration { get; set; }
        public List<ProductionPlane> ProductionPlanes { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}
