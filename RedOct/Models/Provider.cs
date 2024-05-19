using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Models
{
    internal class Provider
    {
        public string Id { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<PurchaseMaterial> PurchaseMaterials { get; set; } = new();
    }
}
