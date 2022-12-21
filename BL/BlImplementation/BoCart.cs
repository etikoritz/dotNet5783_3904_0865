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
    private DalApi.IDal Dal = new Dal.DalList();
    private List<DO.Order> orderlist = new List<DO.Order>();
    public BO.Cart AddToCart(BO.Cart cart, int id)
    {
        DO.Product product = Dal.Product.GetById(id);
        foreach (var item in cart.Items)
        {
            if (item.ProductID == id)
            {//item alredy in cart- amount++
                if (product.InStock - item.Amount >= 0)
                {
                    product.InStock -= item.Amount;
                    item.Amount++;///
                    item.TotalPrice += product.Price*item.Amount;
                    Dal.Product.Update(product);
                    return cart;
                }
                else throw new OutOfStockProductException();
            }

        }
        ///
        ///create random id to the order and check if it already exsit
        ///
        //int orderID = random.Next(99999, 100000);
        //for (int i=0;i<cart.Items.Count;i++)
        //{
        //    if (cart.Items[i].ID == orderID)
        //    {
        //        orderID = random.Next(99999, 100000);
        //        i = 0;
        //    }
        //}

        if (product.InStock > 0)// add the item to cart 
        {
            BO.OrderItem newItem = new BO.OrderItem//maybe its DO
            {
                //ID=orderID,
                Name = product.Name,
                ProductID = id,
                Price = product.Price,
                TotalPrice = product.Price,
                Amount = 1
            };
            cart.Items.Add(newItem);
            //product.InStock -= 1;
            //Dal.Product.Update(product);
            cart.TotalPrice += product.Price;
            return cart;
        }
        else throw new OutOfStockProductException();
    }

    /// <summary>
    /// Recives a cart, checks its propriety, 
    /// make a new Order and OrderItem (Dal) 
    /// and removing products from stock
    /// --Cart screen or Completing order screen--
    /// </summary>
    public int ConfirmOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    {
        foreach (var item in cart.Items)
        {
            //check if the amount is negative
            if (item.Amount <= 0)
            {
                throw new NegativeIdException();
            }
            //check if all the items exsist in stock and the stock have enohgf  
            try
            {
                DO.Product product = Dal.Product.GetById(item.ProductID);
                if (product.InStock - item.Amount <= 0)
                {
                    throw new OutOfStockProductException();
                }
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
        }
        //    //need to check if product exist
        //}
        //if (customerName == "")
        //{
        //    throw new BO.NoCustomerNameException();
        //}
        ////if (customerEmail == "" || !customerEmail.Contains("@") || customerEmail[0] == '@' || customerEmail[customerEmail.Length + 1] == '@')
        ////{
        ////    throw new BO.NoCustomerEmailException();
        ////}
        //if (customerAddress == "")
        //{
        //    throw new BO.NoCustomerAddressException();
        //}
        
        DO.Order order = new()
        {
            //id=Dal.Order.*/
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            CustomerAddress = customerAddress,
            OrderDate = DateTime.Today,
            ShipDate=DateTime.Today,
            DeliveryDate=DateTime.Today,
        };
        int orderId = Dal.Order.Add(order);
        order.ID=orderId;
        
        foreach (var item in cart.Items)
        { 
            DO.OrderItem orderItem = new()
            {
                ID = order.ID,
                OrderID = orderId,
                ProductID = item.ProductID,
                Price = item.Price,
                
            };
            orderItem.Amount+=item.Amount;////!!!!!!!!!!!1
            //try
            //{
            Dal.OrderItem.Add(orderItem);
            //Dal.Order.Add(order);
            orderlist.Add(order);
            //List<DO.OrderForList> orderlist = new List<BO.OrderForList>();
            //orderlist.Add(order);
            //}
            //catch///////////////////////
            //{
            //    throw;
            //}
            DO.Product product = Dal.Product.GetById(item.ProductID);
            product.InStock -= item.Amount;
            Dal.Product.Update(product);
            //Dal.OrderItem.Add(orderItem);
        }
        return orderId;
    }

    

    public BO.Cart UpdateAmount(BO.Cart cart, int productId, int newAmount)
    {
        int count = 0;
        //List<BO.OrderForList> orderForListsData = new List<BO.OrderForList>();
        DO.Product product = Dal.Product.GetById(productId);
        foreach (var item in cart.Items)
        {
            if(item.ProductID== productId)
            {
                if(product.InStock-newAmount<=0)
                {
                    throw new OutOfStockProductException();
                }
                cart.TotalPrice -= item.Amount * item.Price;
                item.Amount = newAmount;
                cart.TotalPrice += item.Price * newAmount;
                cart.Items[count]=item;
                return cart ;
            }
            count++;
        }
        //foreach(var item in orderForListsData)
        //{

        //}
        //orderForLists.
        throw new NotImplementedException();

    }

    //public void GetItemInCartList(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    //{
    //    foreach (var item in cart.Items)
    //    {
    //        Console.WriteLine(item);
    //    }
    //}
}
