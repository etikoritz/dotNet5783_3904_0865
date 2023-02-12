using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

internal class OrderItemXml : IOrderItem
{
    const string s_orderItem = @"OrderItem";  //XML Serializer

    /// <summary>
    /// add New item to order
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Add(OrderItem item)
    {
        //Deserialize
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        //if (s_orderItem.FirstOrDefault(orderItm => orderItm == item.ID) != null)
        //    throw new Exception("ID elready exist!");
        orderItemsList.Add(item);
        //Serialize
        XMLTools.SaveListToXMLSerializer(orderItemsList, s_orderItem);
        return item.ID;
    }

    /// <summary>
    /// Delete item from order
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        //Deserialize
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        if (orderItemsList.RemoveAll(orderItm => orderItm?.ID == id )==0)
            throw new Exception("Missing ID");
        //Serialize
        XMLTools.SaveListToXMLSerializer(orderItemsList, s_orderItem);
    }
    public void Delete(int id, int productID)
    {
        //Deserialize
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        if (orderItemsList.RemoveAll(orderItm => ((orderItm?.ID == id) && (orderItm?.ProductID == productID )))==0)
            throw new Exception("Missing ID");
        //Serialize
        XMLTools.SaveListToXMLSerializer(orderItemsList, s_orderItem);
    }

    public OrderItem? Get(Func<OrderItem?, bool>? condition)
    {
        List<DO.OrderItem?> orderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        return orderItems.FirstOrDefault(condition) ??
            throw new Exception("Missing ID");
    }

    /// <summary>
    /// Deserialize - get orderItem by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem? GetById(int id)
    {
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        return orderItemsList.FirstOrDefault(orderItm => orderItm?.ID == id) ??
            throw new Exception("Missing ID");
    }

    /// <summary>
    /// Deserialize - get orderItem list
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? filter = null)
    {
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        if (filter == null)
            return orderItemsList.Select(orderItm => orderItm).OrderBy(orderItm => orderItm?.ID);
        return orderItemsList.Where(filter).OrderBy(orderItm => orderItm?.ID);
    }

    /// <summary>
    /// Update order
    /// </summary>
    /// <param name="item"></param>
    public void Update(OrderItem item)
    {
        XElement? orderItemRootElem = XMLTools.LoadListFromXMLElement(s_orderItem);
        XElement? itm = (from pr in orderItemRootElem?.Elements()
                          where pr.ToIntNullable("ProductID").Value == item.ProductID
                          where pr.ToIntNullable("OrderID").Value == item.OrderID
                          select pr).FirstOrDefault();
        itm.Element("ID").Value = (item.ID).ToString();
        itm.Element("OrderID").Value = item.OrderID.ToString();
        itm.Element("Amount").Value = (item.Amount).ToString();
        itm.Element("Price").Value = (item.Price).ToString();
        itm.Element("ProductID").Value = (item.ProductID).ToString();
        XElement? lastItem = (from pr in orderItemRootElem?.Elements()
                               where pr.ToIntNullable("ProductID").Value == item.ProductID
                               where pr.ToIntNullable("OrderID").Value==item.OrderID
                               select pr).FirstOrDefault();
        lastItem = itm;
        //productRootElem.Document.Nodes
        XMLTools.SaveListToXMLElement(orderItemRootElem, s_orderItem);
        //return item.ID;
    }
}