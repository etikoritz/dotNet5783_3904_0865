using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Cart
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public List<OrderItem> Items { get; set; }
        public double TotalPrice { get; set; }

        public override string ToString() => $@"
        Customer name: {CustomerName} 
        Customer Email: {CustomerEmail} 
        Customer address: {CustomerAddress} 
        Items: {Items} 
        Total price: {TotalPrice}"; 
    }
}
