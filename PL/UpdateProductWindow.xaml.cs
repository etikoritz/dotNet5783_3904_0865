using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateProductWindow.xaml
    /// </summary>
    public partial class UpdateProductWindow : Window
    {
        private static IBl bl = new BlImplementation.BI();
        //public static void State(string state)
        //{
        //    if(state=="add")
        //        AddProduct.Visibility=Visibility.Visible;
        //    if(state=="update")
        //        UpdateButton.Visibility=Visibility.Visible;
        //}
        public enum State { Add, Update};
        static State state;
        public UpdateProductWindow()
        {
            InitializeComponent();
            categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enum.Category));

        }
        public void AddProducView()
        {
            AddProduct.Visibility = Visibility.Visible;
        }
        public void updateProductView()
        {
            UpdateButton.Visibility=Visibility.Visible;
        }

        private BO.Product Read()
        {
            int.TryParse(productIdTextBox.Text, out int id);
            string name = nameTextBox.Text;
            int.TryParse(inStockTextBox.Text, out int inStock);
            int.TryParse(priceTextBox.Text, out int price);
            BO.Enum.Category.TryParse(categoryComboBox.Text, out BO.Enum.Category category);
            BO.Product product = new()
            {
                ID = id,
                Name = name,
                InStock = inStock,
                Price = price,
                Category = (DO.Enums.Category)category
            };
            return product;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Product product = Read();
            bl.Product.Update(product);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            BO.Product product = Read();
            bl.Product.Add(product);
            ProductListWindow productListWindow = new ProductListWindow();
        }
    }
}
