using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Models
{
    internal class User
    {
        public string Id { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<ProductionPlane> ProductionPlanes { get; set; } = new();
    }
}
