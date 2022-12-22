using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using Dal;
using DalApi;

namespace BlApi
{
    public interface IOrder
    {
        /// <summary>
        /// Get order list from Dal
        /// --Manager screen--
        /// </summary>
        /// <returns>BO order list</returns>
        public IEnumerable<OrderForList?> GetOrderList();

        /// <summary>
        /// get order details by its ID
        /// --Manager and Buyer screen--
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order GetOrderDetails(int orderID);

        /// <summary>
        /// Update order delivery date in Dal and BO
        /// --Manager screen--
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>BO updated order</returns>
        public Order UpdateOrderDelivery(int orderID);

        /// <summary>
        /// Update order supply date in Dal and BO
        /// --Manager screen--
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>BO updated order</returns>
        public Order UpdateOrderSupply(int orderID);

        /// <summary>
        /// Get order tracking information by its ID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>OrderTracking details</returns>
        public OrderTracking TrackOrder(int orderID);

        /// <summary>
        /// BONUS - Update order for manager
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public void UpdateOrderByManager(int orderID, int productID, string action, int amount);

    }
}
