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
    public partial class AddClubMembner : Window
    {
        // attributes
        private PL_GUI parentWindow;
        
        // constructor
        public AddClubMembner(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, password, firstName, lastName, ID, male, female, dateOfBirth };
            Helper.ClearForm(lst);
        }


        // Add a new club member
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Gender myGender;
            if (male.IsChecked == true)
                myGender = Gender.Male;
            else
                myGender = Gender.Female;

            ClubMember newClubMember = new ClubMember(int.Parse(ID.Text), firstName.Text, lastName.Text, dateOfBirth.SelectedDate.Value, myGender);
            User newUser = new User(username.Text, password.Password, newClubMember);

            //adding action
            if (parentWindow.AddDataEntity(newClubMember, newUser, 1))
                this.Close();
        }



    }
}
