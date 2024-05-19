using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RedOct.Models
{
    internal class ProductionPlane
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public DateTime Date{ get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}
