﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using DO;
using DalApi;

namespace Dal;

internal class DalOrder:IOrder
{
    public  int Add(Order ord)
    {
        int temp = DataSource.Confing.get_ID_Order;
        ord.ID = temp;
        ////DataSource.orderList[DataSource.Confing.indexOrder] = ord;
        ////DataSource.Confing.indexOrder++;
        //DataSource.orderList.Add(ord);
        //return ord.ID;
        if (DataSource.orderList.Exists(o=>o.ID==ord.ID))
        {
            throw new DataAlreadyExistException();
        }
        DataSource.orderList.Add(ord);
        return ord.ID;

    }
    public  Order GetById(int id)
    {
        for(int i=0;i<DataSource.orderList.Count;i++)
        {
            if (DataSource.orderList[i].ID==id)
                return DataSource.orderList[i];
        }
        throw new DataNotExistException();
    }
    public  List<Order> GetList()
    {
        List<Order> arr = new List<Order>();
            //(DataSource.orderList); 
        //arr= DataSource.orderList, arr, DataSource.Confing.indexOrder);
        arr=new List<Order>(DataSource.orderList);
        return arr;
    }
    public  void Delete(int id)
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
        throw new DataNotExistException();
    }
    public  void Update(Order order)
    {
        for (int i=0;i< DataSource.orderList.Count; i++)
        {
            if(order.ID==DataSource.orderList[i].ID)
            {
                DataSource.orderList[i] = order;
                return;
            }
        }
        throw new DataNotExistException();
    }
}
