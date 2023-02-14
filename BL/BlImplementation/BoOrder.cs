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
    private DalApi.IDal Dal = DalApi.Factory.Get();
    //private Dal.DalList dalList = new Dal.DalList();

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
    public IEnumerable<BO.OrderForList> GetOrderList(Func<BO.Product?, bool>? condition = null)
    {
        //get the orders list from dal
        IEnumerable<DO.Order?> DalOrdersList = (IEnumerable<DO.Order?>)DalApi.Factory.Get().Order?.GetList();

        //creates new BO orders list
        List<BO.OrderForList> ordersList = new();
        foreach (var (order, ord) in from DO.Order order in DalOrdersList
                                     //creates new OrderForList items from the dal orders list
                                     let ord = new BO.OrderForList()
                                     {
                                         ID = order.ID,
                                         CustomerName = order.CustomerName,
                                     }
                                     select (order, ord))
        {
            //order Status
            ord.Status = OrderStatus(order);
            ord.AmountOfItems = 0;
            //order TotalPrice
            ord.TotalPrice = 0;
            //List<DO.OrderItem?> orderItemList = (List<DO.OrderItem?>)dalList.OrderItem.GetList();
            IEnumerable<DO.OrderItem?> orderItemList = (IEnumerable<DO.OrderItem?>)DalApi.Factory.Get().OrderItem?.GetList();
            //to get all the products from the specific order
            foreach (var item in
            from DO.OrderItem item in orderItemList
            where item.OrderID == order.ID
            select item)
            {
                ord.AmountOfItems += item.Amount;
                ord.TotalPrice += item.Price * item.Amount;
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
    public BO.Order GetOrderDetails(int id)
    {
        //if (condition > 0)
        //{
            try
            {
                DO.Order dalOrder=(DO.Order)DalApi.Factory.Get().Order?.Get(o=>o?.ID==id);
                //DO.Order dalOrder = (DO.Order)Dal.Order.GetById(orderID);
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
                //List<DO.OrderItem?> orderItemList = (List<DO.OrderItem?>)dalList.OrderItem?.GetList();
                IEnumerable<DO.OrderItem?> orderItemList = (IEnumerable<DO.OrderItem?>)DalApi.Factory.Get().OrderItem?.GetList();
                //to get all the products from the specific order
                foreach (var item in
                         from DO.OrderItem item in orderItemList
                         where item.OrderID == order.ID
                         select item)
                {
                    order.TotalPrice += item.Price * item.Amount;
                }


            //order Items
            order.Items = new List<BO.OrderItem>();
                IEnumerable<DO.OrderItem?> orderItemList2 = (IEnumerable<DO.OrderItem?>)DalApi.Factory.Get().OrderItem?.GetList();
            foreach (var (item, item1) in from DO.OrderItem item in orderItemList2
                                          where item.OrderID == order.ID
                                          let item1 = new BO.OrderItem()
                                          {
                                              ID = item.ID,
                                              ProductID = item.ProductID,
                                              Price = item.Price,
                                              Amount = item.Amount
                                          }
                                          select (item, item1))
            {
                item1.TotalPrice = item.Price * item.Amount;
                item1.Name = DalApi.Factory.Get().Product.GetById(item.ProductID)?.Name;////////////////////////////////////////////////////////////////////////////////
                order.Items.Add(item1);
            }

            return order;
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
    }

    /// <summary>
    /// Update order delivery by order ID
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    /// <exception cref="BO.BODataNotExistException"></exception>
    /// <exception cref="BO.OrderAlreadyShippedException"></exception>
    public BO.Order UpdateOrderDelivery(int orderID, DateTime date)
    {
        BO.Order order = new BO.Order();
        try
        {
            DO.Order dalOrder = (DO.Order)DalApi.Factory.Get().Order?.Get(o=>o?.ID ==orderID);
            if (dalOrder.ShipDate >= DateTime.Today)
            {
                //update BO entity
                order = GetOrderDetails(orderID);
                if (date > DateTime.MinValue)
                {
                    order.ShipDate = DateTime.Today;
                    //update DO entity
                    dalOrder.ShipDate = DateTime.Today;
                }
                else
                {
                    order.ShipDate = date;
                    dalOrder.ShipDate = date;
                }
                
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
    /// <param name="date"></param>
    /// <returns></returns>
    /// <exception cref="BO.BODataNotExistException"></exception>
    /// <exception cref="BO.OrderAlreadySuppliedException"></exception>
    public BO.Order UpdateOrderSupply(int orderID, DateTime date)
    {
        BO.Order order = new BO.Order();
        try
        {
            DO.Order dalOrder = (DO.Order)DalApi.Factory.Get().Order?.GetById(orderID);
            if (dalOrder.DeliveryDate>DateTime.Today)
            {
                order = GetOrderDetails(orderID);
                if (date==DateTime.MinValue)
                {
                    //update BO entity
                   
                    order.DeliveryDate = DateTime.Today;
                    //update DO entity
                    dalOrder.DeliveryDate = DateTime.Today;
                }
                else
                {
                    order.DeliveryDate = date;
                    dalOrder.DeliveryDate = date;
                }
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
    /// --inner function for manager-- deleting an item from order
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    internal void deleteItemFromOrder(int orderID, int productID)
    {
        IEnumerable<DO.OrderItem?>? orderItems = (IEnumerable<DO.OrderItem?>?)DalApi.Factory.Get()?.OrderItem.GetList();
        foreach (var item in from item in orderItems
                             where item?.OrderID == orderID
                             where item?.ProductID == productID
                             select item)
        {
            DO.Product product = (DO.Product)Dal.Product.GetById(productID);
            product.InStock += item.Value.Amount;
            Dal.Product.Update(product);
            Dal.OrderItem.Delete(item.Value.ID, productID);
        }

        //if the order has only one item-the one we deleted, we'll delete the order
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
        IEnumerable<DO.OrderItem?> orderItems = (IEnumerable<DO.OrderItem?>)DalApi.Factory.Get().OrderItem?.GetList();
        DO.Product product = (DO.Product)DalApi.Factory.Get().Product.GetById(productID);
        foreach (var _ in from item in orderItems
                          where item?.OrderID == orderID
                          where item?.ProductID == productID
                          select new { })
        {
            if ((product.InStock - 1) >= 0)
            {
                product.InStock--;
                Dal.Product.Update(product);
            }
            else
            {
                throw new OutOfStockProductSException("the product is out of stock!");
            }

            addAmuntToItemInOrder(orderID, productID, 1);
            return;
        }

        foreach (var x in from item in orderItems
                          where item?.OrderID == orderID
                          select new { })
        {
            if ((product.InStock - 1) >= 0)
            {
                product.InStock--;
                Dal.Product.Update(product);
            }
            else
            {
                throw new OutOfStockProductSException("the product is out of stock!");
            }

            DO.OrderItem NewItemDal = new()
            {
                OrderID = orderID,
                ProductID = productID,
                Price = DalApi.Factory.Get().Product.GetById(productID).Value.Price,
                Amount = 1,
            };
            int id = DalApi.Factory.Get().OrderItem.Add(NewItemDal);
            NewItemDal.ID = id;
            return;
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
    /// updating the AMOUNT of an item in the order (manager fonction)
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    /// <param name="amount"></param>
    /// <exception cref="OutOfStockProductException"></exception>
    public void addAmuntToItemInOrder(int orderID, int productID, int amount)
    {
        IEnumerable<DO.OrderItem?> orderItems = (IEnumerable<DO.OrderItem?>)DalApi.Factory.Get().OrderItem.GetList();
        DO.Product product = (DO.Product)DalApi.Factory.Get()?.Product.GetById(productID);
        DO.OrderItem item = (DO.OrderItem)orderItems.FirstOrDefault(o => (o?.OrderID == orderID && o?.ProductID == productID));

        if (product.InStock - amount < 0)
        {
            throw new OutOfStockProductSException("the product is out of stock!");
        }
        item.Amount += amount;
        item.TotalPrice+= amount * product.Price;
        Dal.OrderItem.Update(item);
        product.InStock -= amount;
        Dal.Product.Update(product);
        return;
    }

    /// <summary>
    /// updating the AMOUNT of an item in the order (manager fonction)
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="productID"></param>
    /// <param name="amount"></param>
    public void SubtractAmuntToItemInOrder(int orderID, int productID, int amount)
    {
        IEnumerable<DO.OrderItem?>? orderItems = (IEnumerable<DO.OrderItem?>?)DalApi.Factory.Get()?.OrderItem.GetList();
        DO.Product product = (DO.Product)DalApi.Factory.Get()?.Product?.GetById(productID);

        DO.OrderItem? orderItem = (DO.OrderItem?)DalApi.Factory.Get()?.OrderItem.Get(o => (o?.ProductID == productID) && (o?.OrderID == orderID));
       
        if (orderItem!=null)
        {
            if (orderItem?.Amount - amount >= 0)
            {
                int tempAmount = (int)orderItem?.Amount - amount;
                product.InStock += amount;
                Dal.Product.Update(product);
                DO.OrderItem NewItemDal = new()
                {
                    ID = orderID,
                    OrderID = orderID,
                    ProductID = productID,
                    Price=product.Price,

                };
                NewItemDal.Amount = tempAmount;
                NewItemDal.TotalPrice -= amount *product.Price;
                Dal.OrderItem.Update(NewItemDal);
            }
        }
        if (orderItem?.Amount - amount == 0)
        {
            deleteItemFromOrder(orderID, productID);
            //throw new Exception();
        }


        //if(product.InStock-amount>0)
        //{
        //    for (int i = 0; i < orderItems?.Count; i++)
        //    {
        //        if (orderItems[i]?.ProductID == productID && orderItems[i]?.OrderID == orderID)
        //        {
        //            int Amount = 0;
        //            Amount = orderItems[i].Value.Amount - amount;
        //            product.InStock += amount;
        //            Dal.Product.Update(product);

        //            DO.OrderItem NewItemDal = new()
        //            {
        //                ID = orderID,
        //                OrderID = orderID,
        //                ProductID = productID,
        //                Price = DalApi.Factory.Get().Product.Get(p => p?.ID == productID).Value.Price,

        //            };
        //            NewItemDal.Amount = Amount;
        //            NewItemDal.TotalPrice -= amount * orderItems[i].Value.Price;
        //            Dal.OrderItem.Update(NewItemDal);
        //            return;
        //        }
        //    }
        //}

        ////////////////////////// מאפשרים להוסיף רק אחד בכל פעם PL -אולי לא צריך, כי ב  
        //if (product.InStock - amount < 0)
        //{
        //    throw new Exception(
        //}

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
    public void UpdateOrderByManager(int orderID, int productID, string action, int amount=1)
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

        if (action == "subtract")
        {
            SubtractAmuntToItemInOrder(orderID, productID, amount);
        }
    }

    /// <summary>
    /// Find the oldest order exist by orderDate (for the simulator)
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int? GetOldestOrderID()
    {
        IEnumerable<OrderForList> orderList = GetOrderList();
        IEnumerable<BO.Order> orders = orderList.Select(o => GetOrderDetails(o.ID));
        try
        {
            //find the order with the oldest orderDate
            BO.Order? oldestOrder = orders.Where(o => o.Status.ToString() == "Confirmed").OrderBy(o => o.OrderDate).FirstOrDefault();
            //find the order with the oldest shippingDate
            BO.Order? oldestShipping = orders.Where(o => o.Status.ToString() == "Shipped").OrderBy(o => o.ShipDate).FirstOrDefault();

            if (oldestOrder.OrderDate < oldestShipping.ShipDate)
                return oldestOrder.ID;
            return oldestShipping.ID;
        }
        catch (Exception ex)
        {
            throw new Exception("Error finding order");
        }
        return null;
    }


    //public int ChooseNextOrder()
    //{
    //    IEnumerable<OrderForList> orderList = GetOrderList();
    //    IEnumerable<BO.Order> orders = orderList.Select(o => GetOrderDetails(o.ID));
    //    IEnumerable<BO.Order> shippedOrders = orders.Where(o => o.Status.ToString() == "Shipped");
    //    IEnumerable<BO.Order> confirmedOrders = orders.Where(o => o.Status.ToString() == "Confirmed");
    //    BO.Order? maxObjectShipped = shippedOrders.OrderByDescending(x => x.ShipDate).FirstOrDefault();
    //    BO.Order? maxObjectConfirmed = confirmedOrders.OrderByDescending(x => x.OrderDate).FirstOrDefault();
    //    if (maxObjectShipped?.ShipDate > maxObjectConfirmed?.OrderDate)
    //        return maxObjectConfirmed.ID;
    //    else
    //        return maxObjectShipped.ID;
    //}
}

