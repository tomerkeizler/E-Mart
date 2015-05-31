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
    public partial class AddEditUser : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private Object oldObj;

        // constructor
        public AddEditUser(PL_GUI _parentWindow, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;

            oldObj = _oldObj;
            ResetToDefault();
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            username.Text = ((User)oldObj).UserName;
            password.Password = ((User)oldObj).Password;
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password };
            ResetToDefault();
        }

        // Add or edit
        private void AddOrEdit(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                User newObj = new User(username.Text, password.Password, ((User)oldObj).Person);
                //editing action
                if (parentWindow.EditDataEntity(oldObj, newObj, 7))
                    this.Close();
            }
        }


        private bool IsValid()
        {
            bool flag = true;
            flag = PL_GUI.RegExp(username.Text, "Username", 3);
            if (flag)
                flag = PL_GUI.RegExp(password.Password, "Password", 3);
            return flag;
        }


    }
}
