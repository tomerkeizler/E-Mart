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
    public partial class QueryProduct : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private IBL itsProductBL;

        //constructor
        public QueryProduct(PL_GUI _parentWindow, IBL _itsProductBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            itsProductBL = _itsProductBL;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { name, PType, specificDepID, rangeDepID, fromDepID, toDepID, specificPrice, rangePrice, fromPrice, toPrice, specificPrdID, rangePrdID, fromPrdID, toPrdID, PStatus, specificStockCount, rangeStockCount, fromStockCount, toStockCount };
            Helper.ClearForm(lst);
        }
    }
}
