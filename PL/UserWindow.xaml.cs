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

namespace PL
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        static int cartID=999999;
        public UserWindow()
        {
            InitializeComponent();
            cartID++;
        }

        private void CatalogList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CatalogList.SelectedItems.Count >= 1)
            {
                BO.ProductForList item = (BO.ProductForList)CatalogList.SelectedItems[0];
                
                //new UpdateProductWindow(cart item).Show();
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
            //new CartWindow(userName).Show();
        }
    }
}
