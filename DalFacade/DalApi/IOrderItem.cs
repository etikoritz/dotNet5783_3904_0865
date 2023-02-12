using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi;

public interface IOrderItem : ICrud<DO.OrderItem>
{
    //List<OrderItem?> GetList();
    void Delete(int orderId, int productID);
}
