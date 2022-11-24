//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using DO;

namespace Dal;

public class DalOrder
{
    public  int Add(Order ord)
    {
        int temp = DataSource.Confing.get_ID_Order;
        ord.ID = temp;
        //DataSource.orderList[DataSource.Confing.indexOrder] = ord;
        //DataSource.Confing.indexOrder++;
        DataSource.orderList.Add(ord);
        return ord.ID;
    }
    public  Order GetOrder(int id)
    {
        for(int i=0;i<DataSource.orderList.Count;i++)
        {
            if (DataSource.orderList[i].ID==id)
                return DataSource.orderList[i];
        }
        throw new Exception("the order doesn't exist in the array");
    }
    public  List<Order> getList()
    {
        List<Order> arr = new List<Order>(DataSource.orderList); 
        //arr= DataSource.orderList, arr, DataSource.Confing.indexOrder);
        return arr;
    }
    public  void deleteOrder(int id)
    {
        for (int i = 0; i < DataSource.orderList.Count; i++)
        {
            if (DataSource.orderList[i].ID == id)
            {
                //for (int j = i; j < DataSource.Confing.indexOrder; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                //{
                //    DataSource.orderList[j] = DataSource.orderList[j + 1];
                //}
                //DataSource.Confing.indexOrder--;
                DataSource.orderList.RemoveAt(i);   
                return;
            }
        }
        throw new Exception("the order doesn't exist in the array");
    }
    public  void updateOrder(Order order)
    {
        for (int i=0;i< DataSource.orderList.Count; i++)
        {
            if(order.ID==DataSource.orderList[i].ID)
            {
                DataSource.orderList[i] = order;
                return;
            }
        }
        throw new Exception("the order doesn't exist in the array");
    }
}
