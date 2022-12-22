using DO;
using DalApi;

namespace Dal;

internal class DalOrder : IOrder
{
    /// <summary>
    /// Add new order and returns its ID
    /// </summary>
    /// <param name="ord"></param>
    /// <returns>order ID</returns>
    /// <exception cref="DataAlreadyExistException"></exception>
    public int Add(Order ord)
    {
        int temp = DataSource.Confing.get_ID_Order;
        ord.ID = temp;
        ////DataSource.orderList[DataSource.Confing.indexOrder] = ord;
        ////DataSource.Confing.indexOrder++;
        //DataSource.orderList.Add(ord);
        //return ord.ID;
        if (DataSource.orderList.Exists(o=>o?.ID ==ord.ID))
        {
            throw new DataAlreadyExistException();
        }
        DataSource.orderList.Add(ord);
        return ord.ID;
    }

    /// <summary>
    /// Get order by its ID and return it
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the order</returns>
    /// <exception cref="DataNotExistException"></exception>
    public Order? GetById(int id)
    {
        for (int i = 0; i < DataSource.orderList.Count; i++)
        {
            if (DataSource.orderList[i]?.ID ==id)
                return DataSource.orderList[i];
        }
        throw new DataNotExistException();
    }

    /// <summary>
    /// Get the list of all the orders and return it
    /// </summary>
    /// <returns>orders list</returns>
    public List<Order?> GetList()
    {
        List<Order?> arr = new List<Order?>(DataSource.orderList);
        //(DataSource.orderList); 
        //arr= DataSource.orderList, arr, DataSource.Confing.indexOrder);
        return arr;
    }

    /// <summary>
    /// Delete order by order ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DataNotExistException"></exception>
    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.orderList.Count; i++)
        {
            if (DataSource.orderList[i]?.ID == id)
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

    /// <summary>
    /// Update existing order
    /// </summary>
    /// <param name="order"></param>
    /// <exception cref="DataNotExistException"></exception>
    public void Update(Order order)
    {
        for (int i=0;i< DataSource.orderList.Count; i++)
        {
            if(order.ID==DataSource.orderList[i]?.ID)
            {
                DataSource.orderList[i] = order;
                return;
            }
        }
        throw new DataNotExistException();
    }
}
