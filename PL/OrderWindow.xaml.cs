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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.XPath;
using BlApi;
using BO;
using DalApi;
using DO;
using Microsoft.VisualBasic;

namespace PL;

/// <summary>
/// Interaction logic for Order.xaml
/// </summary>
public partial class OrderWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get();

    private ObservableCollection<BO.OrderItem> items = new ObservableCollection<BO.OrderItem>();

    private void refreshWindow(object sender, EventArgs e)
    {
        //RefreshItemListView();
        Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
    }
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="order"></param>
    /// <param name="readOnly"></param>
    public OrderWindow(OrderForList order, string readOnly)
    {
        InitializeComponent();
        //itemsList.ItemsSource = bl.Order?.GetOrderDetails(o => o?.ID == order.ID).Items;
        //DataContext = bl?.Order?.GetOrderDetails(o => o?.ID == order.ID);
        itemsList.ItemsSource = bl.Order?.GetOrderDetails(order.ID).Items;
        DataContext = bl?.Order?.GetOrderDetails(order.ID);
        this.IsEnabled = false;
    }

    /// <summary>
    /// default Ctor
    /// </summary>
    public OrderWindow()
    {
        InitializeComponent();
    }

    //update view (manager)
    public OrderWindow(OrderForList order)
    {
        InitializeComponent();
        //itemsList.ItemsSource = bl.Order?.GetOrderDetails(o =>o?.ID==order.ID).Items;
        //DataContext = bl?.Order?.GetOrderDetails(o=>o?.ID == order.ID);
        itemsList.ItemsSource = bl.Order?.GetOrderDetails(order.ID).Items;
        DataContext = bl?.Order?.GetOrderDetails(order.ID);
        // DeliveryDatePicker.Text = ((BO.Order)DataContext).DeliveryDate.ToString();

    }

    /// <summary>
    /// Clicking on the "-" to subtract one item from order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void subtractItemButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var item = (BO.OrderItem)button.DataContext;
        int amount = 1;
        try
        {
            bl?.Order.UpdateOrderByManager(((BO.Order)DataContext).ID, item.ProductID, "subtract", amount);
        }
        catch (BO.BODataNotExistException ex)
        {
            MessageBox.Show("The order is deleted!");
            Close();
        }
        //this.DataContext = bl?.Order.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID);
        //this.itemsList.ItemsSource = bl?.Order.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID).Items;
        this.DataContext = bl?.Order.GetOrderDetails((DataContext as BO.Order).ID);

        this.itemsList.ItemsSource = bl?.Order.GetOrderDetails((DataContext as BO.Order).ID).Items;
    }







    private void addToItemButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var item = (BO.OrderItem)button.DataContext;
        int amount = 1;
        try
        {
            bl?.Order.UpdateOrderByManager(((BO.Order)DataContext).ID, item.ProductID, "addAmount", amount);
        }
        catch
        {
            MessageBox.Show("product out of stock!");
        }
        //this.DataContext = bl?.Order.GetOrderDetails(o => o?.ID == ((BO.Order)DataContext).ID);
        //this.itemsList.ItemsSource = bl?.Order?.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID)?.Items;
        this.DataContext = bl?.Order.GetOrderDetails(((BO.Order)DataContext).ID);
        this.itemsList.ItemsSource = ((BO.Order)DataContext)?.Items;
    }

    /// <summary>
    /// button to remove item from cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var item = (BO.OrderItem)button.DataContext;
        try
        {
            bl?.Order.UpdateOrderByManager(((BO.Order)DataContext).ID, item.ProductID, "remove");
        }
        catch
        {
            MessageBox.Show("The order is deleted!");
            Close();
            new ProductListWindow().RefreshOrderListView();
        }
        //this.DataContext = bl?.Order?.GetOrderDetails(o => o?.ID == ((BO.Order)DataContext).ID);
        //this.itemsList.ItemsSource = bl?.Order?.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID)?.Items;
        this.DataContext = bl?.Order?.GetOrderDetails(((BO.Order)DataContext).ID);
        this.itemsList.ItemsSource = bl?.Order?.GetOrderDetails((DataContext as BO.Order).ID)?.Items;
    }

    

    private void DeliveryDatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
    {
        if (this.IsEnabled == false) return; //so if the file is for reading only the message box wont show up
        var item = (BO.Order)DataContext;
        DatePicker datePicker = sender as DatePicker;
        if (datePicker.SelectedDate.HasValue)
        {
            // Do something with the selected date
            DateTime selectedDate = datePicker.SelectedDate.Value;
            try
            {
                bl.Order.UpdateOrderSupply(item.ID, selectedDate.Date);
                this.DataContext = bl?.Order?.GetOrderDetails(((BO.Order)DataContext).ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

    private void ShippingDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.IsEnabled == false) return; //so if the file is for reading only the message box wont show up
        var item = (BO.Order)DataContext;
        DatePicker datePicker = sender as DatePicker;
        if (datePicker.SelectedDate.HasValue)
        {
            // Do something with the selected date
            DateTime selectedDate = datePicker.SelectedDate.Value;
            try
            {
                bl.Order.UpdateOrderDelivery(item.ID, selectedDate.Date);
                this.DataContext = bl?.Order?.GetOrderDetails(((BO.Order)DataContext).ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
