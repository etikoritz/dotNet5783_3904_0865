using DO;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Dal;

public class DalOrderItem
{
    public int Add(OrderItem ori)
    {;
        ori.ID = DataSource.Confing.get_ID_OrderItem;
        DataSource.orderItemList.Add(ori);
        //DataSource.orderItemList[DataSource.Confing.indexOrderItem] = ori;
        //DataSource.Confing.indexOrderItem++;
        return ori.ID;
    }
    public OrderItem GetOrderItem(int id)
    {
        for (int i = 0; i < DataSource.orderItemList.Count; i++)
        {
            if (DataSource.orderItemList[i].ID == id)
                return DataSource.orderItemList[i];
        }
        throw new Exception("the orderItem doesn't exist i the array");
    }
    public List<OrderItem> getList()
    {
        List<OrderItem> arr = new List<OrderItem>();
        arr = new List<OrderItem>(DataSource.orderItemList);
        return arr;
    }
    public void deleteOrderItem(int id)
    {
        for (int i = 0; i < DataSource.orderItemList.Count; i++)
        {
            if (DataSource.orderItemList[i].ID == id)
            {
                DataSource.orderItemList.RemoveAt(i);
                //for (int j = i+1; j < DataSource.orderList.Count; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                //{
                //    DataSource.orderItemList[j] = DataSource.orderItemList[j + 1];
                //}
                //DataSource.Confing.indexOrderItem--;
                return;
            }
        }
        throw new Exception("the orderItem doesn't exist in the array");
    }
    public void updateOrderItem(OrderItem orderItem)
    {
        for (int i = 0; i < DataSource.orderItemList.Count; i++)
        {
            if (orderItem.ID == DataSource.orderItemList[i].ID)
            {
                DataSource.orderItemList[i] = orderItem;
                return;
            }
        }
        throw new Exception("the orderItem doesn't exist in the array");
    }



    public OrderItem get(int m_orderID, int m_productID)
    {
        for(int i=0;i<DataSource.orderItemList.Count;i++)
        {
            if(DataSource.orderItemList[i].OrderID == m_orderID && DataSource.orderItemList[i].ProductID==m_productID)
                return DataSource.orderItemList[i];
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
