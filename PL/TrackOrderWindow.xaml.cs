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

namespace PL;

/// <summary>
/// Interaction logic for TrackOrderWindow.xaml
/// </summary>
public partial class TrackOrderWindow : Window
{
    public TrackOrderWindow()
    {
        InitializeComponent();
    }
    public TrackOrderWindow(BO.Order order)
    {
        InitializeComponent();
        this.OrderIDLable.Content = order.ID;
        this.OrderStatusLable.Content = order.Status;
        if (order.Status == BO.Enum.OrderStatus.Delivered)
        {
            ShippingGrid.Visibility= Visibility.Visible;
            DeliveryGrid.Visibility= Visibility.Visible;
            this.PaymentStatusLable.Content = order.OrderDate;
            this.ShippingLable.Content = order.ShipDate;
            this.DeliveryLable.Content = order.DeliveryDate;
        }
        else if (order.Status == BO.Enum.OrderStatus.Shipped)
        {
            ShippingGrid.Visibility = Visibility.Visible;
            this.PaymentStatusLable.Content = order.OrderDate;
            this.ShippingLable.Content = order.ShipDate;
        }
        else if (order.Status == BO.Enum.OrderStatus.Confirmed)
        {
            this.PaymentStatusLable.Content = order.OrderDate;
        }
        
    }


    
}
