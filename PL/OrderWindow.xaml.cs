using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.XPath;
using BlApi;
using BO;
using DalApi;
using DO;
using Microsoft.VisualBasic;

namespace PL
{
    

    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public Action<BO.OrderItem> ButtonClick { get; set; }
        //add view (manager)
        public OrderWindow()
        {
            InitializeComponent();
            AddOrderB.Visibility=Visibility.Visible;
            UpdateOrderB.Visibility = Visibility.Hidden;
            ButtonClick = ButtonClickHandler;
        }
        public void RefreshItemListView()
        {
            var order=this.DataContext as BO.Order;
            this.itemsList.ItemsSource = bl?.Order.GetOrderDetails(p => p.Value.ID==order?.ID).Items;
        }
        private void refreshWindow(object sender, EventArgs e)
        {
            RefreshItemListView();
        }

        //update view (manager)
        public OrderWindow(OrderForList  order )
        {
            InitializeComponent();
            itemsList.ItemsSource = bl.Order?.GetOrderDetails(o =>o.Value.ID==order.ID).Items;
            AddOrderB.Visibility = Visibility.Hidden;
            UpdateOrderB.Visibility = Visibility.Visible;
            DataContext = bl.Order.GetOrderDetails(o=>o.Value.ID == order.ID);
            //DataContext= orderToShow;

        }

        //user view
        public OrderWindow(BO.Order order, ICart cart )
        {
            InitializeComponent();
            AddOrderB.Visibility = Visibility.Hidden;
            UpdateOrderB.Visibility = Visibility.Hidden;
        }

        private void subtractItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as BO.OrderItem;
            int amount = 1;
            try
            {
                bl.Order.UpdateOrderByManager((DataContext as BO.Order).ID, item.ProductID, "subtract", amount);
                //ShowDialog();
                ProductListWindow productListWindow = new ProductListWindow();
                //Close();
            }
            catch
            {
                MessageBox.Show("product out of stock!");
                MessageBoxButton ok = MessageBoxButton.OK;

            }
            //new ProductListWindow().UpdateLayout();
            //new ProductListWindow().orderListView.UpdateLayout();
        }







        private void ButtonClickHandler(BO.OrderItem item)
        {
            // Do something with the item
            // ...
            int amount = 1;
            bl.Order.UpdateOrderByManager(item.ID, item.ProductID, "add", amount);
            
        }

        private void addToItemButton_Click(object sender, RoutedEventArgs e)
        {
            //var ListItem = DataContext as OrderWindow;
            //B
            //int amount = 1;
            //bl.Order.UpdateOrderByManager(item.ID, item.ProductID, "add", amount);
            //itemsList.DataContext = item;
            //OrderWindow productListWindow = new OrderWindow();
            var button = sender as Button;
            var item = button.DataContext as BO.OrderItem;
            //var item1 = (BO.OrderItem)sender.;
            var order= DataContext as BO.OrderForList;
            //var order = DataContext;
            int amount = 1;
            try
            {
                
                bl.Order.UpdateOrderByManager((DataContext as BO.Order).ID, item.ProductID, "addAmount", amount);
                //ShowDialog();
                //OrderWindow orderWindow = new OrderWindow();
                //orderWindow.ShowDialog();
                //Close();
                //OrderWindow orderWindow = new OrderWindow(order);
                this.Show();




            }
            catch (Exception ex)
            {
                MessageBox.Show("product out of stock!");
                MessageBoxButton ok = MessageBoxButton.OK;

            }
            //DataContext = bl.Order.GetOrderDetails((int)DataContext);
            //this.InitializeComponent();
            //Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);

            //Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
            //new ProductListWindow().UpdateLayout();
            //new ProductListWindow().orderListView.UpdateLayout();
            //ShowDialog();
            //OrderWindow orderWindow = new OrderWindow();
            //his.RefreshItemListView();

        }


        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as BO.OrderItem;
            try
            {
                bl.Order.UpdateOrderByManager((DataContext as BO.Order).ID, item.ProductID, "remove");
                ProductListWindow productListWindow = new ProductListWindow();
                //Close();
            }
            catch
            {
                MessageBox.Show("Error");
                MessageBoxButton ok = MessageBoxButton.OK;

            }
            new ProductListWindow().UpdateLayout();
            new ProductListWindow().orderListView.UpdateLayout();
        }
    }



}
