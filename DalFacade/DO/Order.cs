using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DO;

public struct Order
{
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public override string ToString() => $@"
    Order ID: {ID} 
    Customer Name: {CustomerName}
    Customer Mail - {CustomerEmail}
    Customer Address - {CustomerAddress}
    Date of order: {OrderDate}
    Date of delivery: {DeliveryDate}
    Datev of shipping: {ShipDate}";
}
