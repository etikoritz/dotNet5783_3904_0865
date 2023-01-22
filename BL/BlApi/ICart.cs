using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ICart
{
    /// <summary>
    /// Add product to the cart
    /// --Catalog screen,Product details screen--
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <returns>updated cart (after the adding of the product)</returns>
    public Cart AddToCart(Cart cart, int productId);

    /// <summary>
    /// Update the amount of product in the cart by its ID
    /// --Cart screen--
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <param name="newAmount"></param>
    /// <returns>Updated cart</returns>
    public Cart UpdateAmount(Cart cart, int productId, int newAmount);

    /// <summary>
    /// Recives a cart, checks its propriety, 
    /// make a new Order and OrderItem (Dal) 
    /// and removing products from stock
    /// --Cart screen or Completing order screen--
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="customerName"></param>
    /// <param name="customerEmail"></param>
    /// <param name="customerAddress"></param>
    public int ConfirmOrder(Cart cart);
    public BO.Cart? DeleteFromeCart(BO.Cart cart, int ProductId);
    public IEnumerable<BO.OrderItem> GetItemInCartList(BO.Cart cart);
}
