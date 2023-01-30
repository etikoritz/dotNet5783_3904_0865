using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class OrderItemXml : IOrderItem
{
    public int Add(OrderItem item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public OrderItem? Get(Func<OrderItem?, bool>? condition)
    {
        throw new NotImplementedException();
    }

    public OrderItem? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(OrderItem item)
    {
        throw new NotImplementedException();
    }
}
