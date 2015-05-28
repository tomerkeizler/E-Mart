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
    public partial class QueryTransaction : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryTransaction(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { purchaseType, returnType, currentDate, specificTranID, rangeTranID, fromTranID, toTranID, payment, specificPrdID, rangePrdID, fromPrdID, toPrdID };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByTranType(object sender, RoutedEventArgs e)
        {
            Is_a_return tranType;
            if (purchaseType.IsChecked == true)
                tranType = Is_a_return.Purchase;
            else
                tranType = Is_a_return.Return;
            if (parentWindow.SearchDataEntity(TypeFields.is_a_return, tranType, null, 6))
                this.Close();
        }

        private void SearchByDate(object sender, RoutedEventArgs e)
        {
            if (parentWindow.SearchDataEntity(StringFields.currentDate, ((DateTime)currentDate.SelectedDate).ToShortDateString(), null, 6))
                this.Close();
        }

        private void SearchByTranID(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(fromTranID.Text);
            String max = toTranID.Text;
            if (parentWindow.SearchDataEntity(IntFields.tranHistory, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 6))
                this.Close();
        }

        private void SearchByPayment(object sender, RoutedEventArgs e)
        {
            PaymentMethod pay;
            if (payment.Text.Equals("Cash"))
                pay = PaymentMethod.Cash;
            else if (payment.Text.Equals("Visa"))
                pay = PaymentMethod.Visa;
            else
                pay = PaymentMethod.Check;
            if (parentWindow.SearchDataEntity(TypeFields.payment, pay, null, 6))
                this.Close();
        }

        private void SearchByProductID(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(fromPrdID.Text);
            String max = toPrdID.Text;
            if (parentWindow.SearchDataEntity(IntFields.productID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 6))
                this.Close();
        }

    }
}
