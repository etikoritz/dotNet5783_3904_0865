using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class BoCart : ICart
{
    //i added idal? the mark in stage 4
    private DalApi.IDal? Dal = DalApi.Factory.Get();
    private List<DO.Order> orderlist = new List<DO.Order>();
    private List<DO.OrderItem?> itemlist = new List<DO.OrderItem?>();
    public BO.Cart AddToCart(BO.Cart cart, int id)
    {
        DO.Product product = (DO.Product)Dal.Product.GetById(id);
        //item alredy in cart- amount++
        //foreach (var item in
        //from item in cart.Items
        //where item.ProductID == id
        //select item)
        List<BO.OrderItem?>? items = cart?.Items;
        //if(cart.Items!=null)
        //{
        //foreach (var item in items)
        //{
        //    if (item?.ProductID == id)
        //    {
        //if (product.InStock - item.Amount >= 0)
        //if(items?.Count!=0)
        //{
        foreach (var item in
    from item in cart?.Items
    where item.ProductID == id
    select item)
        {
            item.Amount++;
            item.TotalPrice += product.Price * item.Amount;
            Dal.Product.Update(product);
            cart.Amount++;
            return cart;
        }
        //}


        //}
        //else throw new OutOfStockProductException();
        // }
        // }
        //}

        //from item
        //{

        //}
        //if (items?.Count == 0)
        //{
        DO.OrderItem newItemDO = new DO.OrderItem//maybe its DO
        {
            ProductID = id,
            Price = product.Price,
            //TotalPrice = product.Price,
            Amount = 1
        };
        int itemID = Dal.OrderItem.Add(newItemDO);
        BO.OrderItem newItemBO = new BO.OrderItem//maybe its DO
        {
            ID = itemID,
            Name = product.Name,
            ProductID = id,
            Price = product.Price,
            //TotalPrice = product.Price,
            Amount = 1
        };
        cart.Amount++;
        cart.Items?.Add(newItemBO);
        //product.InStock -= 1;
        //Dal.Product.Update(product);
        cart.TotalPrice += product.Price;
        return cart;
        //}

        //if (product.InStock > 0)// add the item to cart 
        //{
        //    BO.OrderItem newItem = new BO.OrderItem//maybe its DO
        //    {
        //        //ID=orderID,
        //        Name = product.Name,
        //        ProductID = id,
        //        Price = product.Price,
        //        TotalPrice = product.Price,
        //        Amount = 1
        //    };
        //    cart?.Items?.Add(newItem);
        //    //product.InStock -= 1;
        //    //Dal.Product.Update(product);
        //    cart.TotalPrice += product.Price;
        //    return cart;
        //}
        //else throw new OutOfStockProductException();
    }

    /// <summary>
    /// Recives a cart, checks its propriety, 
    /// make a new Order and OrderItem (Dal) 
    /// and removing products from stock
    /// --Cart screen or Completing order screen--
    /// </summary>
    public int ConfirmOrder(BO.Cart cart)
    {
        foreach (var item in cart.Items)
        {
            //check if the amount is negative
            if (item.Amount < 0)
            {
                throw new NegativeIdException();
            }
            //check if all the items exsist in stock and the stock have enohgf  
            try
            {
                DO.Product product = (DO.Product)Dal.Product.GetById(item.ProductID);
                if (product.InStock - item.Amount <= 0)
                {
                    throw new OutOfStockProductSException($"the product {product.Name} is out of stock!");

                }
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
        }
        TimeSpan spaceTimeShipping = TimeSpan.FromDays(3);
        TimeSpan spaceTimeDelivery = TimeSpan.FromDays(4);
        DO.Order order = new()
        {
            //id=Dal.Order.*/
            CustomerName = cart.CustomerName,
            CustomerEmail = cart.CustomerEmail,
            CustomerAddress = cart.CustomerAddress,
            OrderDate = DateTime.Today,
            ShipDate = (DateTime.Today +spaceTimeShipping).Date,
            DeliveryDate = (DateTime.Today+ spaceTimeDelivery).Date,
        };
        int orderId = Dal.Order.Add(order);
        order.ID = orderId;
        foreach (var item in cart.Items)
        {
            DO.OrderItem orderItem = new DO.OrderItem()
            {
                ID = order.ID,
                OrderID = orderId,
                ProductID = item.ProductID,
                Price = item.Price,

            };
            orderItem.Amount += item.Amount;
            Dal.OrderItem.Add(orderItem);
            orderlist.Add(order);
            DO.Product product = (DO.Product)Dal.Product.GetById(item.ProductID);
            product.InStock -= item.Amount;
            Dal.Product.Update(product);
        }
        return orderId;
    }



    public BO.Cart UpdateAmount(BO.Cart cart, int productId, int newAmount)
    {
        DO.Product product = (DO.Product)Dal.Product.GetById(productId);
        if (cart.Items.Exists(o => o?.ProductID == productId))
        {
            var item = cart.Items.FirstOrDefault(o => o?.ProductID == productId);
            if (!(item.Amount - newAmount < 0))
            {
                item.Amount -= newAmount;
                cart.TotalPrice -= item.Price * newAmount;
                cart.Amount -= newAmount;
                return cart;
            }
            if (item.Amount - newAmount == 0)
            {
                DeleteFromeCart(cart, product.ID);
            }
        }
        else
        {
            throw new NotImplementedException();
        }
        return cart;
    }

    /// <summary>
    /// delete product from cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="ProductId"></param>
    /// <returns></returns>
    public BO.Cart? DeleteFromeCart(BO.Cart cart, int ProductId)
    {
        DO.Product product = (DO.Product)Dal.Product.GetById(ProductId);
        int count = -1;

        foreach (var item in
        from item in cart.Items
        where item.ProductID == ProductId
        select item)
        {
            BO.OrderItem deleteItem = item;
            product.InStock += item.Amount;
            //items?.Remove(item);
            Dal.OrderItem.Delete(item.ID);
            Dal.Product.Update(product);
            cart.Amount -= item.Amount;
            cart.TotalPrice -= product.Price * item.Amount;
            count++;
        }
        cart.Items?.RemoveAt(count);

        return cart;
    }

    public IEnumerable<BO.OrderItem> GetItemInCartList(BO.Cart cart)
    {
        return cart.Items;
    }
    
}
