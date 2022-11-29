using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BlImplementation
{
    internal class BoOrder : IOrder
    {
        public BO.Order GetOrderDetails(int orderID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BO.OrderForList> GetOrderList()
        {
            throw new NotImplementedException();
        }

        public BO.OrderTracking TrackOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public BO.Order UpdateOrderByManager(int orderID)
        {
            throw new NotImplementedException();
        }

        public BO.Order UpdateOrderDelivery(int orderID)
        {
            throw new NotImplementedException();
        }

        public BO.Order UpdateOrderSupply(int orderID)
        {
            throw new NotImplementedException();
        }
    }
}
