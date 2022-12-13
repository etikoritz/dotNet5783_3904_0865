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

    public BO.Cart AddToCart(BO.Cart cart, int id)
    {
        DO.Product product = Dal.Product.GetById(id);

        foreach (var item in cart.Items)
        {
            if (item.ID == id)
            {//item alredy in cart- amount++
                if (product.InStock - item.Amount >= 0)
                {
                    item.Amount++;
                    item.TotalPrice += product.Price;
                    return cart;
                }
                else throw new OutOfStockProductException();
            }
        }
        if (product.InStock > 0)// add the item to cart 
        {
            BO.OrderItem newItem = new BO.OrderItem//maybe its DO
            {
                Name = product.Name,
                ID = product.ID,
                Price = product.Price,
                TotalPrice = product.Price,
                Amount = 1
            };
            cart.Items.Add(newItem);
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
    /// כל המוצרים קיימים, כמויות חיוביות, יש מספיק במלאי, שם וכתובת קונה לא ריקים, כתובת דוא"ל ריקה או לפי פורמט חוקי)

    public void ConfirmOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    {
        foreach (var item in cart.Items)
        {
            //check if the amount is negative
            if (item.Amount <= 0)
            {
                throw new NegativeProductIdException();
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

            //need to check if product exist
        }
        if (customerName == "")
        {
            throw new BO.NoCustomerNameException();
        }
        if (customerEmail == "" || !customerEmail.Contains("@") || customerEmail[0] == '@' || customerEmail[customerEmail.Length + 1] == '@')
        {
            throw new BO.NoCustomerEmailException();
        }
        if (customerAddress == "")
        {
            throw new BO.NoCustomerAddressException();
        }

        DO.Order order = new()
        {
            //id
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            CustomerAddress = customerAddress,
            OrderDate = DateTime.Now,
            //ShipDate
            //DeliveryDat
        };
        int orderId = Dal.Order.Add(order);
        
        foreach (var item in cart.Items)
        { 
            DO.OrderItem orderItem = new()
            {
                //id
                OrderID = orderId,
                ProductID = item.ProductID,
                Price = item.Price,
                Amount = item.Amount
            };
            //try
            //{
                Dal.OrderItem.Add(orderItem);
            //}
            //catch///////////////////////
            //{
            //    throw;
            //}
            DO.Product product = Dal.Product.GetById(item.ProductID);
            product.InStock -= item.Amount;
            Dal.Product.Update(product);
        }

    }

    

    public BO.Cart UpdateAmount(BO.Cart cart, int productId, int newAmount)
    {
        DO.Product product = Dal.Product.GetById(productId);
        foreach (var item in cart.Items)
        {
            if(item.ProductID== productId)
            {
                if(product.InStock-newAmount<=0)
                {
                    throw new OutOfStockProductException();
                }
                item.Amount = newAmount;
                item.TotalPrice += item.Price * newAmount;
                product.InStock -= newAmount;
                return cart;
            }
        }
        throw new NotImplementedException();

    }
}
