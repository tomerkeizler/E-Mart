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
        private bool canDrag;
        private Buyable rowBeingEdited;

        private Dictionary<ComboBox, int> currentTypes;
        private bool areThereAnyProducts;
        private ObservableCollection<Buyable> currentList;
        private ObservableCollection<Purchase> purchasesList;

        // constructor
        public PurchaseWindow(PL_GUI _parentWindow, Object _customer, IBL _itsProductBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            customer = _customer;
            itsProductBL = _itsProductBL;
            canDrag = true;
            rowBeingEdited = null;

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
            currentList = new ObservableCollection<Buyable>();
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

            Buyable b = e.Row.DataContext as Buyable;
            if (b.Prod.IsTopSeller)
                e.Row.Background = myBrush;
        }

        private void Pay(object sender, RoutedEventArgs e)
        {
            if (purchasesList.Any())
            {
                previewBox.Visibility = Visibility.Collapsed;
                paymentBox.Visibility = Visibility.Visible;
                amountColumn.IsReadOnly = true;
                canDrag = false;
            }
            else
                MessageBox.Show("You shopphing cart is empty!");
        }

        private void UnPay(object sender, RoutedEventArgs e)
        {
            if (purchasesList.Any())
            {
                previewBox.Visibility = Visibility.Visible;
                paymentBox.Visibility = Visibility.Collapsed;
                amountColumn.IsReadOnly = false;
                canDrag = true;
            }
            else
                MessageBox.Show("You shopphing cart is empty!");
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
            if (canDrag) // only if this now paying mode
            {
                Buyable data = (Buyable)e.Data.GetData(typeof(Buyable));
                if (data.Amount < 0)
                    MessageBox.Show("Invalid product amount");
                else if (data.Amount == 0)
                    MessageBox.Show("please choose an amount to buy");
                else if (data.Amount > data.Prod.StockCount)
                    MessageBox.Show("We don't have so many " + data.Prod.Name + "s at E-MART!");
                else
                {
                    AddSingleToCart(data);
                    data.ZeroAmount();
                    ProductGrid.Items.Refresh();
                }
            }
        }

        private void AddManyToCart(object sender, RoutedEventArgs e)
        {
            bool wasChosen = false;
            if (currentList.Any())
                foreach (Buyable b in currentList)
                    if (b.Amount > 0)
                    {
                        AddSingleToCart(b);
                        wasChosen = true;
                    }

            // no products were chosen
            if (!wasChosen)
                MessageBox.Show("Please choose some products");
            else            // reset all amounts of products in main datagrid of products in the store
                if (currentList.Any())
                    foreach (Buyable b in currentList)
                    {
                        b.ZeroAmount();
                    }

            // update the main table of products to zero amounts
            purchaseGrid.Items.Refresh();
        }

        private void AddSingleToCart(Buyable b)
        {
            Purchase toBuy = new Purchase(b.Prod.ProductID, b.Prod.Name, b.Prod.Price, b.Amount);

            // check if the same product is already in the shopping cart
            Purchase identical = null;
            if (purchasesList.Any())
                foreach (Purchase p in purchasesList)
                    if (p.PrdID == toBuy.PrdID)
                        identical = p;
            if (identical == null)
                purchasesList.Add(toBuy);
            else
                identical.Amount += toBuy.Amount;

            // refresh the shopping cart datagrid
            purchaseGrid.Items.Refresh();

            // update the stock of the product and refresh the products datagrid
            b.LeftInStock = b.LeftInStock - b.Amount;
            ProductGrid.Items.Refresh();

            // update the total price and total amount of products in the shopping cart
            String total = Convert.ToString(int.Parse(totalPrice1.Text) + (b.Prod.Price) * (b.Amount));
            totalPrice1.Text = total;
            totalPrice2.Text = total;
            totalAmount.Text = Convert.ToString(int.Parse(totalAmount.Text) + b.Amount);

            emptyCart.Visibility = Visibility.Collapsed;
            removals.Visibility = Visibility.Visible;
        }

        private void CallRemoveSingleFromCart(object sender, RoutedEventArgs e)
        {
            Object toRemove = purchaseGrid.SelectedItem;
            if (toRemove == null)
                MessageBox.Show("Please select a product to remove from the shopping cart");
            else
                RemoveSingleFromCart((Purchase)toRemove);
        }

        private void RemoveSingleFromCart(Purchase toRemove)
        {
            purchasesList.Remove(toRemove);

            // update the stock of the product
            foreach (Buyable b in currentList)
                if (b.Prod.ProductID == toRemove.PrdID)
                    b.LeftInStock = b.LeftInStock + toRemove.Amount;

            String total = Convert.ToString(int.Parse(totalPrice1.Text) - (toRemove.Price) * (toRemove.Amount));
            totalPrice1.Text = total;
            totalPrice2.Text = total;
            totalAmount.Text = Convert.ToString(int.Parse(totalAmount.Text) - toRemove.Amount);

            if (!purchasesList.Any())
            {
                emptyCart.Visibility = Visibility.Visible;
                removals.Visibility = Visibility.Collapsed;
            }
        }

        private void RemoveManyFromCart(object sender, RoutedEventArgs e)
        {
            // update the stock of the products
            foreach (Buyable b in currentList)
            {
                int plusToStock = purchasesList.Where(n => n.PrdID == b.Prod.ProductID).First().Amount;
                b.LeftInStock = b.LeftInStock + plusToStock;
            }

            purchasesList.Clear();

            // zero the total price and total amount of products in the shopping cart
            totalPrice1.Text = Convert.ToString(0);
            totalPrice2.Text = Convert.ToString(0);
            totalAmount.Text = Convert.ToString(0);
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
            if (ProductGrid.SelectedItem != null)
            {
                Buyable b = (Buyable)ProductGrid.SelectedItem;
                DragDrop.DoDragDrop(sender as DependencyObject, b, DragDropEffects.Move);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /*
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Buyable rowView = e.Row.Item as Buyable;
            rowBeingEdited = rowView;
        }

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                
                //rowBeingEdited.EndEdit();
            }
        }
        */

        






    }
}
