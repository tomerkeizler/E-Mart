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
    public partial class QueryCustomer : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryCustomer(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { firstName, lastName, ID, specificTranID, rangeTranID, fromTranID, toTranID };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByFirstName(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(firstName.Text, "First name", 1))
                if (parentWindow.SearchDataEntity(StringFields.firstName, firstName.Text, null, 2))
                    this.Close();
        }

        private void SearchByLastName(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(lastName.Text, "Last name", 1))
                if (parentWindow.SearchDataEntity(StringFields.lastName, lastName.Text, null, 2))
                    this.Close();
        }

        private void SearchByID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(ID.Text, "ID", 0))
                if (parentWindow.SearchDataEntity(IntFields.id, int.Parse(ID.Text), int.Parse(ID.Text), 2))
                    this.Close();
        }

        private void SearchByTranID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromTranID.Text, toTranID.Text, "Transaction ID", rangeTranID, 2))
            {
                int min = int.Parse(fromTranID.Text);
                String max = toTranID.Text;
                if (parentWindow.SearchDataEntity(IntFields.tranHistory, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 2))
                    this.Close();
            }
        }


    }
}
