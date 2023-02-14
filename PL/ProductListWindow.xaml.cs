//using DO;
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
        orderListView.ItemsSource = bl.Order.GetOrderList();
        orderListView.Visibility=Visibility.Hidden;
    }

    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BO.Enum.Category category = (BO.Enum.Category)e.AddedItems[0];
        productListView.ItemsSource = bl?.Product?.GetProductListBySort( p => (BO.Enum.Category)p?.Category==category);
        Clear.Visibility = Visibility.Visible;
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        productListView.ItemsSource = bl?.Product.GetProductList(p => p != null);
        Clear.Visibility = Visibility.Hidden;
    }

    /// <summary>
    /// Adding a new product button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new UpdateProductWindow().Show();
    }

    /// <summary>
    /// refresh the list view that shows the customers information
    /// </summary>
    public void RefreshProductListView()
    {
        this.productListView.ItemsSource = bl?.Product.GetProductList(p => p != null);
    }
    public void RefreshOrderListView()
    {
        this.orderListView.ItemsSource = bl?.Order.GetOrderList(p => p != null);
    }


    /// <summary>
    /// refresh the list view of the products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void refreshWindow(object sender, EventArgs e)
    {
        RefreshProductListView();
        RefreshOrderListView();
    }
    private DataTable dt = new DataTable();
    private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        
        if (productListView.SelectedItems.Count >= 1)
        {
            BO.ProductForList item = (BO.ProductForList)productListView.SelectedItems[0];
            //updateProductWindow.updateProductView(item);
            new UpdateProductWindow(item).Show();
        }
    }

    private void orderButton_Click(object sender, RoutedEventArgs e)
    {
        addProductButton.Visibility = Visibility.Hidden;
        productListView.Visibility=Visibility.Hidden;
        CategorySelector.Visibility=Visibility.Hidden;
        categoryLabel.Visibility=Visibility.Hidden;
        orderListView.Visibility = Visibility.Visible;
    }

    private void productButton_Click(object sender, RoutedEventArgs e)
    {
        orderListView.Visibility = Visibility.Hidden;
        productListView.Visibility = Visibility.Visible;
        addProductButton.Visibility = Visibility.Visible;
    }

    private void addOrderButton_Click(object sender, RoutedEventArgs e)
    {
        new OrderWindow().Show();
    }

    private void orderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (orderListView.SelectedItems.Count >= 1)
        {
            BO.OrderForList item = (BO.OrderForList)orderListView.SelectedItems[0];
            if(item.Status!= BO.Enum.OrderStatus.Delivered)
            {
                new OrderWindow(item).Show();
            }
            else
            {
                new OrderWindow(item,"readonly").Show();
            }
        }
    }

    private void deleteButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var product = (BO.ProductForList)button.DataContext;
        //var product = (BO.ProductForList)((Button)sender).DataContext;
        try
        {
            string message = "Are you sure you want to delete the product?";
            var d = MessageBox.Show(" ", message, MessageBoxButton.YesNo);
            if (d == MessageBoxResult.Yes)
            {
                bl?.Product.Delete(product.ID);
                MessageBox.Show("Product deleted!");
                productListView.Items.Refresh();
            }
        }
        catch
        {
            MessageBox.Show("Error cant delete, the product exist in orders");
        }
       
    }

    private void GroupByCategory_Click(object sender, RoutedEventArgs e)
    {
        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(productListView.ItemsSource);
        PropertyGroupDescription groupDescription = new PropertyGroupDescription("Category");
        view.GroupDescriptions.Add(groupDescription);
        var button = (Button)sender;
        Clear.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// starts simulation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StartSimulation_Click(object sender, RoutedEventArgs e)
    {
        new SimulationWindow().Show();
    }
}

