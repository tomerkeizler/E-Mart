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
    public partial class QueryEmployee : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryEmployee(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { firstName, lastName, ID, specificDepID, rangeDepID, fromDepID, toDepID, specificSalary, rangeSalary, fromSalary, toSalary, specificSupID, rangeSupID, fromSupID, toSupID, male, female, rank };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByFirstName(object sender, RoutedEventArgs e)
        {
            if (parentWindow.SearchDataEntity(StringFields.firstName, firstName.Text, null, 4))
                this.Close();
        }

        private void SearchByLastName(object sender, RoutedEventArgs e)
        {
            if (parentWindow.SearchDataEntity(StringFields.lastName, lastName.Text, null, 4))
                this.Close();
        }

        private void SearchByID(object sender, RoutedEventArgs e)
        {
            if (parentWindow.SearchDataEntity(IntFields.id, int.Parse(ID.Text), int.Parse(ID.Text), 4))
                this.Close();
        }

        private void SearchByDepID(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(fromDepID.Text);
            String max = toDepID.Text;
            if (parentWindow.SearchDataEntity(IntFields.depID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 4))
                this.Close();
        }

        private void SearchBySalary(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(fromSalary.Text);
            String max = toSalary.Text;
            if (parentWindow.SearchDataEntity(IntFields.salary, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 4))
                this.Close();
        }

        private void SearchBySupID(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(fromSupID.Text);
            String max = toSupID.Text;
            if (parentWindow.SearchDataEntity(IntFields.supervisiorID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 4))
                this.Close();
        }

        private void SearchByGender(object sender, RoutedEventArgs e)
        {
            Gender gen;
            if (male.IsChecked == true)
                gen = Gender.Male;
            else
                gen = Gender.Female;
            if (parentWindow.SearchDataEntity(TypeFields.gender, gen, null, 4))
                this.Close();
        }

        private void SearchByRank(object sender, RoutedEventArgs e)
        {
            Rank rnk;
            if (rank.Text.Equals("Administrator"))
                rnk = Rank.Administrator;
            else if (rank.Text.Equals("Manager"))
                rnk = Rank.Manager;
            else if (rank.Text.Equals("Worker"))
                rnk = Rank.Worker;
            else
                rnk = Rank.Customer;
            if (parentWindow.SearchDataEntity(TypeFields.rank, rnk, null, 4))
                this.Close();
        }


    }
}
