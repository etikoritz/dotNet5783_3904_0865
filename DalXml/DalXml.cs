using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
namespace Dal;

sealed internal class DalXml : IDal
{
    private DalXml() { } // constructor stage 6

    public static IDal Instance { get; } = new DalXml(); // stage 6

    public IOrder Order { get; } = new OrderXml();

    public IProduct Product { get; } = new ProductXml();

    public IOrderItem OrderItem { get; } = new OrderItemXml();
}

