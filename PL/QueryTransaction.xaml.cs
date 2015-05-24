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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class QueryTransaction : Window
    {
        public QueryTransaction()
        {
            InitializeComponent();
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { is_a_return, currentDate, specificTranID, rangeTranID, fromTranID, toTranID, payment, specificPrdID, rangePrdID, fromPrdID, toPrdID };
            Helper.ClearForm(lst);
        }
    }
}
