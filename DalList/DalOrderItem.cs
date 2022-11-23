using DO;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Dal;

public class DalOrderItem
{
    public int Add(OrderItem ori)
    {;
        ori.ID = DataSource.Confing.get_ID_OrderItem;
        DataSource.orderItemArray[DataSource.Confing.indexOrderItem] = ori;
        DataSource.Confing.indexOrderItem++;
        return ori.ID;
    }
    public OrderItem GetOrderItem(int id)
    {
        for (int i = 0; i < DataSource.Confing.indexOrderItem; i++)
        {
            if (DataSource.orderItemArray[i].ID == id)
                return DataSource.orderItemArray[i];
        }
        throw new Exception("the orderItem doesn't exist i the array");
    }
    public OrderItem[] getList()
    {
        OrderItem[] arr = new OrderItem[DataSource.Confing.indexOrderItem];
        Array.Copy(DataSource.orderItemArray, arr, DataSource.Confing.indexOrderItem);
        return arr;
    }
    public void deleteOrderItem(int id)
    {
        for (int i = 0; i < DataSource.Confing.indexOrderItem; i++)
        {
            if (DataSource.orderItemArray[i].ID == id)
            {
                for (int j = i+1; j < DataSource.Confing.indexOrderItem; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                {
                    DataSource.orderItemArray[j] = DataSource.orderItemArray[j + 1];
                }
                DataSource.Confing.indexOrderItem--;
                return;
            }
        }
        throw new Exception("the orderItem doesn't exist in the array");
    }
    public void updateOrderItem(OrderItem orderItem)
    {
        for (int i = 0; i < DataSource.Confing.indexOrderItem; i++)
        {
            if (orderItem.ID == DataSource.orderItemArray[i].ID)
            {
                DataSource.orderItemArray[i] = orderItem;
                return;
            }
        }
        throw new Exception("the orderItem doesn't exist in the array");
    }



    public OrderItem get(int m_orderID, int m_productID)
    {
        for(int i=0;i<DataSource.orderItemArray.Length;i++)
        {
            if(DataSource.orderItemArray[i].OrderID == m_orderID && DataSource.orderItemArray[i].ProductID==m_productID)
                return DataSource.orderItemArray[i];
        }
        throw new Exception("the orderItem doesn't exist in the array");
    }

    //public static OrderItem[] getList(int orderId)
    //{
    //    OrderItem[] arr = new OrderItem[];
    //    Array.Copy(DataSource.orderItemArray, arr, DataSource.Confing.I_OrderItem);
    //    return arr;
    //}
}
