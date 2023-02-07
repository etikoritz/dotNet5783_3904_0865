using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal;

internal class OrderXml : IOrder
{
    const string s_order = @"Order";  //XML Serializer

    /// <summary>
    /// Add new order
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Add(Order item)
    {
        //Deserialize
        List<DO.Order?> ordersList = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        if (s_order.FirstOrDefault(ordr => ordr == item.ID) != null)
            throw new Exception("ID elready exist!");
        ordersList.Add(item);
        //Serialize
        XMLTools.SaveListToXMLSerializer(ordersList, s_order);
        return item.ID;
    }

    /// <summary>
    /// Delete order by order ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        //Deserialize
        List<DO.Order?> ordersList = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        if (ordersList.RemoveAll(ordr => ordr?.ID == id) == 0)
            throw new Exception("Missing ID");
        //Serialize
        XMLTools.SaveListToXMLSerializer(ordersList, s_order);
    }

    public Order? Get(Func<Order?, bool>? condition)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get order by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Order? GetById(int id)
    {
        List<DO.Order?> ordersList = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        return ordersList.FirstOrDefault(ordr => ordr?.ID == id) ??
            throw new Exception("Missing ID");
    }

    /// <summary>
    /// Get list of all orders
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? filter = null)
    {
        List<DO.Order?> ordersList = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        if (filter == null)
            return ordersList.Select(ordr => ordr).OrderBy(ordr => ordr?.ID);
        return ordersList.Where(filter).OrderBy(ordr => ordr?.ID);
    }

    /// <summary>
    /// Update order
    /// </summary>
    /// <param name="item"></param>
    public void Update(Order item)
    {
        Delete(item.ID);
        Add(item);
    }
}