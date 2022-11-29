using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BlImplementation;

internal class BoCart : ICart
{
    public BO.Cart AddToCart(BO.Cart cart, int productId)
    {
        throw new NotImplementedException();
    }

    public void ConfirmOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    {
        throw new NotImplementedException();
    }

    public BO.Cart UpdateAmount(BO.Cart cart, int productId, int newAmount)
    {
        throw new NotImplementedException();
    }
}
