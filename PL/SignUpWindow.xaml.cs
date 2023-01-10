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
using System.Diagnostics;
using System.Windows.Navigation;
using System.Text.RegularExpressions;
using BlApi;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {

        bool IsManager; //to now if registration of manger or user
        public static IBl? bl;

        /// <summary>
        /// initializing
        /// </summary>
        public SignUpWindow(bool tmpIsMAnager)
        {
            InitializeComponent();
            enterDetailsGrid.Visibility = Visibility.Visible;
            IsManager = tmpIsMAnager;
            bl = BlApi.Factory.Get();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        /// <summary>
        /// checks if all the fileds were filled in
        /// </summary>
        /// <returns></returns>
        private bool AllFieldsRequired()
        {
            if (FirstNameTextBox.Text.Length != 0 && LastNameTextBox.Text.Length != 0)
                return true;
            return false;
        }

        /// <summary>
        /// to reset registration
        /// </summary>
        public void Reset()
        {
            ChooseUserNameTextBox.Text = "";
            ChoosePasswordTextBox.Text = "";
            ConfirmPasswordTextBox.Password = "";
        }

        /// <summary>
        /// sign up
        /// </summary>
        private void SignUp()
        {
            if (AllFieldsRequired())
            {
                enterDetailsGrid.Visibility = Visibility.Collapsed;
                UserNameAndPassword_Grid.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("All fields are required to continue");
        }

        /// <summary>
        /// SignUp_Final_Button_Click- when finished entering the new user details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignUp_Final_Button_Click(object sender, RoutedEventArgs e)
        {
            if(FirstNameTextBox.Text.Length==0)
            {
                MessageBox.Show("Please enter name");
                FirstNameTextBox.Focus();
            }
            else if (LastNameTextBox.Text.Length==0)
            {
                MessageBox.Show("Please enter last name");
                LastNameTextBox.Focus();
            }
            else if (IDTextBox.Text.Length==0)
            {
                MessageBox.Show("Please enter ID");
                IDTextBox.Focus();
            }
            else if (PhoneTextBox.Text.Length==0)
            {
                MessageBox.Show("Please enter Phone number");
                PhoneTextBox.Focus();
            }
             else if (LongitudeTextBox.Text.Length==0)
            {
                MessageBox.Show("Please enter longitude");
                LongitudeTextBox.Focus();
            }
             else if (LatitudeTextBox.Text.Length==0)
            {
                MessageBox.Show("Please enter latitude");
                LatitudeTextBox.Focus();
            }
            else if (IDTextBox.Text.Length != 9)
                MessageBox.Show("ID should have 9 digits");
            else if (PhoneTextBox.Text.Length < 9 || PhoneTextBox.Text.Length > 10)
                MessageBox.Show("Incorrect phone number");
            else if (double.Parse(LongitudeTextBox.Text) < -180 || double.Parse(LongitudeTextBox.Text) > 180)
                MessageBox.Show("Incorrect Longitude");
            else if (double.Parse(LatitudeTextBox.Text) < -90 || double.Parse(LongitudeTextBox.Text) > 90)
                MessageBox.Show("Incorrect Latitude");
            else
                SignUp();
        }

        /// <summary>
        /// check that the text box includes numberic values only- you can't enter something that isn't digit
        /// from:https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// check that the text box includes letters only- you can't enter something that isn't letters
        /// from:https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlphabetValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// check that the text box includes letters only- you can't enter something that isn't letters
        /// from:https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// after choosing username and password---- add the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_SignUp_Button_Click(object sender, RoutedEventArgs e)
        {
            Submition();
        }

        /// <summary>
        /// Submition of registration
        /// checks that fields are full and sends registration to bl
        /// </summary>
        private void Submition()
        {
            if (ChooseUserNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Enter user name.");
                ChooseUserNameTextBox.Focus();
            }
            if (ChoosePasswordTextBox.Text.Length == 0)
            {
                MessageBox.Show("Enter password.");
                ChoosePasswordTextBox.Focus();
            }
            if (ChoosePasswordTextBox.Text.Length < 4)
            {
                MessageBox.Show("Password should include at least 4 digits");
                ChoosePasswordTextBox.Focus();
            }
            else if (ConfirmPasswordTextBox.Password.Length == 0)
            {
                MessageBox.Show("Confirm your password.");
                ConfirmPasswordTextBox.Focus();
            }
            else if (ChoosePasswordTextBox.Text != ConfirmPasswordTextBox.Password)
            {
                MessageBox.Show("Confirm password must be same as password.");
                ConfirmPasswordTextBox.Focus();
            }
            //the username and password are currect
            else
            {
                //keep the username and password in the memory
                string username = ChooseUserNameTextBox.Text;
                string password = ChoosePasswordTextBox.Text;
                BO.User user = new BO.User() //creates new user to register
                {
                    UserName = username,
                    Password = password,
                    Permission=Permit.User
                    
                };
                if (IsManager)
                {
                    user.Permission = BO.Permit.Admin;
                }
                else
                {
                    user.Permission = BO.Permit.User;
                }
                try
                {
                    bl.AddUser(user); //registers user
                    MessageBox.Show($"WELCOME {username}!");
                    Reset();
                }
                catch (BO.BOBadUserException exception)
                {
                    MessageBox.Show(exception.Message);
                    Reset();
                }
                //add the new customer to the BO layer
                BO.Customer customer = new BO.Customer()
                {
                    Id = Int32.Parse(IDTextBox.Text),
                    Name = FirstNameTextBox.Text,// + LastNameTextBox.Text,
                    Phone = PhoneTextBox.Text,
                };
                customer.Location = new BO.Location()
                {
                    Longitude = double.Parse(LongitudeTextBox.Text),
                    Latitude = double.Parse(LatitudeTextBox.Text),
                };
                //new customer doesn't have any parcels
                customer.SentParcels = null;
                customer.ReceiveParcels = null;
               customer.User = new() //creates new user to register
                {
                    UserName = username,
                    Password = password,
                    Permission = Permit.User

                };
                bl.AddCustomer(customer);
                //go to sign in page again- and doesn't need to fill in the username and password again
                new SignInWindow(username, password).Show();
                this.Close();
            }
        }



        /// <summary>
        /// When enter key pushed then submit to sign up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignUp();
            }
        }
        /// <summary>
        /// When enter key then submit form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submition();
            }
        }
    }
}
