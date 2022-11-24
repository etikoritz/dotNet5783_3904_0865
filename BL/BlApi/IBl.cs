using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi;

public interface IBl
{
    public IProduct Product { get; }
    public IOrder Order { get; }
    public IOrderItem orderItem { get; }

    //----NOT IN USE FOR NOW--
    public IProductForList ProductForList { get; }
    public IProductItem ProductItem { get; }
    public IOrderForList OrderForList { get; }
    public IOrderItem OrderItem { get; }
    public IOrderTracking OrderTracking { get; }
}
