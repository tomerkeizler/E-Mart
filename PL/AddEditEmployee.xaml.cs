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
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private bool isAdd;
        private Object oldObj;

        // constructor
        public AddEditEmployee(PL_GUI _parentWindow, IBL itsDepartmentBL, IBL itsEmployeeBL, bool _isAdd, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            // Create departments list for choosing depID
            depID.ItemsSource = itsDepartmentBL.GetAll();
            // Create Employees list for choosing supID
            List<Object> allEmployees = itsEmployeeBL.GetAll();
            Employee def = new Employee();
            def.Id = 0;
            allEmployees.Add(def);
            supID.ItemsSource = allEmployees;

            isAdd = _isAdd;
            if (!isAdd)
            {
                loginDetails.Visibility = Visibility.Collapsed;
                AddEditTitle.Text = "Edit Employee";
                AddEditButton.Content = "Edit Employee";
                oldObj = _oldObj;
                ResetToDefault();
            }
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            firstName.Text = ((Employee)oldObj).FirstName;
            lastName.Text = ((Employee)oldObj).LastName;
            ID.Text = Convert.ToString(((Employee)oldObj).Id);
            depID.Text = Convert.ToString(((Employee)oldObj).DepID);
            supID.Text = Convert.ToString(((Employee)oldObj).SupervisiorID);
            salary.Text = Convert.ToString(((Employee)oldObj).Salary);
            if (((Employee)oldObj).Gender.Equals(Gender.Male))
                male.IsChecked = true;
            else
                female.IsChecked = true;
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password, firstName, lastName, ID, depID, supID, salary, male, female };
            if (isAdd)
                PL_GUI.ClearForm(lst);
            else
                ResetToDefault();
        }

        // Add or edit
        private void AddOrEdit(object sender, RoutedEventArgs e)
        {
            Gender myGender;
            if (male.IsChecked == true)
                myGender = Gender.Male;
            else
                myGender = Gender.Female;

            Employee newObj = new Employee(firstName.Text, lastName.Text, int.Parse(ID.Text), myGender, int.Parse(depID.Text), int.Parse(salary.Text), int.Parse(supID.Text));
            User newUser = new User(username.Text, password.Password, newObj);

            //adding action
            if (isAdd)
            {
                if (parentWindow.AddDataEntity(newObj, newUser, 4))
                    this.Close();
            }
            //editing action
            else
            {
                if (parentWindow.EditDataEntity(oldObj, newObj, 4))
                    this.Close();
            }
        }



    }
}
