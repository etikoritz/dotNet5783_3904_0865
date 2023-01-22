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

        public CartWindow()
        {
            InitializeComponent();
        }
        public CartWindow(BO.Cart cart)
        {
            InitializeComponent();
            orderItemList.ItemsSource=bl.Cart.GetItemInCartList(cart);
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addToItemButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private void subtractItemButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
