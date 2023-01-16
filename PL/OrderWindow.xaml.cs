using System;
using System.Collections.Generic;
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
using BlApi;
using BO;
using DO;

namespace PL
{

    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        //add view (manager)
        public OrderWindow()
        {
            InitializeComponent();
            AddOrderB.Visibility=Visibility.Visible;
            UpdateOrderB.Visibility = Visibility.Hidden;
        }

        //update view (manager)
        public OrderWindow(OrderForList  order )
        {
            InitializeComponent();
            itemsList.ItemsSource = bl.Order.GetOrderDetails(order.ID).Items;
            AddOrderB.Visibility = Visibility.Hidden;
            UpdateOrderB.Visibility = Visibility.Visible;
            BO.Order orderToShow = bl.Order.GetOrderDetails(order.ID);
            DataContext= orderToShow;

        }

        //user view
        public OrderWindow(IOrder order, ICart cart )
        {
            InitializeComponent();
            AddOrderB.Visibility = Visibility.Hidden;
            UpdateOrderB.Visibility = Visibility.Hidden;
        }

        private void AddAmountB_Click(object sender, RoutedEventArgs e)
        {
            //var button = (Button)sender;
            //var obj = (BO.OrderItem)button.Tag;
            //bl.Order.UpdateOrderByManager(obj.ID, obj.ProductID, "add", DataContext);

        }
    }
}
