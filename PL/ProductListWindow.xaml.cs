
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for ProductListWindow.xaml
/// </summary>
public partial class ProductListWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get();



    public ProductListWindow()
    {
        InitializeComponent();
        CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enum.Category));
        productListView.ItemsSource = bl.Product.GetProductList(p =>p!=null);
    }

    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BO.Enum.Category category = (BO.Enum.Category)e.AddedItems[0];
        productListView.ItemsSource = bl.Product?.GetProductListBySort( p => (BO.Enum.Category)p?.Category==category);
        Clear.Visibility = Visibility.Visible;
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        productListView.ItemsSource = bl.Product.GetProductList(p => p != null);
    }

    /// <summary>
    /// Adding a new product button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        UpdateProductWindow updateProductWindow = new UpdateProductWindow();
        updateProductWindow.Show();
        updateProductWindow.AddProducView();
    }

    /// <summary>
    /// refresh the list view that shows the customers information
    /// </summary>
    public void RefreshProductListView()
    {
        this.productListView.ItemsSource = bl.Product.GetProductList(p => p != null);
    }

    /// <summary>
    /// refresh the list view of the products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void refreshWindow(object sender, EventArgs e)
    {
        RefreshProductListView();
    }
    private DataTable dt = new DataTable();
    private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        UpdateProductWindow updateProductWindow = new UpdateProductWindow();
        updateProductWindow.Show();
        if (productListView.SelectedItems.Count >= 1)
        {
            BO.ProductForList item = (BO.ProductForList)productListView.SelectedItems[0];
            updateProductWindow.updateProductView(item);
        }
        
        //Product product = productListView.SelectedItem.GetType(property);
        

    }


}

