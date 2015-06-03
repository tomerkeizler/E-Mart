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
using System.Collections.ObjectModel;
using System.IO;

namespace PL
{
    /// <summary>
    /// Interaction logic for PurchaseWindow.xaml
    /// </summary>
    public partial class PurchaseWindow : Window
    { 
        // static attributes
        public static PType[] typeIndex = new PType[3] { PType.Electronics, PType.Clothes, PType.Food };
        
        // attributes
        private PL_GUI parentWindow;
        private Object customer;
        private IBL itsProductBL;
        private Dictionary<ComboBox, int> currentTypes;
        private bool areThereAnyProducts;
        private ObservableCollection<Product> currentList;
        private ObservableCollection<Purchase> purchasesList;

        // constructor
        public PurchaseWindow(PL_GUI _parentWindow, Object _customer, IBL _itsProductBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            customer = _customer;
            itsProductBL = _itsProductBL;

            // initializing the dictionary with value 3="none" for each combobox
            currentTypes = new Dictionary<ComboBox, int>(3);
            currentTypes.Add(pType1, 3);
            currentTypes.Add(pType2, 3);
            currentTypes.Add(pType3, 3);

            // check if are there any products
            areThereAnyProducts = itsProductBL.GetAll().Any();
            if (!areThereAnyProducts)
            {
                ProductGrid.Visibility = Visibility.Collapsed;
                purchaseGrid.Visibility = Visibility.Collapsed;
                emptyStore.Visibility = Visibility.Visible;
            }

            // bind the datagrid to currentList
            currentList = new ObservableCollection<Product>();
            ProductGrid.DataContext = currentList;

            // bind the datagrid to shopping cart
            purchasesList = new ObservableCollection<Purchase>();
            purchaseGrid.DataContext = purchasesList;

            if (customer is Customer)
            {
                myName.Text = "Name: " + ((Customer)customer).FirstName + " " + ((Customer)customer).LastName;
                if (((Customer)customer).CreditCard != null)
                {
                    cardNumber.Text = ((Customer)customer).CreditCard.CreditNumber.ToString();
                    expDate.SelectedDate = ((Customer)customer).CreditCard.ExpirationDate;
                }
            }



        }


        private void UpdateProducts(object sender, SelectionChangedEventArgs e)
        {
            if (areThereAnyProducts)
            {
                bool areAllNull = true;
                foreach (var comboBox in currentTypes.Keys)
                    if (comboBox.SelectedItem != null && comboBox.SelectedIndex != 3)
                        areAllNull = false;
                if (areAllNull)
                    currentList.Clear();
                else
                {
                    ComboBox selectedComboBox = ((ComboBox)sender);
                    int TypesInStore = Enum.GetNames(typeof(PType)).Length;
                    int previousChoice = currentTypes[selectedComboBox];
                    if (!previousChoice.Equals(TypesInStore))
                        if (!isDuplicate(selectedComboBox, previousChoice))
                            ((Product_BL)itsProductBL).FilterProducts(currentList, typeIndex[previousChoice], false);

                    int newChoice = selectedComboBox.SelectedIndex;
                    if (!newChoice.Equals(TypesInStore))
                        if (!isDuplicate(selectedComboBox, newChoice))
                            ((Product_BL)itsProductBL).FilterProducts(currentList, typeIndex[newChoice], true);

                    currentTypes[selectedComboBox] = newChoice;
                }
            }
        }

        private bool isDuplicate(ComboBox cmb, int choice)
        {
            bool isDuplicate = false;
            foreach (KeyValuePair<ComboBox, int> pairComboboxType in currentTypes)
                if (!pairComboboxType.Key.Equals(cmb))
                    if (pairComboboxType.Value.Equals(choice))
                        isDuplicate = true;
            return isDuplicate;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            foreach (var comboBox in currentTypes.Keys)
                comboBox.ClearValue(ComboBox.SelectedItemProperty);
        }

        public void EmphasizeBestSellers(object sender, DataGridRowEventArgs e)
        {
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
            
            Product p = e.Row.DataContext as Product;
            if (p.IsTopSeller)
                e.Row.Background = myBrush;
        }

        private void Pay(object sender, RoutedEventArgs e)
        {
            paymentBox.Visibility = Visibility.Visible;
        }

        private void CompletePurchase(object sender, RoutedEventArgs e)
        {
            if (toSaveVisa.IsChecked == true)
            {
                Customer buyer = ((Customer)customer);
                CreditCard myVisa = new CreditCard(buyer.FirstName, buyer.LastName, buyer.CreditCard.CreditNumber, buyer.CreditCard.ExpirationDate);
                buyer.CreditCard = myVisa;
            }
            //Transaction t1 = new Transaction();
            MessageBox.Show("Thank you for your purchase!");
            this.Close();
        }

        private void DropProduct(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(Product));

        }

        private void paymentMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 2)
                creditCard.Visibility = Visibility.Visible;
            else
                creditCard.Visibility = Visibility.Collapsed;
        }

        private void ProductGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Product p = (Product)ProductGrid.SelectedItem;
            DragDrop.DoDragDrop(sender as DependencyObject, p, DragDropEffects.Move);
        }



    }
}
