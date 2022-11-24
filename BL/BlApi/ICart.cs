using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ICart
{
    public Cart AddToCart(Cart cart, int productId);
    public Cart UpdateAmount(Cart cart, int productId, int newAmount);
    public void ConfirmOrder(Cart cart, string customerName, string customerEmail, string customerAddress);
}
