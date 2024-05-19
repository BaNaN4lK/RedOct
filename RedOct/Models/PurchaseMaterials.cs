using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Models
{
    class PurchaseMaterial
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string MaterialId { get; set; }
        public Material Material { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public string ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
