using Dal;
using DO;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;

namespace Dal;

public class Program
{
    private static DalApi.IDal dalList = DalApi.Factory.Get();

    static void OptionProduct(DalApi.IDal dalList)
    {
        int newProductInStock;
        double newProductPrice;
        int newProducCategory;
        string newProductName;
        int newProductID;
        char action;

        do
        {
            Console.WriteLine("Select the desired option:\r\n" +
            "a: Add product\r\n" +
            "b: Single product display\r\n" +
            "c: Display of all products\r\n" +
            "d: Update product\r\n" +
            "e: Delete product\r\n" +
            "f: exit\n");
            char.TryParse(Console.ReadLine(), out action);

            switch (action)
            {
                case 'a':
                    Console.WriteLine(@"
You have selected to add a new Product.
Please enter the name of the Product:");
                    newProductName = Console.ReadLine();
                    Console.WriteLine("Enter the category of the new Product: 0 for Laptops, 1 for DesktopComputer, 2 for Tablet, 3 for Cellphone, and 4 for Headphones  ");
                    int.TryParse(Console.ReadLine(), out newProducCategory);

                    Console.WriteLine("Enter the Price of the Product: ");
                    double.TryParse(Console.ReadLine(), out newProductPrice);

                    Console.WriteLine("Enter the number of products left in stock: ");
                    int.TryParse(Console.ReadLine(), out newProductInStock);

                    Product newProduct = new Product
                    {
                        Name = newProductName,
                        Category = (Enums.Category)newProducCategory,
                        Price = newProductPrice,
                        InStock = newProductInStock,
                    };

                    try
                    {
                        dalList.Product.Add(newProduct);
                        Console.WriteLine("secsess");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case 'b':
                    Console.WriteLine(@"Enter product ID:");
                    int.TryParse(Console.ReadLine(), out int id);
                    try
                    {
                        Console.WriteLine(dalList.Product.GetById(id));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case 'c':
                    foreach (var item in dalList.Product.GetList())
                    {
                        Console.WriteLine(item);
                    }
                    break;


                case 'd':
                    Console.WriteLine(@"Enter ID of the product tou want to update:");
                    int.TryParse(Console.ReadLine(), out newProductID);

                    Console.WriteLine("Please enter the name of the Product:");
                    newProductName = Console.ReadLine();

                    Console.WriteLine("Enter the category of the new Product: 0 for Laptops, 1 for DesktopComputer, 2 for Tablet, 3 for Cellphone, and 4 for Headphones  ");
                    int.TryParse(Console.ReadLine(), out newProducCategory);

                    Console.WriteLine("Enter the Price of the Product: ");
                    double.TryParse(Console.ReadLine(), out newProductPrice);

                    Console.WriteLine("Enter the number of products left in stock: ");
                    int.TryParse(Console.ReadLine(), out newProductInStock);

                    Product updateProduct = new Product
                    {
                        ID = newProductID,
                        Name = newProductName,
                        Category = (Enums.Category)newProducCategory,
                        Price = newProductPrice,
                        InStock = newProductInStock,
                    };
                    try
                    {
                        dalList.Product.Update(updateProduct);
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;
                case 'e':
                    Console.WriteLine(@"Enter product ID:");
                    int.TryParse(Console.ReadLine(), out newProductID);
                    try
                    {
                        dalList.Product.Delete(newProductID);
                        Console.WriteLine("The product is deleted\n");
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;
                default:
                    while  (action > 'f')
                    {
                        Console.WriteLine("invalid input, pleaase press again:");
                        char.TryParse(Console.ReadLine(), out action);
                    }
                    break;
            }

        } while (action != 'f');

    }


    static void OptionOrder(DalApi.IDal dalList)
    {
        int newOrderID;
        string newCustomerName;
        string newCustomerEmail;
        string CustomerAddress;
        DateTime newOrderDate;
        DateTime newShipDate;
        DateTime newDeliveryDate;
        char action;

        do
        {
            Console.WriteLine('\n'+@"Select the desired option:
a: Add order
b: Single order display
c: Display of all orders
d: Update order
e: Delete order
f: exit");

            char.TryParse(Console.ReadLine(), out action);
            
            switch(action)
            {
                //---------Add Order-----------//
                case 'a':
                    Console.WriteLine(@"
You have selected to add a new Order.
Enter customer name:");
                    newCustomerName = Console.ReadLine();
                    Console.WriteLine("Enter customer email:");
                    newCustomerEmail = Console.ReadLine();
                    Console.WriteLine("Enter customer address:");
                    CustomerAddress = Console.ReadLine();
                    newOrderDate = DateTime.Now;
                    Console.WriteLine("Enter delivery date");
                    DateTime.TryParse(Console.ReadLine(), out newDeliveryDate);
                    Console.WriteLine("Enter shipping date");
                    DateTime.TryParse(Console.ReadLine(), out newShipDate);
                    ///צריך לעשטת בדיקה שהתאריכים של ההגעה לא לפני ההזמנה

                    Order newOrder = new Order
                    {
                        CustomerName = newCustomerName,
                        CustomerEmail=newCustomerEmail,
                        CustomerAddress= CustomerAddress,
                        OrderDate = newOrderDate,
                        ShipDate = newShipDate,
                        DeliveryDate = newDeliveryDate,
                    };
                    try
                    {
                        dalList.Order.Add(newOrder);
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;

               //---------get single Order-----------//
               case 'b':
                    Console.WriteLine(@"Enter Order ID:");
                    int.TryParse(Console.ReadLine(), out newOrderID);
                    try
                    {
                        Console.WriteLine(dalList.Order.GetById(newOrderID));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                //---------get list Order-----------//
                case 'c':
                    foreach (var order in dalList.Order.GetList())
                    {
                        Console.WriteLine(order);
                    }
                    break;

                //---------update Order-----------//
                case 'd':
                    Console.WriteLine(@"
Enter order ID:");
                    int.TryParse(Console.ReadLine(), out newOrderID);
                    Console.WriteLine("Enter customer name:");
                    newCustomerName = Console.ReadLine();
                    Console.WriteLine("Enter customer email:");
                    newCustomerEmail = Console.ReadLine();
                    Console.WriteLine("Enter customer address:");
                    CustomerAddress = Console.ReadLine();
                    newOrderDate = DateTime.Now;
                    Console.WriteLine("Enter delivery date");
                    DateTime.TryParse(Console.ReadLine(), out newDeliveryDate);
                    Console.WriteLine("Enter shipping date");
                    DateTime.TryParse(Console.ReadLine(), out newShipDate);
                    ///צריך לעשטת בדיקה שהתאריכים של ההגעה לא לפני ההזמנה

                    Order updateOrder = new Order
                    {
                        ID = newOrderID,
                        CustomerName = newCustomerName,
                        CustomerEmail = newCustomerEmail,
                        CustomerAddress = CustomerAddress,
                        OrderDate = newOrderDate,
                        ShipDate = newShipDate,
                        DeliveryDate = newDeliveryDate,
                    };
                    try
                    {
                        dalList.Order.Update(updateOrder);
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;

                //---------delete Order-----------//
                case 'e':
                    Console.WriteLine(@"Enter Order ID:");
                    int.TryParse(Console.ReadLine(), out newOrderID);
                    try
                    {
                        dalList.Order.Delete(newOrderID);
                        Console.WriteLine("The Order is deleted\n");
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;
            }

        } while (action != 'f');

        //switch

    }


    static void OptionOrderItem(DalApi.IDal dalList)
    {
        int newOrderItemID;
        int newProductID;
        int newOrderID;
        double newPrice;
        int newAmount;
        char action;

        do
        {
            Console.WriteLine('\n' + @"Select the desired option:
a: Add OrderItem
b: Single OrderItem display
c: Display of all OrderItem
d: Update OrderItem
e: Delete OrderItem
f: exit");

            char.TryParse(Console.ReadLine(), out action);

            switch (action)
            {
                //---------Add OrderItem-----------//
                case 'a':
                    Console.WriteLine(@"
You have selected to add a new OrderItem.
Enter Product ID");
                    int.TryParse(Console.ReadLine(), out newProductID);
                    Console.WriteLine("Enter order ID:");
                    int.TryParse(Console.ReadLine(), out newOrderID);
                    Console.WriteLine("Enter price:");
                    double.TryParse(Console.ReadLine(), out newPrice);
                    Console.WriteLine("Enter Amount:");
                    int.TryParse(Console.ReadLine(), out newAmount);

                    OrderItem newOrderItem = new OrderItem
                    {
                        ProductID = newProductID,
                        OrderID = newOrderID,
                        Price = newPrice,
                        Amount = newAmount
                    };
                    try
                    {
                        dalList.OrderItem.Add(newOrderItem);
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;

                //---------get single OrderItem-----------//
                case 'b':
                    Console.WriteLine(@"Enter OrderItem ID:");
                    int.TryParse(Console.ReadLine(), out newOrderItemID);
                    try
                    {
                        Console.WriteLine(dalList.OrderItem.GetById(newOrderItemID));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                //---------get list Order-----------//
                case 'c':
                    foreach (var orderItem in dalList.OrderItem.GetList())
                    {
                        Console.WriteLine(orderItem);
                    }
                    break;

                //---------update OrderItem-----------//
                case 'd':
                    Console.WriteLine(@"
Enter OrderItem ID:");
                    int.TryParse(Console.ReadLine(), out newOrderItemID);
                    Console.WriteLine("Enter Product ID:");
                    int.TryParse(Console.ReadLine(), out newProductID);
                    Console.WriteLine("Enter order ID:");
                    int.TryParse(Console.ReadLine(), out newOrderID);
                    Console.WriteLine("Enter price:");
                    double.TryParse(Console.ReadLine(), out newPrice);
                    Console.WriteLine("Enter Amount:");
                    int.TryParse(Console.ReadLine(), out newAmount);

                    OrderItem updateOrderItem = new OrderItem
                    {
                        ID= newOrderItemID,
                        ProductID = newProductID,
                        OrderID = newOrderID,
                        Price = newPrice,
                        Amount = newAmount
                    };
                    try
                    {
                        dalList.OrderItem.Update(updateOrderItem);
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;

                //---------delete OrderItem-----------//
                case 'e':
                    Console.WriteLine(@"Enter OrderItem ID:");
                    int.TryParse(Console.ReadLine(), out newOrderItemID);
                    try
                    {
                        dalList.OrderItem.Delete(newOrderItemID);
                        Console.WriteLine("The OrderItem is deleted\n");
                    }
                    catch (Exception str)
                    {
                        Console.WriteLine(str);
                    }
                    break;
            }

        } while (action != 'f');
    }

    static void Main()
    {
        int choice;
        
        do
        {
            Console.WriteLine(@"Press 1 to choose Product option
Press 2 to choose Order option
Press 3 to choose Order items option
Press 0 to exit");
            int.TryParse(Console.ReadLine(), out choice);
            //choice = temp;
            //choice=Console.Read();  
            switch (choice)
            {
                case 1:
                    OptionProduct(dalList);
                    break;
                case 2:
                    OptionOrder(dalList);
                    break;
                case 3:
                    OptionOrderItem(dalList);
                    break;
                default:
                    break;
            }

        } while (choice != 0) ;
    }
}