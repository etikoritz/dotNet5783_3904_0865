using Dal;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using BlImplementation;
using System.Collections.Generic;
using BO;
using BlApi;
using System.Linq.Expressions;
using DO;

namespace BL;

public class Program
{

    /// <summary>
    /// in order to allow acsses ta all bl methods
    /// </summary>
    private static BlApi.IBl Bl = BlApi.Factory.Get();
    static void Main()
    {
        try
        {
            bool flag = true;
            while (flag)
            {

                Console.WriteLine(@"enter: 1 if you are manager 
       2 if you are customer
       0 to Exit");
                if (!int.TryParse(Console.ReadLine(), out int ch)) // converts the input to integer
                    throw new BO.InvalidChoisException();
                switch (ch)
                {
                    case 1:
                        Manager();
                        break;
                    case 2:
                        Customer();
                        break;
                    case 0:
                        flag = false;
                        break;
                    default:
                        break;
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }
    private static void Manager()
    {
        bool flag = true;
        while (flag)
        {
            try
            {
                Console.WriteLine(@"enter: 1 for product
       2 for Order
       0 to Exit");
                if (!int.TryParse(Console.ReadLine(), out int ch)) // converts the input to integer
                    throw new BO.InvalidChoisException();
                switch (ch)
                {
                    case 0:
                        flag = false;
                        break;
                    case 1:
                        manageProduct();
                        break;
                    case 2:
                        manageOrderManager();
                        break;
                    default: // back to main menu
                        break;
                }


            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
    private static void Customer()
    {
        bool flag = true;
        Console.WriteLine("please enter your name, email and adress");
        string? name = Console.ReadLine();
        if (name == "")
        {
            throw new BO.IlegalDataException("Ilegal name");
        }
        string? Email = Console.ReadLine();
        if(!Email.Contains("@gmail.com")|| Email[0]=='@')
        {
            throw new BO.IlegalDataException("Ilegal email");
        }
        string? adress = Console.ReadLine();
        if (adress == "")
        {
            throw new BO.IlegalDataException("Ilegal address");
        }
        BO.Cart cart = new()
        {
            CustomerName = name,
            CustomerAddress = adress,
            CustomerEmail = Email,
            TotalPrice = 0,
            Items = new List<BO.OrderItem?>()
        };
        while (flag)
        {
            try
            {
                Console.WriteLine(@"enter: 1 for products catalog 
       2 managing cart
       3 for getting Order Description
       0 to Exit");
                if (!int.TryParse(Console.ReadLine(), out int ch)) // converts the input to integer
                    throw new BO.InvalidChoisException();
                switch (ch)
                {
                    case 0:
                        flag = false;
                        break;
                    case 1:
                        GetAllProducts();
                        break;
                    case 2:
                        manageCart(cart, name, Email, adress);
                        break;
                    case 3:
                        GetOrderDesc();
                        break;
                    default: // back to main menu
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }

        }
    }

    private static void manageProduct()
    {
        Console.WriteLine(@"enter: 1 for adding a new product
       2 for getting a product description according to ID
       3 for getting catalog of all products
       4 for updating an existing product 
       5 for deleting a product
       0 for returning back to the main menu ");
        if (!int.TryParse(Console.ReadLine(), out int ch1)) // converts the input to integer
            throw new BO.InvalidChoisException();
        switch (ch1)
        {
            case 1: AddNewProduct(); break;
            case 2: PrintDescriptionMANAGER(); break;
            case 3: GetAllProducts(); break;
            case 4: UpdateProduct(); break;
            case 5: DeleteProduct(); break;
            default:
                break;
        }
    }
    /// <summary>
    /// adding a new product with user's input details
    /// </summary>
    private static void AddNewProduct()
    {
        Console.WriteLine("enter the product's uniqe ID number");
        if (!int.TryParse(Console.ReadLine(), out int id)) // convert id from string to int
            throw new BO.IlegalDataException("Ilegal ID");
        Console.WriteLine("enter the product's category");
        string? cat = Console.ReadLine();
        //if (!int.TryParse(Console.ReadLine(), out int cat));
        //    throw new BO.IlegalDataException("Ilegal category");
        Console.WriteLine("enter the product's name");
        string? name = Console.ReadLine();
        Console.WriteLine("enter the product's price");
        if (!double.TryParse(Console.ReadLine(), out double price)) // convert string to double
            throw new BO.IlegalDataException("Ilegal price");
        Console.WriteLine("enter amount of products in stock");
        if (!int.TryParse(Console.ReadLine(), out int amount)) // convert string to int
            throw new BO.IlegalDataException("Ilegal amount");
        BO.Product p = new()
        {
            ID = id,
            Category =((DO.Enums.Category)(BO.Enum.Category)System.Enum.Parse(typeof(BO.Enum.Category), cat!)),
            //Category = (BO.Enums.Category)Enum.Parse(typeof(BO.Enums.Category), cat!),
            //((BO.Enum.Category)typeof(BO.Enum.Category), cat!),
            //Category = (BO.Enum.Category)BO.Enum.Category,cat,
            Name = name,
            Price = price,
            InStock = amount,
        };
        Bl.Product.Add(p); // add p to data list
    }

    /// <summary>
    /// prints a product description
    /// </summary>
    private static void PrintDescriptionMANAGER()
    {
        Console.WriteLine("enter the product ID number");
        if (!int.TryParse(Console.ReadLine(), out int id)) // convert input to int
            throw new BO.IlegalDataException("Ilegal ID");
        Console.WriteLine(Bl.Product.GetProductDetails(id)); // finds the right product, and print the description
    }

    /// <summary>
    /// print description for every product in data list
    /// </summary>
    private static void GetAllProducts()
    {

        IEnumerable<BO.ProductForList?> ie = Bl.Product.GetProductList();
        foreach (BO.ProductForList? item in ie) // print every product in list
        {
            Console.WriteLine($"{item}\n");
        }
    }

    /// <summary>
    /// update a product according to the user's input
    /// </summary>
    private static void UpdateProduct()
    {
        Console.WriteLine("enter Id number of product you want to update");
        if (!int.TryParse(Console.ReadLine(), out int id))  // convert input to int
            throw new BO.IlegalDataException("Ilegal ID");
        Console.WriteLine("enter the product's category");
        string? cat = Console.ReadLine();
        Console.WriteLine("enter the product's name");
        string? name = Console.ReadLine();
        Console.WriteLine("enter the product's price");
        if (!double.TryParse(Console.ReadLine(), out double price)) // convert string to double
            throw new BO.IlegalDataException("Ilegal price");
        Console.WriteLine("enter amount of product in stock");
        if (!int.TryParse(Console.ReadLine(), out int amount)) // convert string to int
            throw new BO.IlegalDataException("Ilegal amount");
        BO.Product p = new()
        {
            ID = id,
            Name = name,
            Price = price,
            Category = ((DO.Enums.Category)(BO.Enum.Category)System.Enum.Parse(typeof(BO.Enum.Category), cat!)),
            //Category = (BO.Enums.Category)Enum.Parse(typeof(BO.Enums.Category), cat!), // convert string to enum
            InStock = amount
        };
        Bl.Product.Update(p); // updates p in data list;
    }

    /// <summary>
    /// recieves product's id and delete the product from list
    /// </summary>
    private static void DeleteProduct()
    {
        Console.WriteLine("enter Id number of product you want to delete");
        if (!int.TryParse(Console.ReadLine(), out int ID))  // convert input to int
            throw new BO.IlegalDataException("Ilegal ID");
        Bl.Product.Delete(ID); // delete product from list
    }
    private static void manageOrderManager()
    {
        bool flag = true;
        while (flag)
        {
            Console.WriteLine(@"enter: 1 for getting all orders description
       2 for getting an order description according to ID
       3 for updating shipping
       4 for updating Delivery 
       5 for Order tracking
       6 for updating Order
       0 for returning back to the main menu ");
            if (!int.TryParse(Console.ReadLine(), out int ch1)) // converts the input to integer
                throw new BO.IlegalDataException("Ilegal choice");
            switch (ch1)
            {
                case 0: flag = false; break;
                case 1: GetAllOrderDesc(); break;
                case 2: GetOrderDesc(); break;
                case 3: UpdateDelivery(); break;
                case 4: UpdateShipping(); break;
                case 5: TrackOrder(); break;
                case 6: UpdateOrder(); break;
                default: // back to sub menu
                    break;
            }
        }

    }
    private static void GetAllOrderDesc()
    {
        IEnumerable<BO.OrderForList?> ie = Bl.Order.GetOrderList();
        foreach (BO.OrderForList? item in ie)
        {
            Console.WriteLine(item);
        }
    }
    private static void GetOrderDesc()
    {
        Console.WriteLine("enter order ID");
        if (!int.TryParse(Console.ReadLine(), out int ID)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        Console.WriteLine(Bl.Order.GetOrderDetails(o=>o.Value.ID == ID)); ;// printing description.
    }
    private static void UpdateDelivery()
    {
        Console.WriteLine("enter order ID");
        if (!int.TryParse(Console.ReadLine(), out int ID)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        Bl.Order.UpdateOrderDelivery(ID);
    }
    private static void UpdateShipping()
    {
        Console.WriteLine("enter order ID");
        if (!int.TryParse(Console.ReadLine(), out int ID)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        Bl.Order.UpdateOrderSupply(ID);
    }
    private static void TrackOrder()
    {
        Console.WriteLine("enter order ID");
        if (!int.TryParse(Console.ReadLine(), out int ID)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        Console.WriteLine(Bl.Order.TrackOrder(ID));
    }
    private static void UpdateOrder()
    {
        Console.WriteLine("enter order ID");
        if (!int.TryParse(Console.ReadLine(), out int ID)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        string action = "";
        bool flag = true;
        int amount = 0;
        Console.WriteLine("enter product id");
        int.TryParse(Console.ReadLine(), out int productID);
        while (flag)
        {
            Console.WriteLine($@"enter 1 to remove item
2 to add item 
3 to update item amount
0 to exit");
            int.TryParse(Console.ReadLine(), out int choise);
            switch (choise)
            {
                case 0: flag = false; break;
                case 1:
                    action = "remove";
                    Bl.Order.UpdateOrderByManager(ID, productID, action, amount);
                    break;
                case 2:
                    action = "add";
                    Bl.Order.UpdateOrderByManager(ID, productID, action, amount);
                    break;
                case 3:
                    Console.WriteLine("enter amount to update:");
                    int.TryParse(Console.ReadLine(), out amount);
                    action = "addAmount";
                    Bl.Order.UpdateOrderByManager(ID, productID, action, amount);
                    break;
                default: break;
            }
            
        }
    }

    /// <summary>
    /// sub cart menu
    /// user
    /// </summary>
    /// <param name="cart"></param>
    private static void manageCart(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    {
        bool flag = true;
        while (flag)
        {
            Console.WriteLine(@"enter: 1 for adding a product to cart
       2 for updating a product's amount in cart
       3 for orderring all product in shopping cart
       4 for list of items in cart
       0 for returning back to the main menu ");
            if (!int.TryParse(Console.ReadLine(), out int ch1)) // converts the input to integer
                throw new BO.IlegalDataException("Ilegal choice");
            switch (ch1)
            {
                case 0: flag = false; break;
                case 1: AddToCart(cart); break;
                case 2: UpdateAmountInCart(cart); break;
                case 3: OrderCart(cart); break;
                case 4: GetListItemInCart(cart, customerName, customerEmail, customerAddress); break;
                default: // back to sub menu
                    break;
            }
        }
    }

    /// <summary>
    /// adding a product to cart
    /// </summary>
    /// <param name="cart"></param>
    private static void AddToCart(BO.Cart cart)
    {
        Console.WriteLine("Enter ID of a product that you want to add to shopping cart");
        if (!int.TryParse(Console.ReadLine(), out int pID)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        Bl.Cart.AddToCart(cart, pID);
    }

    /// <summary>
    /// updating a product's amount in cart
    /// </summary>
    /// <param name="cart"></param>
    private static void UpdateAmountInCart(BO.Cart cart)
    {
        Console.WriteLine("Enter ID of a product that you want to update");
        if (!int.TryParse(Console.ReadLine(), out int pID))// converts the input to integer
            throw new BO.IlegalDataException("Ilegal ID");
        Console.WriteLine("Enter the amount to update");
        if (!int.TryParse(Console.ReadLine(), out int pAmount)) // converts the input to integer
            throw new BO.IlegalDataException("Ilegal amount");
        Bl.Cart.UpdateAmount(cart, pID, pAmount);
    }

    /// <summary>
    /// orderring all product in shopping cart
    /// </summary>
    /// <param name="cart"></param>
    private static void OrderCart(BO.Cart cart)
    {
        //Console.WriteLine("enter custumer name:");
        //string? name = Console.ReadLine();
        //Console.WriteLine("enter custumer address:");
        //string? address = Console.ReadLine();
        //Console.WriteLine("enter custumer email:");
        //string? email = Console.ReadLine();
        int orderID = Bl.Cart.ConfirmOrder(cart, cart.CustomerName, cart.CustomerEmail, cart.CustomerAddress);
        Console.WriteLine("your order ID is: "+ orderID);
    }

    private static void GetListItemInCart(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    {
        Console.WriteLine(cart);
        //Bl.Cart.GetItemInCartList(cart, customerName, customerEmail, customerAddress);
    }

}