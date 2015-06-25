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
using BL;
using Backend;
using System.IO;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private User_BL userBL;
        private bool isEndProccess;

        // constructors
        public Login(IBL _userBL, PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            userBL = (User_BL)_userBL;
            isEndProccess = true;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password };
            PL_GUI.ClearForm(lst);
        }

        // check username and password validity
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                User _user = null;
                try
                {
                    _user = (User)userBL.isItValidUser(new User(username.Text, password.Password, null));
                }
                catch (System.Data.DataException error)
                {
                    MessageBox.Show(error.Message);
                }

                if (_user != null)
                {
                    MessageBox.Show("Log in done successfully!\nPlease click OK to continue");

                    Rank _rank;
                    if (_user.Person is Customer || _user.Person is ClubMember)
                        _rank = Rank.Customer;
                    else
                        _rank = ((Employee)(_user.Person)).Rank;

                    parentWindow.user = _user;
                    parentWindow.rank = (int)_rank;

                    // display the username and permission in the main Window at the upper left square
                    parentWindow.title_name.Text = "Hey " + _user.ToString() + "!";
                    parentWindow.title_rank.Text = "Logged in as " + ((_user.Person is ClubMember) ? ("Club Member") : (_rank.ToString()));

                    parentWindow.Permissions(); // activate permissions control
                    isEndProccess = false;
                    this.Close();
                    parentWindow.Show();
                }
            }
        }

        // Open registration window
        private void Register(object sender, RoutedEventArgs e)
        {
            isEndProccess = false;
            this.Hide();
            AddEditCustomer reg = new AddEditCustomer(this, this.parentWindow, true, true, null);
            reg.Show();
        }

        // Go to E-MART as a guest
        private void BeMyGuest(object sender, RoutedEventArgs e)
        {
            parentWindow.user = null;
            parentWindow.rank = 4;

            parentWindow.title_name.Text = "Hey guest!";
            parentWindow.title_rank.Text = "Enjoy the store!";

            parentWindow.Permissions(); // activate permissions control
            isEndProccess = false;
            this.Close();
            parentWindow.Show();
        }
        

        private bool IsValid()
        {
            bool flag = PL_GUI.RegExp(username.Text, "User name", 3);
            if (flag)
                flag = PL_GUI.RegExp(password.Password, "Password", 3);
            return flag;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isEndProccess)
                Application.Current.Shutdown();
        }


    }
}
