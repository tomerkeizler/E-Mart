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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>

    public partial class QueryClubMember : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryClubMember(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { firstName, lastName, ID, specificMemberID, rangeMemberID, fromMemberID, toMemberID, specificTranID, rangeTranID, fromTranID, toTranID, male, female, fromDateOfBirth, toDateOfBirth, specificDateOfBirth, rangeDateOfBirth };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByFirstName(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(firstName.Text, "First name", 1))
                if (parentWindow.SearchDataEntity(StringFields.firstName, firstName.Text, null, 1))
                    this.Close();
        }

        private void SearchByLastName(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(lastName.Text, "Last name", 1))
                if (parentWindow.SearchDataEntity(StringFields.lastName, lastName.Text, null, 1))
                    this.Close();
        }

        private void SearchByID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(ID.Text, "ID", 0))
                if (parentWindow.SearchDataEntity(IntFields.id, int.Parse(ID.Text), int.Parse(ID.Text), 1))
                    this.Close();
        }

        private void SearchByMemberID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromMemberID.Text, toMemberID.Text, "Member ID", rangeMemberID, 2))
            {
                int min = int.Parse(fromMemberID.Text);
                String max = toMemberID.Text;
                if (parentWindow.SearchDataEntity(IntFields.memberID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 1))
                    this.Close();
            }
        }

        private void SearchByTranID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromTranID.Text, toTranID.Text, "Transaction ID", rangeTranID, 2))
            {
                int min = int.Parse(fromTranID.Text);
                String max = toTranID.Text;
                if (parentWindow.SearchDataEntity(IntFields.tranHistory, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 1))
                    this.Close();
            }
        }

        private void SearchByGender(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.DoubleRadioValidate(male, female, "Gender"))
            {
                Gender gen;
                if (male.IsChecked == true)
                    gen = Gender.Male;
                else
                    gen = Gender.Female;
                if (parentWindow.SearchDataEntity(TypeFields.gender, gen, null, 1))
                    this.Close();
            }
        }

        private void SearchByDateOfBirth(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromDateOfBirth.Text, toDateOfBirth.Text, "Date of birth", rangeDateOfBirth, 4))
            {
                int min = int.Parse(((DateTime)fromDateOfBirth.SelectedDate).ToString("yyyyMMdd"));
                String max = toDateOfBirth.Text;
                if (parentWindow.SearchDataEntity(IntFields.dateOfBirth, min, (max.Equals(String.Empty)) ? (min) : int.Parse(((DateTime)toDateOfBirth.SelectedDate).ToString("yyyyMMdd")), 1))
                    this.Close();
            }
        }


    }
}
