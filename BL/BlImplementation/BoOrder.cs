using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
using DO;
using static System.Collections.Specialized.BitVector32;

namespace BlImplementation;

internal class BoOrder : BlApi.IOrder
{
    private DalApi.IDal Dal = new Dal.DalList();
    private Dal.DalList dalList = new Dal.DalList();

    /// <summary>
    /// --inner function for BO.Order-- that recives a BO.order and return the order's status
    /// </summary>
    /// <param name="order"></param>
    /// <returns>OrderStatus</returns>
    internal BO.Enum.OrderStatus OrderStatus(BO.Order order)
    {
        BO.Enum.OrderStatus orderStatus = new BO.Enum.OrderStatus();
        if (order.DeliveryDate <= DateTime.Now)
            orderStatus = BO.Enum.OrderStatus.Delivered;
        else if (order.ShipDate <= DateTime.Now)
            orderStatus = BO.Enum.OrderStatus.Shipped;
        else
            orderStatus = BO.Enum.OrderStatus.Confirmed;
        return orderStatus;
    }

    /// <summary>
    /// --inner function for DO.Order-- that recives a DO.order and return the order's status
    /// </summary>
    /// <param name="order"></param>
    /// <returns>OrderStatus</returns>
    internal BO.Enum.OrderStatus OrderStatus(DO.Order order)
    {
        BO.Enum.OrderStatus orderStatus = new BO.Enum.OrderStatus();
        if (order.DeliveryDate <= DateTime.Now)
            orderStatus = BO.Enum.OrderStatus.Delivered;
        else if (order.ShipDate <= DateTime.Now)
            orderStatus = BO.Enum.OrderStatus.Shipped;
        else
            orderStatus = BO.Enum.OrderStatus.Confirmed;
        return orderStatus;
    }

    /// <summary>
    /// Get OrderList from DO (manager screen)
    /// </summary>
    /// <returns>Order list</returns>
    public IEnumerable<BO.OrderForList> GetOrderList()
    {
        //get the orders list from dal
        List<DO.Order?> DalOrdersList = (List<DO.Order?>)Dal.Order.GetList();

        //creates new BO orders list
        List<BO.OrderForList> ordersList = new();

        foreach (DO.Order order in DalOrdersList)
        {
            //creates new OrderForList items from the dal orders list
            BO.OrderForList ord = new()
            {
                ID = order.ID,
                CustomerName = order.CustomerName,
            };
            //order Status
            ord.Status = OrderStatus(order);

            ord.AmountOfItems = 0;
            //order TotalPrice
            ord.TotalPrice = 0;

            List<DO.OrderItem?> orderItemList = (List<DO.OrderItem?>)dalList.OrderItem.GetList();
            //to get all the products from the specific order
            foreach (DO.OrderItem item in orderItemList)
            {
                if (item.OrderID == order.ID)
                {
                    ord.AmountOfItems += item.Amount;
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
                DO.Order dalOrder = (DO.Order)Dal.Order?.GetById(orderID);
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
                order.Status = OrderStatus(order);

                //order TotalPrice
                order.TotalPrice = 0;
                List<DO.OrderItem> orderItemList = (List<DO.OrderItem>)dalList.OrderItem.GetList();
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
                List<DO.OrderItem> orderItemList2 = (List<DO.OrderItem>)dalList.OrderItem.GetList();
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
                        item1.Name = Dal.Product.GetById(item.ProductID).Value.Name;
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
            DO.Order dalOrder = (DO.Order)Dal.Order.GetById(orderID);
            if (dalOrder.ShipDate == null || dalOrder.ShipDate > DateTime.Today)
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
            DO.Order dalOrder = (DO.Order)Dal.Order.GetById(orderID);
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
            DO.Order dalOrder = (DO.Order)Dal.Order.GetById(orderID);
            orderTracking.ID = dalOrder.ID;
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


    /// <summary>
    /// --inner function for manager fonction-- deleting an item from the order
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    internal void deleteItemFromOrder(int orderID, int productID)
    {
        List<DO.OrderItem> orderItems = (List<DO.OrderItem>)Dal.OrderItem.GetList();
        bool OnlyOneItemInOrder = true;
        foreach (var item in orderItems)
        {
            if (item.OrderID == orderID)
            {
                if (item.ProductID == productID)
                {
                    DO.Product product = (DO.Product)Dal.Product.GetById(productID);
                    product.InStock += item.Amount;
                    Dal.Product.Update(product);
                    Dal.OrderItem.Delete(item.ID);
                }
                else
                {
                    OnlyOneItemInOrder = false;
                }

            }
        }
        //if the order has only one item-the one we deleted, we'll delete the order
        if (OnlyOneItemInOrder == true)
        {
            Dal.Order.Delete(orderID);
        }
        //foreach (var item in orderForLists)
        //{
        //    if (item.ID == orderID)
        //    {
        //        orderForLists.Add(item);
        //    }
        //}
        return;
    }

    /// <summary>
    /// --inner function for manager fonction-- adding new product to an order (manager fonction)
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    /// <exception cref="OutOfStockProductException"></exception>
    internal void addItemToOrder(int orderID, int productID)
    {
        List<DO.OrderItem> orderItems = (List<DO.OrderItem>)Dal.OrderItem.GetList();
        DO.Product product = (DO.Product)Dal.Product.GetById(productID);
        foreach (var item in orderItems)
        {
            if (item.OrderID == orderID)
            {
                if ((product.InStock - 1) >= 0)
                {
                    product.InStock--;
                    Dal.Product.Update(product);
                }
                else
                {
                    throw new OutOfStockProductException();
                }
                ///if the product alredy exsist in the order, we will just update it and add 1 to the amount.
                if (item.ProductID == productID)
                {
                    addAmuntToItemInOrder(orderID, productID, 1);
                    return;
                }
                else
                {
                    DO.OrderItem NewItemDal = new()
                    {
                        OrderID = orderID,
                        ProductID = productID,
                        Price = Dal.Product.GetById(productID).Value.Price,
                        Amount = 1,
                    };

                    int id = Dal.OrderItem.Add(NewItemDal);
                    NewItemDal.ID = id;
                }

            }
        }
        //foreach (var item in orderForLists)
        //{
        //    if (item.ID == orderID)
        //    {
        //        orderForLists.Add(item);
        //    }
        //}
        return;
    }

    /// <summary>
    /// --inner function for manager fonction-- updating the AMOUNT of an item in the order (manager fonction)
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    /// <param name="amount"></param>
    /// <exception cref="OutOfStockProductException"></exception>
    internal void addAmuntToItemInOrder(int orderID, int productID, int amount)
    {
        List<DO.OrderItem> orderItems = (List<DO.OrderItem>)Dal.OrderItem.GetList();
        DO.Product product = (DO.Product)Dal.Product.GetById(productID);
        for (int i = 0; i < orderItems.Count; i++)
        {
            if (orderItems[i].ProductID == productID && orderItems[i].OrderID == orderID)
            {
                if (product.InStock - amount < 0)
                {
                    throw new OutOfStockProductException();
                }
                int Amount = 0;
                Amount = orderItems[i].Amount + amount;
                product.InStock -= amount;
                Dal.Product.Update(product);

                DO.OrderItem NewItemDal = new()
                {
                    ID = orderID,
                    OrderID = orderID,
                    ProductID = productID,
                    Price = Dal.Product.GetById(productID).Value.Price,

                };
                NewItemDal.Amount = Amount;
                NewItemDal.TotalPrice += Amount * orderItems[i].Price;
                Dal.OrderItem.Update(NewItemDal);
                return;
            }
        }
    }


    /// <summary>
    /// updating an order Allows adding a product, deleting
    /// a product, and updating the amount product in the order
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    /// <param name="action"></param>
    /// <param name="amount"></param>
    /// <exception cref="OutOfStockProductException"></exception>
    /// 
    public void UpdateOrderByManager(int orderID, int productID, string action, int amount)
    {
        //deleting an item from the order
        if (action == "remove")
        {
            deleteItemFromOrder(orderID, productID);
        }

        //adding new product to an order
        if (action == "add")
        {
            addItemToOrder(orderID, productID);
        }

        //updating the AMOUNT of an item in the order
        if (action == "addAmount")
        {
            addAmuntToItemInOrder(orderID, productID, amount);
        }
    }
}

