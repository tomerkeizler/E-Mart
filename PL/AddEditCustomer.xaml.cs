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
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class AddEditCustomer : Window
    {
        // attributes
        private Login login;
        private PL_GUI parentWindow;
        private bool isRegister;
        private string title;
        private bool isAdd;
        private Object oldObj;

        // constructor
        public AddEditCustomer(Login _login, PL_GUI _parentWindow, bool _isRegister, bool _isAdd, Object _oldObj)
        {
            InitializeComponent();
            login = _login;
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
            if (isRegister)
                backButton.Visibility = Visibility.Visible;
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
            if (IsValid())
            {
                Customer newObj = new Customer(int.Parse(ID.Text), firstName.Text, lastName.Text);

                //adding action
                if (isAdd)
                {
                    User newUser = new User(username.Text, password.Password, newObj);
                    if (parentWindow.AddDataEntity(newObj, newUser, 2))
                    {
                        this.Close();
                        if (isRegister)
                        {
                            MessageBox.Show("Registration to E-MART done successfully!\nPlease click OK to continue");

                            // if this is a self registration - then send the user to the main window as a customer
                            parentWindow.user = newUser;
                            parentWindow.rank = (int)Rank.Customer;

                            // display the username and permission in the main Window at the upper left square
                            parentWindow.title_name.Text = "Hey " + newUser.UserName + "!";
                            parentWindow.title_rank.Text = "Logged in as " + Rank.Customer;

                            parentWindow.Permissions(); // activate permissions control
                            parentWindow.Show();

                            login.Close();
                        }
                        else
                            MessageBox.Show("Customer was added successfully!\nPlease click OK to continue");
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


        private bool IsValid()
        {
            bool flag = true;
            if (isAdd)
                flag = PL_GUI.RegExp(username.Text, "User name", 3);
            if (flag && isAdd)
                flag = PL_GUI.RegExp(password.Password, "Password", 3);
            if (flag)
                flag = PL_GUI.RegExp(firstName.Text, "First name", 1);
            if (flag)
                flag = PL_GUI.RegExp(lastName.Text, "Last name", 1);
            if (flag)
                flag = PL_GUI.RegExp(ID.Text, "ID", 0);
            return flag;
        }


        private void backToLogIn(object sender, RoutedEventArgs e)
        {
            this.Close();
            login.Show();
        }


    }
}
