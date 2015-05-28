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
    public partial class AddEditClubMember : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private bool isAdd;
        private Object oldObj;

        // constructor
        public AddEditClubMember(PL_GUI _parentWindow, bool _isAdd, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;

            isAdd = _isAdd;
            if (!isAdd)
            {
                loginDetails.Visibility = Visibility.Collapsed;
                AddEditTitle.Text = "Edit Club Member";
                AddEditButton.Content = "Edit Club Member";
                oldObj = _oldObj;
                ResetToDefault();
            }
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            firstName.Text = ((ClubMember)oldObj).FirstName;
            lastName.Text = ((ClubMember)oldObj).LastName;
            ID.Text = Convert.ToString(((ClubMember)oldObj).Id);
            if (((ClubMember)oldObj).Gender.Equals(Gender.Male))
                male.IsChecked = true;
            else
                female.IsChecked = true;
            dateOfBirth.SelectedDate = ((ClubMember)oldObj).DateOfBirth;
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password, firstName, lastName, ID, male, female, dateOfBirth };
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

            ClubMember newObj = new ClubMember(int.Parse(ID.Text), firstName.Text, lastName.Text, dateOfBirth.SelectedDate.Value, myGender);
            User newUser = new User(username.Text, password.Password, newObj);

            //adding action
            if (isAdd)
            {
                if (parentWindow.AddDataEntity(newObj, newUser, 1))
                    this.Close();
            }
            //editing action
            else
            {
                newObj.CreditCard = ((ClubMember)oldObj).CreditCard;
                newObj.TranHistory = ((ClubMember)oldObj).TranHistory;
                if (parentWindow.EditDataEntity(oldObj, newObj, 1))
                    this.Close();
            }
        }



    }
}
