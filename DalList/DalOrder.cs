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
        DataSource.orderArray[DataSource.Confing.indexOrder] = ord;
        DataSource.Confing.indexOrder++;
        return ord.ID;
    }
    public  Order GetOrder(int id)
    {
        for(int i=0;i<DataSource.Confing.indexOrder;i++)
        {
            if (DataSource.orderArray[i].ID==id)
                return DataSource.orderArray[i];
        }
        throw new Exception("the order doesn't exist in the array");
    }
    public  Order[] getList()
    {
        Order[] arr=new Order[DataSource.Confing.indexOrder]; 
        Array.Copy(DataSource.orderArray, arr, DataSource.Confing.indexOrder);
        return arr;
    }
    public  void deleteOrder(int id)
    {
        for (int i = 0; i < DataSource.Confing.indexOrder; i++)
        {
            if (DataSource.orderArray[i].ID == id)
            {
                for (int j = i; j < DataSource.Confing.indexOrder; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                {
                    DataSource.orderArray[j] = DataSource.orderArray[j + 1];
                }
                DataSource.Confing.indexOrder--;
                return;
            }
        }
        throw new Exception("the order doesn't exist in the array");
    }
    public  void updateOrder(Order order)
    {
        for (int i=0;i<DataSource.orderArray.Length;i++)
        {
            if(order.ID==DataSource.orderArray[i].ID)
            {
                DataSource.orderArray[i] = order;
                return;
            }
        }
        throw new Exception("the order doesn't exist in the array");
    }
}
