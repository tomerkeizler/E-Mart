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
using Backend;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class AddEditCustomer : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private bool isRegister;
        private string title;
        private bool isAdd;
        private Object oldObj;

        // constructor
        public AddEditCustomer(PL_GUI _parentWindow, bool _isRegister, bool _isAdd, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            isRegister = _isRegister;
            if (_isRegister)
                title = "Register to E-MART";
            else
                title = "Add Customer";
            customerForm.DataContext = title;

            isAdd = _isAdd;
            if (!isAdd)
            {
                loginDetails.Visibility = Visibility.Collapsed;
                AddEditTitle.Text = "Edit Customer";
                AddEditButton.Content = "Edit Customer";
                oldObj = _oldObj;
                ResetToDefault();
            }
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            firstName.Text = ((Customer)oldObj).FirstName;
            lastName.Text = ((Customer)oldObj).LastName;
            ID.Text = Convert.ToString(((Customer)oldObj).Id);
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password, firstName, lastName, ID };
            if (isAdd)
                PL_GUI.ClearForm(lst);
            else
                ResetToDefault();
        }

        // Add or edit
        private void AddOrEdit(object sender, RoutedEventArgs e)
        {
            Customer newObj = new Customer(int.Parse(ID.Text), firstName.Text, lastName.Text);
            User newUser = new User(username.Text, password.Password, newObj);

            //adding action
            if (isAdd)
            {
                if (parentWindow.AddDataEntity(newObj, newUser, 2))
                {
                    this.Close();

                    MessageBox.Show("Registration to E-MART done successfully!\nPlease click OK to continue");

                    // if this is a self registration - then send the user to the main window as a customer
                    if (isRegister)
                    {
                        Rank _rank = Rank.Customer;
                        int[] myPermissions = PL_GUI.allPermissions[3];
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

                        parentWindow.user = newUser;
                        parentWindow.rank = _rank;
                        parentWindow.viewPermissions = _viewPermissions;
                        parentWindow.fullPermissions = _fullPermissions;

                        // display the username and permission in the main Window at the upper left square
                        parentWindow.title_name.Text = "Hey " + newUser.UserName + "!";
                        parentWindow.title_rank.Text = "Logged in as " + Rank.Customer;

                        parentWindow.Show();
                    }
                }
            }
            //editing action
            else
            {
                newObj.CreditCard = ((Customer)oldObj).CreditCard;
                newObj.TranHistory = ((Customer)oldObj).TranHistory;
                if (parentWindow.EditDataEntity(oldObj, newObj, 2))
                    this.Close();
            }
        }




    }
}
