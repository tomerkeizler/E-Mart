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
using System.Text.RegularExpressions;

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
            allEmployees.Add(def); // gives an option to be an administrator

            isAdd = _isAdd;
            if (!isAdd)
            {
                loginDetails.Visibility = Visibility.Collapsed;
                AddEditTitle.Text = "Edit Employee";
                AddEditButton.Content = "Edit Employee";
                oldObj = _oldObj;
                allEmployees.Remove(((Employee)oldObj)); // prevents an option of employee being his own supervisor
                ResetToDefault();
            }
            supID.ItemsSource = allEmployees;
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            firstName.Text = ((Employee)oldObj).FirstName;
            lastName.Text = ((Employee)oldObj).LastName;
            ID.Text = Convert.ToString(((Employee)oldObj).Id);
            depID.SelectedItem = ((Department)parentWindow.cats[3].FindByNumber(IntFields.departmentID, ((Employee)oldObj).DepID, ((Employee)oldObj).DepID).First());
            depID.Text = ((Department)depID.SelectedItem).Name;
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
            if (IsValid())
            {
                Gender myGender;
                if (male.IsChecked == true)
                    myGender = Gender.Male;
                else
                    myGender = Gender.Female;

                Employee newObj = new Employee(firstName.Text, lastName.Text, int.Parse(ID.Text), myGender, ((Department)depID.SelectedItem).DepartmentID, int.Parse(salary.Text), int.Parse(supID.Text));

                //adding action
                if (isAdd)
                {
                    User newUser = new User(username.Text, password.Password, newObj);
                    if (parentWindow.AddDataEntity(newObj, newUser, 4))
                        this.Close();
                }
                //editing action
                else
                {
                    ///newObj.Rank = ((Employee)oldObj).Rank;
                    if (parentWindow.EditDataEntity(oldObj, newObj, 4))
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
            if (flag)
                flag = PL_GUI.DoubleRadioValidate(male, female, "Gender");
            if (flag)
                flag = PL_GUI.ComboboxValidate(depID, "Department name");
            if (flag)
                flag = PL_GUI.ComboboxValidate(supID, "Supervisor ID");
            if (flag)
                flag = PL_GUI.RegExp(salary.Text, "Salary", 2);
            return flag;
        }



    }
}
