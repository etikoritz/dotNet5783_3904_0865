﻿using System;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.XPath;
using BlApi;
using BO;
using DalApi;
using DO;
using Microsoft.VisualBasic;

namespace PL
{
    

    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        private ObservableCollection<BO.OrderItem> items = new ObservableCollection<BO.OrderItem>();


        //public void RefreshItemListView()
        //{
        //    Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        //}
        private void refreshWindow(object sender, EventArgs e)
        {
            //RefreshItemListView();
            Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        }
        public OrderWindow(OrderForList order, string readOnly)
        {
            InitializeComponent();
            itemsList.ItemsSource = bl.Order?.GetOrderDetails(o => o?.ID == order.ID).Items;
            DataContext = bl?.Order?.GetOrderDetails(o => o?.ID == order.ID);
            this.IsEnabled = false;
        }
        public OrderWindow()
        {
            InitializeComponent();

        }

        //update view (manager)
        public OrderWindow(OrderForList  order )
        {
            InitializeComponent();
            itemsList.ItemsSource = bl.Order?.GetOrderDetails(o =>o?.ID==order.ID).Items;
            DataContext = bl?.Order?.GetOrderDetails(o=>o?.ID == order.ID);

        }

        private void subtractItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item =(BO.OrderItem)button.DataContext;
            int amount = 1;
            try
            {
                bl?.Order.UpdateOrderByManager(((BO.Order)DataContext).ID, item.ProductID, "subtract", amount);
            }
            catch(BO.BODataNotExistException ex)
            {
                MessageBox.Show("The order is deleted!");
                Close();
            }
            this.DataContext = bl?.Order.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID);
            this.itemsList.ItemsSource = bl?.Order.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID).Items;
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
            this.DataContext = bl?.Order.GetOrderDetails(o => o?.ID == ((BO.Order)DataContext).ID);
            this.itemsList.ItemsSource = bl?.Order?.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID)?.Items;
        }


        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item =(BO.OrderItem)button.DataContext;
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
            this.DataContext = bl?.Order?.GetOrderDetails(o => o?.ID == ((BO.Order)DataContext).ID);
            this.itemsList.ItemsSource = bl?.Order?.GetOrderDetails(o => o?.ID == (DataContext as BO.Order)?.ID)?.Items;
        }
    }



}
