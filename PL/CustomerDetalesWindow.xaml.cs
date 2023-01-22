using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerDetalesWindow.xaml
    /// </summary>
    public partial class CustomerDetalesWindow : Window
    {
        static int count = 0;
        BlApi.IBl? bl = BlApi.Factory.Get();

        public CustomerDetalesWindow(BO.Cart cart)
        {
            InitializeComponent();
            DataContext=cart;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(((BO.Cart)DataContext).CustomerAddress==null|| ((BO.Cart)DataContext).CustomerName==null|| ((BO.Cart)DataContext).CustomerEmail==null)
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                new UserWindow(((BO.Cart)DataContext)).Show();
            }
        }
        //private Trigger UpdateSourceTrigger PropertyChanged();
        //private void PropertyChanged(string propertyName)
        //{
        //   // PropertyChangedEventHandler handler = PropertyChanged;
        //    DataContext = propertyName;
        //    //if (handler != null)
        //    //    handler(this, new PropertyChangedEventArgs(propertyName));
        //    count++;
        //}


        //private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //     cart.CustomerName = ((BO.Cart)DataContext).CustomerName;
        //}

        //private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    cart.CustomerEmail = ((sender as TextBox).DataContext).ToString();
        //}

        //private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    cart.CustomerAddress = ((sender as TextBox).DataContext).ToString();
        //}
    }
    
}
