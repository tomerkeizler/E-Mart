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
    public partial class QueryTransaction : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private IBL itsTransactionBL;

        //constructor
        public QueryTransaction(PL_GUI _parentWindow, IBL _itsTransactionBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            itsTransactionBL = _itsTransactionBL;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { is_a_return, currentDate, specificTranID, rangeTranID, fromTranID, toTranID, payment, specificPrdID, rangePrdID, fromPrdID, toPrdID };
            Helper.ClearForm(lst);
        }
    }
}
