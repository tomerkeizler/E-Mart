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
using Backend;
using BL;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddEditTransaction : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private Object oldObj;

        // constructor
        public AddEditTransaction(PL_GUI _parentWindow, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            oldObj = _oldObj;
            ResetToDefault();
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            if (((Transaction)oldObj).Is_a_Return.Equals(Is_a_return.Purchase))
                purchaseType.IsChecked = true;
            else
                returnType.IsChecked = true;

            if (((Transaction)oldObj).Payment.Equals(PaymentMethod.Cash))
                payment.Text = "Cash";
            else if (((Transaction)oldObj).Payment.Equals(PaymentMethod.Check))
                payment.Text = "Check";
            else
                payment.Text = "Visa";
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { purchaseType, returnType, payment };
            ResetToDefault();
        }

        // Add or edit
        private void AddOrEdit(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                Is_a_return tranType;
                if (purchaseType.IsChecked == true)
                    tranType = Is_a_return.Purchase;
                else
                    tranType = Is_a_return.Return;

                PaymentMethod myPayment;
                var selectedItem = payment.SelectedItem as ComboBoxItem;
                if (selectedItem.Name.Equals("Cash"))
                    myPayment = PaymentMethod.Cash;
                else if (selectedItem.Name.Equals("Check"))
                    myPayment = PaymentMethod.Check;
                else
                    myPayment = PaymentMethod.Visa;

                Transaction newObj = new Transaction(((Transaction)oldObj).TransactionID, tranType, ((Transaction)oldObj).Receipt, myPayment);

                //editing action
                if (parentWindow.EditDataEntity(oldObj, newObj, 6))
                    this.Close();
            }
        }


        private bool IsValid()
        {
            bool flag = true;
            if (purchaseType.IsChecked == false && returnType.IsChecked == false)
            {
                MessageBox.Show("Transaction type must be selected");
                flag = false;
            }
            else if (payment.SelectedIndex == -1)
            {
                MessageBox.Show("Payment method must be selected");
                flag = false;
            }
            return flag;
        }


    }
}
