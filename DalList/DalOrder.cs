using DO;
using DalApi;
using System;

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
        if (DataSource.orderList.Exists(o => o?.ID == ord.ID))
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

        DO.Order? order = DataSource.orderList.FirstOrDefault(o => o.Value.ID == id);
        if (order == null)
            throw new DataNotExistException();
        return order ?? new();
    }

    /// <summary>
    /// Get the list of all the orders and return it
    /// </summary>
    /// <returns>orders list</returns>
    //public List<Order?> GetList()
    //{
    public IEnumerable<DO.Order?> GetList(Func<DO.Order?, bool>? filter = null)
    {
        return DataSource.orderList.ConvertAll(order => order);
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
            //if (DataSource.orderList[i]?.ID == id)
            if (DataSource.orderList[i]?.ID == id)
            {
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
        for (int i = 0; i < DataSource.orderList.Count; i++)
        {
            //if(order.ID==DataSource.orderList[i]?.ID)
            if (order.ID == DataSource.orderList[i]?.ID)
            {
                DataSource.orderList[i] = order;
                return;
            }
        }
        throw new DataNotExistException();
    }

    public Order? Get(Func<Order?, bool>? condition)
    {

        DO.Order? order = DataSource.orderList.FirstOrDefault(o => condition(o));
        if (order == null)
            throw new DataNotExistException();
        return order ?? new ();
    }
}
