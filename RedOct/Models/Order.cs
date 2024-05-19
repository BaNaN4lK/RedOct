using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string PhoneCustomer { get; set; }
        public override string ToString()
        {
            return $"Заказ Id: {this.Id}, " +
                $"Количество: {this.Amount}, Продукт: {this.ProductId} , " +
                $"Сумма: {this.Sum}, Дата: {this.Date} , " +
                $"Статус: {this.Status}, Заказчик: {this.CustomerName}, " +
                $"Номер телефона: {this.PhoneCustomer} ";
        }
    }
}
