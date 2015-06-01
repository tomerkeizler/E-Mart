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
    public partial class QueryDepartment : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryDepartment(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { depName, specificDepID, rangeDepID, fromDepID, toDepID };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByDepName(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(depName.Text, "Department name", 1))
                if (parentWindow.SearchDataEntity(StringFields.name, depName.Text, null, 3))
                    this.Close();
        }

        private void SearchByDepID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromDepID.Text, toDepID.Text, "Department ID", rangeDepID, 2))
            {
                int min = int.Parse(fromDepID.Text);
                String max = toDepID.Text;
                if (parentWindow.SearchDataEntity(IntFields.departmentID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 3))
                    this.Close();
            }
        }


    }
}
