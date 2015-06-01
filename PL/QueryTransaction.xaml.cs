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
            List<Control> lst = new List<Control>() { purchaseType, returnType, fromCurrentDate, toCurrentDate, specificCurrentDate, rangeCurrentDate, specificTranID, rangeTranID, fromTranID, toTranID, payment, specificPrdID, rangePrdID, fromPrdID, toPrdID };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByTranType(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.DoubleRadioValidate(purchaseType, returnType, "Gender"))
            {
                Is_a_return tranType;
                if (purchaseType.IsChecked == true)
                    tranType = Is_a_return.Purchase;
                else
                    tranType = Is_a_return.Return;
                if (parentWindow.SearchDataEntity(TypeFields.is_a_return, tranType, null, 6))
                    this.Close();
            }
        }

        private void SearchByDate(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromCurrentDate.Text, toCurrentDate.Text, "Date of transaction", rangeCurrentDate, 4))
            {
                int min = int.Parse(((DateTime)fromCurrentDate.SelectedDate).ToString("yyyyMMdd"));
                String max = toCurrentDate.Text;
                if (parentWindow.SearchDataEntity(IntFields.currentDate, min, (max.Equals(String.Empty)) ? (min) : int.Parse(((DateTime)toCurrentDate.SelectedDate).ToString("yyyyMMdd")), 1))
                    this.Close();
            }
        }

        private void SearchByTranID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromTranID.Text, toTranID.Text, "Transaction ID", rangeTranID, 2))
            {
                int min = int.Parse(fromTranID.Text);
                String max = toTranID.Text;
                if (parentWindow.SearchDataEntity(IntFields.tranHistory, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 6))
                    this.Close();
            }
        }

        private void SearchByPayment(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.ComboboxValidate(payment, "Payment method"))
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
        }

        private void SearchByProductID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromPrdID.Text, toPrdID.Text, "Product ID", rangePrdID, 2))
            {
                int min = int.Parse(fromPrdID.Text);
                String max = toPrdID.Text;
                if (parentWindow.SearchDataEntity(IntFields.productID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 6))
                    this.Close();
            }
        }


    }
}
