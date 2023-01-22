using BlApi;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using System.CodeDom;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IBl? bl)
    {
        InitializeComponent();
    }
    public MainWindow()
    {
        InitializeComponent();
    }
    IBl bl = BlApi.Factory.Get();

    private void ___adminOptions__Click(object sender, RoutedEventArgs e)
    {
        new ProductListWindow().Show();
    }

    /// <summary>
    /// Track order button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TrackOrderButton_Click(object sender, RoutedEventArgs e)
    {
        int orderID = -1;
        Order order = new();

        try
        {
            orderID = Int32.Parse(this.trackOrderTextBox.Text);
            if (orderID > 0)
            {
                try
                {
                    order = bl.Order.GetOrderDetails(o=> o?.ID==orderID);
                    TrackOrderWindow trackOrderWindow = new TrackOrderWindow(order);
                    trackOrderWindow.Show();
                }
                catch (BO.BODataNotExistException ex)
                {
                    MessageBox.Show("Order ID does not exist, please try again");
                    this.trackOrderTextBox.Text = "";
                    this.trackOrderTextBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Order ID should be a positive number, please try again");
                this.trackOrderTextBox.Text = "";
                this.trackOrderTextBox.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Order ID should be a positive number, please try again");
            this.trackOrderTextBox.Text = "";
            this.trackOrderTextBox.Focus();
        }
    }

    private void trackOrderTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            TrackOrderButton_Click(sender, e);
        }
    }

    private void orderButton_Click(object sender, RoutedEventArgs e)
    {
        BO.Cart cart = new BO.Cart();
        new CustomerDetalesWindow(cart).Show();
    }
}
