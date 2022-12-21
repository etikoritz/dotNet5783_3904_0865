using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DO;

public struct OrderItem
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
        OrderItem ID: {ID}
        Product ID: {ProductID}
        Order number: {OrderID}
    	Price: {Price}
    	Amount: {Amount}";


}
