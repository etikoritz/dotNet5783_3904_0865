using DO;
using System.ComponentModel;
using System.Security.Cryptography;
using DalApi;
using System.Data.Common;

namespace Dal;

internal class DalOrderItem: IOrderItem
{
    /// <summary>
    /// Add new OrderItem and return its ID
    /// </summary>
    /// <param name="ori"></param>
    /// <returns>OrderItem ID</returns>
    /// <exception cref="DataAlreadyExistException"></exception>
    public int Add(OrderItem ori)
    {
        int count= 0;
        if(ori.OrderID==0)
        {
            int temp = DataSource.Confing.get_ID_OrderItem;
            ori.ID = temp;
        }
        else
        {
            ori.ID = ori.OrderID;
        }
        //if (DataSource.orderItemList.Exists(orderItem => orderItem?.ID == ori.ID))
        if (DataSource.orderItemList.Exists(orderItem => orderItem?.ID == ori.ID))
        {
            //throw new DataAlreadyExistException();!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        DataSource.orderItemList.Add(ori);
        return ori.ID;
    }

    /// <summary>
    /// Get OrderItem by order ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the OrderItem</returns>
    /// <exception cref="DataNotExistException"></exception>
    public OrderItem? GetById(int id)
    {
        DO.OrderItem? orderItem = DataSource.orderItemList.FirstOrDefault(oI => oI?.ID == id);
        if (orderItem == null)
            throw new DataNotExistException();
        return orderItem ?? new();
    }

    /// <summary>
    /// Get list of all OrderItems
    /// </summary>
    /// <returns>list of OrderItem</returns>
    public IEnumerable<DO.OrderItem?> GetList(Func<DO.OrderItem?, bool>? filter = null)
    {
        return DataSource.orderItemList.ConvertAll(orderItem => orderItem);
    }

    /// <summary>
    /// Delete OrderItem by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DataNotExistException"></exception>
    public void Delete(int orderItemID)
    {
        //for (int i = 0; i < DataSource.orderItemList.Count; i++)
        //{
        //    if (DataSource.orderItemList[i]?.ID == orderItemID)
        //{
        //DataSource.orderItemList.Remove(DataSource.orderItemList.FirstOrDefault(o=>((o?.OrderID== DataSource.orderItemList[i]?.OrderID) &&(o?.ProductID== DataSource.orderItemList[i]?.ProductID))));
        try
        {
            DataSource.orderItemList.Remove(DataSource.orderItemList.FirstOrDefault(o => o?.ID == orderItemID));
        }
        catch
        {
            throw new DataNotExistException();
        }
                //for (int j = i + 1; j < DataSource.orderList.Count; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                //{
                //    DataSource.orderItemList[j] = DataSource.orderItemList[j + 1];
                //}
                //DataSource.Confing.indexOrderItem--;
                return;
        //    }
        //}
        //throw new DataNotExistException();
    }

    /// <summary>
    /// Update existing OrderItem
    /// </summary>
    /// <param name="orderItem"></param>
    /// <exception cref="DataNotExistException"></exception>
    public void Update(OrderItem orderItem)
    {
        for (int i = 0; i < DataSource.orderItemList.Count; i++)
        {
            //if (orderItem.OrderID == DataSource.orderItemList[i]?.OrderID && orderItem.ProductID== DataSource.orderItemList[i]?.ProductID)
            if (orderItem.OrderID == DataSource.orderItemList[i]?.OrderID && orderItem.ProductID== DataSource.orderItemList[i]?.ProductID)
            {
                DataSource.orderItemList[i] = orderItem;
                return;
            }
        }
        throw new DataNotExistException();
    }

    /// <summary>
    /// Get OrderItem by its order-ID and product-ID
    /// </summary>
    /// <param name="m_orderID"></param>
    /// <param name="m_productID"></param>
    /// <returns></returns>
    /// <exception cref="DataNotExistException"></exception>
    public OrderItem? Get(int m_orderID, int m_productID)
    {
        for(int i=0;i<DataSource.orderItemList.Count;i++)
        {
            //if(DataSource.orderItemList[i]?.OrderID == m_orderID && DataSource.orderItemList[i]?.ProductID == m_productID)
            if (DataSource.orderItemList[i]?.OrderID == m_orderID && DataSource.orderItemList[i]?.ProductID == m_productID)
                return DataSource.orderItemList[i];
        }
        throw new DataNotExistException();
    }

    public OrderItem? Get(Func<OrderItem?, bool>? condition)
    {
        DO.OrderItem? orderItem = DataSource.orderItemList.FirstOrDefault(o => condition(o));
        if (orderItem == null)
            throw new DataNotExistException();
        return orderItem ?? new();
    }
}
