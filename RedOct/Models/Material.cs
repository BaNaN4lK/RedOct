using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Models
{
    internal class Material
    {
        public string Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Amount { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public List<PurchaseMaterial> PurchaseMaterials { get; set; } = new();
    }
}
