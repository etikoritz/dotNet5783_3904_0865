using BlApi;
using MaterialDesignThemes.Wpf.Converters;
//using DO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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


namespace PL;

/// <summary>
/// Interaction logic for UpdateProductWindow.xaml
/// </summary>
public partial class UpdateProductWindow : Window
{
    private static IBl bl = BlApi.Factory.Get();
    BlApi.IBl? bl2 = BlApi.Factory.Get();
    private ObservableCollection<BO.OrderItem> orderitems = new ObservableCollection<BO.OrderItem>();
    private static BO.Product? Product1;
    //  BO.Product product;




    //public static void State(string state)
    //{
    //    if(state=="add")
    //        AddProduct.Visibility=Visibility.Visible;
    //    if(state=="update")
    //        UpdateButton.Visibility=Visibility.Visible;
    //}




    /// <summary>
    /// ctor for user
    /// </summary>
    /// <param name="productForList"></param>
    public UpdateProductWindow(BO.ProductForList productForList)
    {
        InitializeComponent();
        DataContext = bl.Product.GetProductList(p => p != null);
        categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enum.Category));
        UpdateButton.Visibility = Visibility.Visible;
        AddProduct.Visibility = Visibility.Hidden;
        this.DataContext = bl.Product.GetProductDetails(productForList.ID);

    }

    /// <summary>
    /// ctor for admin
    /// </summary>
    public UpdateProductWindow()
    {
        InitializeComponent();
        //DataContext = bl.Product.GetProductList(p => p != null);
        categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enum.Category));
        UpdateButton.Visibility = Visibility.Hidden;
        AddProduct.Visibility = Visibility.Visible;
        DataContext = new BO.Product();
        

        //DataContext = new items();
    }

    //public void AddProducView()
    //{
    //    AddProduct.Visibility = Visibility.Visible;
    //}

    //public void updateProductView(BO.ProductForList product)
    //{
    //    UpdateButton.Visibility=Visibility.Visible;

    //    productIdTextBox.Text = (product.ID).ToString();
    //    productIdTextBox.IsReadOnly= true;
    //    priceTextBox.Text = (product.Price).ToString();
    //    inStockTextBox.Text = (product.inStock).ToString();
    //    nameTextBox.Text=product.Name;
    //    categoryComboBox.Text= product.Category.ToString();


    private BO.Product Read()
    {
        int.TryParse((DataContext as BO.Product).ID.ToString(), out int id);

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
            Category = (BO.Enum.Category)category
        };
        return product;
    }
    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {

        var button = (Button)sender;
        var product = (BO.Product)button.DataContext;
        bl.Product.Update(product);
        MessageBox.Show("product updated seccessfully!");
        ProductListWindow productListWindow = new ProductListWindow();
        Close();
    }

    private void AddProduct_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var product = (BO.Product)button.DataContext;
        try
        {
            bl.Product.Add(product);
            MessageBox.Show("product added seccessfully!");
            ProductListWindow productListWindow = new ProductListWindow();
            Close();
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }


    }
}
