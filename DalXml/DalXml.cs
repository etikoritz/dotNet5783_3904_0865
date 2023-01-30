using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

sealed internal class DalXml : IDal
{
    #region Singleton
    static readonly DalXml instance = new DalXml();
    public static DalXml Instance { get { return instance; } }
    static DalXml() { }
    //public static IDal instance { get; } = new DalXml();
    /// <summary>
    /// private ctor for the singleton
    /// </summary>
    private DalXml() { }
    #endregion

    public IOrder Order { get; } = new Dal.OrderXml();
    public IProduct Product { get; } = new Dal.ProductXml();
    public IOrderItem OrderItem { get; } = new Dal.OrderItemXml();

}
