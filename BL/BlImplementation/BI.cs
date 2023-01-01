using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class BI : IBl
{
    public IProduct Product => new BoProduct();

    public IOrder Order => new BoOrder();

    public ICart Cart => new BoCart();

    /*----NOT IN USE FOR NOW----

    public IOrderItem OrderItem => new BoOrderItem();

    public IProductForList ProductForList => new BoProductForList();

    public IProductItem ProductItem => new BoProductItem();

    public IOrderForList OrderForList => new BoOrderForList();

    public IOrderTracking OrderTracking => new BoOrderTracking();
    */
 
    //אחרי שנממש את מה ששמתי בהערה למעלה יהיה אפשר למחוק את השורות מפה עד הסוף

    public IOrderItem OrderItem => throw new NotImplementedException();

    public IProductForList ProductForList => throw new NotImplementedException();

    public IProductItem ProductItem => throw new NotImplementedException();

    public IOrderForList OrderForList => throw new NotImplementedException();

    public IOrderTracking OrderTracking => throw new NotImplementedException();

}
