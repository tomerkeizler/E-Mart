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

        // constructors
        public Login(IBL _userBL, PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            userBL = (User_BL)_userBL;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password };
            PL_GUI.ClearForm(lst);
        }

        // check username and password validity
        private void Button_Click(object sender, RoutedEventArgs e)
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

                int[] myPermissions = PL_GUI.allPermissions[(int)_rank];
                bool[] _viewPermissions = new bool[9];
                bool[] _fullPermissions = new bool[9];

                for (int i = 0; i < 9; i++)
                {
                    if (myPermissions[i] > 0)
                    {
                        _viewPermissions[i] = true;
                        if (myPermissions[i] == 1)
                            _fullPermissions[i] = false;
                        else
                            _fullPermissions[i] = true;
                    }
                    else
                    {
                        _viewPermissions[i] = false;
                        _fullPermissions[i] = false;
                    }
                }

                /////////////////
                _viewPermissions[1] = false;
                /////////////////

                parentWindow.user = _user;
                parentWindow.rank = _rank;
                parentWindow.viewPermissions = _viewPermissions;
                parentWindow.fullPermissions = _fullPermissions;

                // display the username and permission in the main Window at the upper left square
                parentWindow.title_name.Text = "Hey " + _user.ToString() + "!";
                parentWindow.title_rank.Text = "Logged in as " + _rank.ToString();

                this.Close();
                parentWindow.Show();
            }
        }

        // Open registration window
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            AddEditCustomer reg = new AddEditCustomer(this.parentWindow, true, true, null);
            reg.Show();
        }


    }
}
