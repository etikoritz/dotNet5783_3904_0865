using DO;
using System.ComponentModel;
using System.Security.Cryptography;
using DalApi;

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
        if(ori.OrderID==0)
        {
            int temp = DataSource.Confing.get_ID_OrderItem;
            ori.ID = temp;
        }
        else
        {
            ori.ID = ori.OrderID;
        }
       
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
    public OrderItem GetById(int id)
    {
        for (int i = 0; i < DataSource.orderItemList.Count; i++)
        {
            if (DataSource.orderItemList[i]?.ID == id)
                return DataSource.orderItemList[i];
        }
        throw new DataNotExistException();
    }

    /// <summary>
    /// Get list of all OrderItems
    /// </summary>
    /// <returns>list of OrderItem</returns>
    public List<OrderItem?> GetList()
    {
         return new List<OrderItem?>(DataSource.orderItemList);
    }

    /// <summary>
    /// Delete OrderItem by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DataNotExistException"></exception>
    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.orderItemList.Count; i++)
        {
            if (DataSource.orderItemList[i]?.ID == id)
            {
                DataSource.orderItemList.RemoveAt(i);
                for (int j = i+1; j < DataSource.orderList.Count; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                {
                    DataSource.orderItemList[j] = DataSource.orderItemList[j + 1];
                }
                //DataSource.Confing.indexOrderItem--;
                return;
            }
        }
        throw new DataNotExistException();
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
    public OrderItem Get(int m_orderID, int m_productID)
    {
        for(int i=0;i<DataSource.orderItemList.Count;i++)
        {
            if(DataSource.orderItemList[i]?.OrderID == m_orderID && DataSource.orderItemList[i]?.ProductID == m_productID)
                return DataSource.orderItemList[i];
        }
        throw new DataNotExistException();
    }

    //public static OrderItem[] getList(int orderId)
    //{
    //    OrderItem[] arr = new OrderItem[];
    //    Array.Copy(DataSource.orderItemArray, arr, DataSource.Confing.I_OrderItem);
    //    return arr;
    //}
}
