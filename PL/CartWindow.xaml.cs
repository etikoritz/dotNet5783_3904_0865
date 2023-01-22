using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        static int count = 0;
        BlApi.IBl? bl = BlApi.Factory.Get();
        BO.Cart cart1;

        public CartWindow()
        {
            InitializeComponent();
        }
        public CartWindow(BO.Cart cart)
        {
            InitializeComponent();
            orderItemList.ItemsSource=bl.Cart.GetItemInCartList(cart);
            cart1 = cart;
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.OrderItem)button.DataContext;
            cart1.Items.Remove(item);
            orderItemList.Items.Remove(item);
            //bl?.Cart.GetItemInCartList(cart1).Select(o=>o?.ProductID==item.ProductID).r;
            orderItemList.Items.DeferRefresh();
        }

        private void addToItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.OrderItem)button.DataContext;
            bl?.Cart.AddToCart(cart1, item.ProductID);
            orderItemList.Items.DeferRefresh();
        }
        private void subtractItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.OrderItem)button.DataContext;
            int amount = 1;
            //orderItemList.Items.
            bl?.Cart.UpdateAmount(cart1, item.ProductID, amount);
            orderItemList.Items.DeferRefresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id= bl.Cart.ConfirmOrder(cart1);
            MessageBox.Show(@$"your order is confirmed!
your orderID is: {id}");
            this.Close();

        }
    }
}
