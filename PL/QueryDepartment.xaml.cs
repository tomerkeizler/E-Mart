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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class QueryDepartment : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private IBL itsDepartmentBL;

        //constructor
        public QueryDepartment(PL_GUI _parentWindow, IBL _itsDepartmentBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            itsDepartmentBL = _itsDepartmentBL;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { depName, specificDepID, rangeDepID, fromDepID, toDepID };
            Helper.ClearForm(lst);
        }
    }
}
