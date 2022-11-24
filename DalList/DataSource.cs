using DalApi;
using DO;
using System.Reflection.Emit;
using static DO.Enums;

namespace Dal;

internal static class DataSource
{
    internal static readonly Random random = new Random();
    internal static List<Product> productList = new List<Product>() { };
    internal static List<Order> orderList = new List<Order>() { };
    internal static List<OrderItem> orderItemList = new List<OrderItem>(new OrderItem[200]) { };
    internal static string[] productName = new string[] { "Laptopxr", "LaptopV11", "Desktopgameing23", "LaptopAppleM1", "Cellphon_ealaxy21", "iphone13", "ipadPro_Tablet", "ipadMini_Tablet", "nothing1_Headphones", "sonyX_Headphones" };
    internal static int[] productPrice = new int[] { 2500, 3000, 5000, 4500, 2000, 4000, 4200, 3000, 350, 100 };


    static DataSource()
    {
        s_Initialize();
    }
    internal static void s_Initialize()
    {   //data
        string[] CustomerName = new string[] { "Eti", "Yael", "Rachel", "Israel", "Meir", "David", "Sara", "Lea", "Avi", "Yair", "Eli", "Chedvi", "Riki", "Tal", "Tamir", "Hadas", "Efrat", "Tamar", "Rami", "Miriam" };
        string[] CustomerEmail = new string[] { "Eti@gmail.com", "Yael@gmail.com", "Rachel@gmail.com", "Israel@gmail.com", "Meir@gmail.com", "David@gmail.com", "Sara@gmail.com", "Lea@gmail.com", "Avi@gmail.com", "Yair@gmail.com", "Eli@gmail.com", "Chedvi@gmail.com", "Riki@gmail.com", "Tal@gmail.com", "Tamir@gmail.com", "Hadas@gmail.com", "Efrat@gmail.com", "Tamar@gmail.com", "Rami@gmail.com", "Miriam@gmail.com" };
        string[] CustomerAddress = new string[] { "Jerusalem", "Beit shemesh", "Beitar", "Netivot", "Tveria", "Hadera", "Rechassim", "Jerusalem", "Beit shemesh", "Beit shemesh", "Netivot", "Jerusalem", "Netivot", "Lod", "Jerusalem", "Tel aviv", "Raanana", "Bat yam", "Haifa", "Netivot" };

        //products
        for (int i = 0; i < 10; i++)                     //initial the product array
        {
            Product product = new Product();             
            product.ID = Confing.get_ID_Product;
            //for (int j = 0; j < 10; j++)                 //loop to theck if the id already belong to another product and if it does draw a new id, and check the new one..
            //{
            //    if (product.ID == productList[j].ID)
            //    {
            //        product.ID = Confing.get_ID_Product;
            //        j = 0;                               //reseting j in order to go all over they array again
            //    }
            //}
            while (productList.Exists(p => p.ID == product.ID))
            {
                product.ID = Confing.get_ID_Product;
            }
            int index = random.Next(0, 10);
            int indexEnum = random.Next(0, 6);
            product.Name = productName[index];
            product.Price = productPrice[index];
            if (product.Name.StartsWith("Laptop") || product.Name.EndsWith("Laptop"))
                product.Category = Enums.Category.Laptop;
            if (product.Name.StartsWith("Desktop") || product.Name.EndsWith("Desktop"))
                product.Category = Enums.Category.DesktopComputer;
            if (product.Name.StartsWith("Tablet") || product.Name.EndsWith("Tablet"))
                product.Category = Enums.Category.Tablet;
            if (product.Name.StartsWith("Cellphone") || product.Name.EndsWith("Cellphone"))
                product.Category = Enums.Category.Cellphone;
            if (product.Name.StartsWith("Headphones") || product.Name.EndsWith("Headphones"))
                product.Category = Enums.Category.Headphones;
            if (i < 8)                                  //i<8 so %5 of the products wont be in stock and their inStock value remain 0
            {
                int temp = random.Next(0, 100);
                product.InStock = temp;
            }
            productList.Add(product);
           
        }

        //order
        for (int i = 0; i < 20; i++)
        {
            int index = random.Next(0, 20);
            Order order = new Order();
            order.ID = Confing.get_ID_Order;
            //the order id is in ascending order so we dont need to check if two orders have the same id here

            //for (int j = 0; j < 20; j++)
            //{
            //    if (order.ID == orderArray[j].ID)
            //    {
            //        order.ID = Confing.get_ID_Order;
            //        j = 0;
            //    }
            //}
            order.CustomerName = CustomerName[index];
            order.CustomerEmail = CustomerEmail[index];
            order.CustomerAddress = CustomerAddress[index];
            order.OrderDate = DateTime.Now;
            int dayOfDelivery = random.Next(0, 7);//choose random num of days until the delivery
            TimeSpan spaceTime = TimeSpan.FromDays(dayOfDelivery);
            order.DeliveryDate = order.OrderDate + spaceTime;
            int dayOfShipping = random.Next(0, 7);//choosev random num of days until the shipping
            while (dayOfShipping < dayOfDelivery)// cheking that the shipping date is not earlier than the delivery date
            {
                dayOfShipping = random.Next(0, 7);
            }
            TimeSpan spaceTimeShipping = TimeSpan.FromDays(dayOfShipping);
            order.ShipDate = order.OrderDate + spaceTimeShipping;
            orderList.Add(order);
        }

        //Confing.indexOrder = 20;

        //orderItem
        for (int i = 0; i < 40; i++)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.ID = Confing.get_ID_OrderItem;
            //for (int j = 0; j < 40; j++)
            //{
            //    if (orderItem.ID == orderItemList[j].ID)
            //    {
            //        orderItem.ID = Confing.get_ID_OrderItem;
            //        j = 0;
            //    }
            //}
            while (orderItemList.Exists(o => o.ID == orderItem.ID))
            {
                orderItem.ID = Confing.get_ID_OrderItem;
            }
            int index = random.Next(0, 10);
            orderItem.ProductID = productList[index].ID;
            int index1 = random.Next(0, 20);
            orderItem.OrderID = orderList[index1].ID;
            orderItem.Price = productPrice[index];
            orderItem.Amount = random.Next(1, 4);
            orderItemList.Add(orderItem);
        }
        //Confing.indexOrderItem = 40;
    }

    internal static class Confing
    {
        //internal static int indexOrder = 20;
        //internal static int indexProduct = 10;
        //internal static int indexOrderItem = 40;
        private static int ID_Order = 99999;//reseting the id at 99999 so hthe id will start at 100000, (every time w'ill call it in get_ID_Order w'ill do ++..)  
        public static int get_ID_Order
        {
            get
            {
                ID_Order++;
                return ID_Order;
            }
        }
        //private static int ID_Product = 0;
        public static int get_ID_Product
        {
            get
            {
                int ID_Product = random.Next(100000, 1000000);
                return ID_Product;
            }
        }
        private static int ID_OrderItem = 100000; 
        public static int get_ID_OrderItem
        {
            get
            {
                ID_OrderItem++;
                return ID_OrderItem;
            }
        }
    }

}
