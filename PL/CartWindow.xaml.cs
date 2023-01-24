using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<BO.OrderItem?> items = new ObservableCollection<BO.OrderItem?>();
        BlApi.IBl? bl = BlApi.Factory.Get();
        BO.Cart cart1;
        private void refreshWindow(object sender, EventArgs e)
        {
            //RefreshItemListView();
            Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        }
        public CartWindow()
        {
            InitializeComponent();
        }
        public CartWindow(BO.Cart cart, List<BO.OrderItem> orderItems)
        {
            InitializeComponent();
            orderItemList.ItemsSource=orderItems;
            DataContext = cart;
            cart1 = cart;
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.OrderItem)button.DataContext;
            bl?.Cart.DeleteFromeCart(cart1, item.ProductID);
            DataContext = bl?.Cart.GetItemInCartList(cart1);
            orderItemList.Items.Refresh();
        }
       

        private void addToItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.OrderItem)button.DataContext;
            bl?.Cart.AddToCart(cart1, item.ProductID);
            DataContext = bl?.Cart.GetItemInCartList(cart1);
            orderItemList.Items.Refresh();
        }
        private void subtractItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.OrderItem)button.DataContext;
            int amount = 1;
            bl?.Cart.UpdateAmount(cart1, item.ProductID, amount);
            DataContext = bl?.Cart.GetItemInCartList(cart1);
            orderItemList.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //new UserWindow().Close();
                int id = bl.Cart.ConfirmOrder(cart1);
                MessageBox.Show(@$"your order is confirmed!
your orderID is: {id}");
                cart1.Items.Clear();
                cart1.TotalPrice = 0;
                this.Close();
                new UserWindow(cart1).Show();
                //orderItemList.Items.Clear();
            }
            catch(OutOfStockProductException ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow(cart1).Show();
            this.Close();
        }
    }
}
