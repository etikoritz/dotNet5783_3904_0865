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
using BO;
using BlApi;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public SignInWindow()
        {
            InitializeComponent();
            UserNameTextBox.Focus();
        }

        /// <summary>
        /// this window opens just after signing up- so the user won't need to insert his username and password again
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SignInWindow(string username, string password)
        {
            InitializeComponent();
            this.UserNameTextBox.Text = username;
            this.PasswordTextBox.Password = password;
        }

        /// <summary>
        /// Sign in button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignIn_Button_Click(object sender, RoutedEventArgs e)
        {
            //the username and password shoouldn't be empty
            if (UserNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Enter a Name.");
                UserNameTextBox.Focus();
            }
            else if (PasswordTextBox.Password.Length == 0)
            {
                MessageBox.Show("Enter a Password.");
                PasswordTextBox.Focus();
            }
            //else- username and password are not empty
            else
            {
                string name = UserNameTextBox.Text;
                string password = PasswordTextBox.Password;

                //ADMINS

                User user1 = new User()
                {
                    UserName = name,
                    Password = password,
                };
                if ((UserNameTextBox.Text == "shirel" && PasswordTextBox.Password == "shirel") || (UserNameTextBox.Text == "batsheva" && PasswordTextBox.Password == "batsheva"))
                    user1.Permission = Permit.Admin;
                else
                    user1.Permission = Permit.User;

                try
                {
                 
                    if (password == user1.Password)
                    {
                        if (user1.Permission == BO.Permit.User) //USER
                        {
                            new MainWindow(bl, user1).Show();

                            //new ParcelListWindowe(bl, user1).Show();
                            Close();
                        }
                        else if(user1.Permission == BO.Permit.Admin) //Admin
                        {
                            new MainWindow(bl, name).Show();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Sorry! Please enter correct user account");
                            UserNameTextBox.Clear();
                            PasswordTextBox.Clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry! Please enter correct Password");
                        PasswordTextBox.Clear();
                    }
                }
                catch (BO.BOBadUserException)
                {
                    MessageBox.Show("Sorry! Please enter existing UserName/Password");
                    UserNameTextBox.Clear();
                    PasswordTextBox.Clear();
                    //errormessage.TextWrapping = TextWrapping.WrapWithOverflow;
                }
            }
        }

        /// <summary>
        /// checks if all the fileds were filled in
        /// </summary>
        /// <returns></returns>
        private bool AllFieldsRequired()
        {
            if (UserNameTextBox.Text.Count() != 0 && PasswordTextBox.Password.Count() != 0)
                return true;
            return false;
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
        /// sign up button- open the sign up window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignUp_Button_Click(object sender, RoutedEventArgs e)
        {
            bool tmpIsMAnager = false;
            new SignUpWindow(tmpIsMAnager).Show();
            this.Close();
        }

        /// <summary>
        /// When enter key pushed then submit to sign in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignIn_Button_Click((object)sender, e);
            }
        }
    }
}
