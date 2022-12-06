using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlApi;


namespace BlImplementation;

internal class BoOrder : IOrder
{
    private DalApi.IDal Dal = new Dal.DalList();
    private Dal.DalList dalList = new Dal.DalList();

    /// <summary>
    /// Get OrderList from DO (manager screen)
    /// </summary>
    /// <returns>Order list</returns>
    public IEnumerable<BO.OrderForList> GetOrderList()
    {
        //get the orders list from dal
        List<DO.Order> DalOrdersList = Dal.Order.GetList();

        //creates new BO orders list
        List<BO.OrderForList> ordersList = new();

        foreach (DO.Order order in DalOrdersList)
        {
            //creates new OrderForList items from the dal orders list
            BO.OrderForList ord = new()
            {
                ID= order.ID,
                CustomerName= order.CustomerName
            };
            //order Status
            if (order.DeliveryDate <= DateTime.Now)
                ord.Status = BO.Enum.OrderStatus.Delivered;
            else if (order.ShipDate <= DateTime.Now)
                ord.Status = BO.Enum.OrderStatus.Shipped;
            else
                ord.Status= BO.Enum.OrderStatus.Confirmed;

            //order AmountOfItems
            ord.AmountOfItems = dalList.OrderItem.GetById(order.ID).Amount;

            //order TotalPrice
            ord.TotalPrice = 0;
            List <DO.OrderItem> orderItemList = dalList.OrderItem.GetList();
            //to get all the products from the specific order
            foreach(DO.OrderItem item in orderItemList)
            {
                if(item.OrderID==order.ID)
                {
                    ord.TotalPrice += item.Price * item.Amount;
                }
            }

            //Add the new order to the list
            ordersList.Add(ord);
        }
        return ordersList;
    }

    /// <summary>
    /// Get order details by its ID
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns>the order</returns>
    /// <exception cref="BO.BODataNotExistException"></exception>
    /// <exception cref="BO.NegativeIdException"></exception>
    public BO.Order GetOrderDetails(int orderID)
    {
        if (orderID > 0) 
        {
            try
            {
                DO.Order dalOrder = Dal.Order.GetById(orderID);
                BO.Order order = new()
                {
                    ID = dalOrder.ID,
                    CustomerName = dalOrder.CustomerName,
                    CustomerEmail = dalOrder.CustomerEmail,
                    CustomerAddress = dalOrder.CustomerAddress,
                    OrderDate = dalOrder.OrderDate,
                    ShipDate = dalOrder.ShipDate,
                    DeliveryDate = dalOrder.DeliveryDate
                };
                //order Status
                if (order.DeliveryDate <= DateTime.Now)
                    order.Status = BO.Enum.OrderStatus.Delivered;
                else if (order.ShipDate <= DateTime.Now)
                    order.Status = BO.Enum.OrderStatus.Shipped;
                else
                    order.Status = BO.Enum.OrderStatus.Confirmed;

                //order TotalPrice
                order.TotalPrice = 0;
                List<DO.OrderItem> orderItemList = dalList.OrderItem.GetList();
                //to get all the products from the specific order
                foreach (DO.OrderItem item in orderItemList)
                {
                    if (item.OrderID == order.ID)
                    {
                        order.TotalPrice += item.Price * item.Amount;
                    }
                }

                //order Items
                order.Items = new List<BO.OrderItem>();
                List <DO.OrderItem> orderItemList2 = dalList.OrderItem.GetList();
                foreach (DO.OrderItem item in orderItemList2)
                {
                    if (item.OrderID == order.ID)
                    {
                        BO.OrderItem item1 = new()
                        {
                            ID = item.ID,
                            ProductID = item.ProductID,
                            Price = item.Price,
                            Amount = item.Amount
                        };
                        item1.TotalPrice = item.Price * item.Amount;
                        item1.Name = Dal.Product.GetById(item.ProductID).Name; 
                        order.Items.Add(item1);
                    }
                }
                return order;
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
        }
        else
            throw new BO.NegativeIdException();
    }

    /// <summary>
    /// Update order delivery by order ID
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns>the updated order</returns>
    /// <exception cref="BO.BODataNotExistException"></exception>
    /// <exception cref="BO.OrderAlreadyShippedException"></exception>
    public BO.Order UpdateOrderDelivery(int orderID)
    {
        BO.Order order = new BO.Order();
        try
        {
            DO.Order dalOrder = Dal.Order.GetById(orderID);
            if (dalOrder.ShipDate == null || dalOrder.ShipDate>DateTime.Today)
            {
                //update BO entity
                order = GetOrderDetails(orderID);
                order.ShipDate = DateTime.Now;
                //update DO entity
                dalOrder.ShipDate = DateTime.Now;
                try
                {
                    Dal.Order.Update(dalOrder);
                }
                catch (DO.DataNotExistException ex)
                {
                    throw new BO.BODataNotExistException(ex.Message);
                }
            }
            else
                throw new BO.OrderAlreadyShippedException();
        }
        catch (DO.DataNotExistException ex)
        {
            throw new BO.BODataNotExistException(ex.Message);
        }
        return order;
    }

    /// <summary>
    /// Update order supply by order ID
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns>the updated order</returns>
    /// <exception cref="BO.BODataNotExistException"></exception>
    /// <exception cref="BO.OrderAlreadySuppliedException"></exception>
    public BO.Order UpdateOrderSupply(int orderID)
    {
        BO.Order order = new BO.Order();
        try
        {
            DO.Order dalOrder = Dal.Order.GetById(orderID);
            if (dalOrder.ShipDate != null && dalOrder.DeliveryDate == null)
            {
                //update BO entity
                order = GetOrderDetails(orderID);
                order.DeliveryDate = DateTime.Today;
                //update DO entity
                dalOrder.DeliveryDate = DateTime.Today;
                try
                {
                    Dal.Order.Update(dalOrder);
                }
                catch (DO.DataNotExistException ex)
                {
                    throw new BO.BODataNotExistException(ex.Message);
                }
            }
            else
                throw new BO.OrderAlreadySuppliedException();
        }
        catch (DO.DataNotExistException ex)
        {
            throw new BO.BODataNotExistException(ex.Message);
        }
        return order;
    }



    /// <summary>
    /// Track order by its ID
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns>OrderTracking</returns>
    /// <exception cref="BO.BODataNotExistException"></exception>
    public BO.OrderTracking TrackOrder(int orderID)
    {
        BO.Order order = new BO.Order();
        BO.OrderTracking orderTracking = new BO.OrderTracking();
        try
        {
            DO.Order dalOrder = Dal.Order.GetById(orderID);
            orderTracking.ID= dalOrder.ID;
            //orderTracking Status
            if (order.PaymentDate <= DateTime.Today)
            {
                orderTracking.Status = BO.Enum.OrderStatus.Confirmed;
                Tuple<DateTime?, BO.Enum.OrderStatus> tuple = Tuple.Create(order.PaymentDate, order.Status);
                orderTracking.DateAndPrograss[0] = tuple;
            }
            if (order.DeliveryDate <= DateTime.Today)
            {
                orderTracking.Status = BO.Enum.OrderStatus.Delivered;
                Tuple<DateTime?, BO.Enum.OrderStatus> tuple = Tuple.Create(order.DeliveryDate, order.Status);
                orderTracking.DateAndPrograss[1] = tuple;
            }
            if (order.ShipDate <= DateTime.Today)
            {
                orderTracking.Status = BO.Enum.OrderStatus.Shipped;
                Tuple<DateTime?, BO.Enum.OrderStatus> tuple = Tuple.Create(order.ShipDate, order.Status);
                orderTracking.DateAndPrograss[2] = tuple;
            }
        }
        catch (DO.DataNotExistException ex)
        {
            throw new BO.BODataNotExistException(ex.Message);
        }
        return orderTracking;
    }

    public BO.Order UpdateOrderByManager(int orderID)
    {
        throw new NotImplementedException();
    }
}
