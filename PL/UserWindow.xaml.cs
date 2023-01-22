using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        BO.Cart cart;
        private ObservableCollection<BO.OrderItem> orderitems = new ObservableCollection<BO.OrderItem>();
        public UserWindow(BO.Cart cartWithUserDetails)
        {
            InitializeComponent();
            CatalogList.ItemsSource = bl.Product.GetProductList(p => p != null);
            cart = cartWithUserDetails;
            cart.Items = new List<BO.OrderItem>();
            DataContext = new BO.Cart();
            CategorySelector.ItemsSource = System.Enum.GetValues(typeof(BO.Enum.Category));
        }
        public UserWindow()
        {
            InitializeComponent();
            CategorySelector.ItemsSource = System.Enum.GetValues(typeof(BO.Enum.Category));
        }

        private void CatalogList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CatalogList.SelectedItems.Count >= 1)
            {
                BO.ProductForList prod = (BO.ProductForList)CatalogList.SelectedItems[0];
                
                new UpdateProductWindow(prod).Show();
            }
        }


        /// <summary>
        /// Go to cart button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cartButton_Click(object sender, RoutedEventArgs e)
        {
            //להוסיף בדיקה של יוזר ולהגיע לסל הקניות הספציפי שלו
            new CartWindow(cart, orderitems).Show();

        }


        private void addItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (BO.ProductForList)button.DataContext;
            bl?.Cart.AddToCart(cart, item.ID);
            //orderitems.Add(bl.Cart.GetItemInCartList(cart).FirstOrDefault(o => o?.ProductID == item.ID));
            ///(cart?.Amount)(this.DataContext)=cart.Items.Count;
            ///
            //((BO.Cart)DataContext).Amount++;
            
            if(orderitems.Contains(bl?.Cart.GetItemInCartList(cart).FirstOrDefault(o => o?.ProductID == item.ID)))
            {
                BO.OrderItem updateItem = cart.Items.FirstOrDefault(p => p?.ProductID == item.ID);
                int index = orderitems.IndexOf(cart.Items.FirstOrDefault(p => p?.ProductID == item.ID));
                orderitems[index] = updateItem;
            }
            else
            {
                orderitems.Add(bl?.Cart.GetItemInCartList(cart).FirstOrDefault(o => o?.ProductID == item.ID));
            }
            //orderitems. = cart?.Items;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CatalogList.ItemsSource = bl?.Product.GetProductList(p => p != null);
            Clear.Visibility = Visibility.Hidden;
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Enum.Category category = (BO.Enum.Category)e.AddedItems[0];
            CatalogList.ItemsSource = bl.Product?.GetProductListBySort(p => (BO.Enum.Category)p?.Category == category);
            Clear.Visibility = Visibility.Visible;
        }
    }
}
