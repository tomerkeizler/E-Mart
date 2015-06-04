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
        private Object buyer;
        private IBL itsProductBL;
        private bool canDrag;

        private Dictionary<ComboBox, int> currentTypes;
        private bool areThereAnyProducts;
        private ObservableCollection<Buyable> currentList;
        private ObservableCollection<Purchase> purchasesList;

        // constructor
        public PurchaseWindow(PL_GUI _parentWindow, Object _buyer, IBL _itsProductBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            buyer = _buyer;
            itsProductBL = _itsProductBL;
            canDrag = true;

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

            if (buyer != null)
            {
                if (buyer is Customer)
                {
                    myName.Text = "Name: " + ((Customer)buyer).FirstName + " " + ((Customer)buyer).LastName;
                    if (((Customer)buyer).CreditCard != null)
                    {
                        cardNumber.Text = ((Customer)buyer).CreditCard.CreditNumber.ToString();
                        expDate.SelectedDate = ((Customer)buyer).CreditCard.ExpirationDate;
                    }
                }
                else
                    myName.Text = "Name: " + ((Employee)buyer).FirstName + " " + ((Employee)buyer).LastName;
            }
            else
                toSaveVisa.Visibility = Visibility.Collapsed;
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
            if (!purchasesList.Any())
                MessageBox.Show("There are no products in your shopping cart!");
            else
            {
                PaymentMethod myPaymentType;
                if (paymentMethod.SelectedIndex == 0)
                    myPaymentType = PaymentMethod.Cash;
                else if (paymentMethod.SelectedIndex == 1)
                    myPaymentType = PaymentMethod.Check;
                else
                {
                    myPaymentType = PaymentMethod.Visa;

                    if (toSaveVisa.IsChecked == true)
                    {
                        CreditCard myVisa = new CreditCard(((Customer)buyer).FirstName, ((Customer)buyer).LastName, ((Customer)buyer).CreditCard.CreditNumber, ((Customer)buyer).CreditCard.ExpirationDate);

                        if (buyer is ClubMember)
                        {
                            ClubMember oldClubMember = ((ClubMember)buyer);
                            ClubMember newClubMember = new ClubMember(oldClubMember);
                            newClubMember.CreditCard = myVisa;
                            parentWindow.cats[1].Edit(oldClubMember, newClubMember);
                        }
                        else
                        {
                            Customer oldCustomer = ((Customer)buyer);
                            Customer newCustomer = new Customer(oldCustomer);
                            newCustomer.CreditCard = myVisa;
                            parentWindow.cats[1].Edit(oldCustomer, newCustomer);
                        }
                    }
                }

                List<Purchase> receipt = purchasesList.Cast<Purchase>().ToList();
                Transaction newTran = new Transaction(0, Is_a_return.Purchase, receipt, myPaymentType);
                MessageBox.Show("Thank you for your purchase!");

                // commiting the transaction for real
                List<Object> bought = new List<Object>();
                foreach (Purchase p in purchasesList)
                {
                    List<Object> op = itsProductBL.FindByNumber(IntFields.productID, p.PrdID, p.PrdID);
                    if (op.Any())
                        ((Product)op.First()).Buy(p.Amount);
                }
                this.Close();
            }
        }
    

        private void DropProduct(object sender, DragEventArgs e)
        {
            if (canDrag) // only if this now paying mode
            {
                Buyable data = (Buyable)e.Data.GetData(typeof(Buyable));
                AddSingleToCart(data);
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
            else
                // reset all amounts of products in main datagrid
                if (currentList.Any())
                    foreach (Buyable b in currentList)
                    {
                        b.ZeroAmount();
                    }

            // update the main table of products to zero amounts
            //purchaseGrid.Items.Refresh();
        }

        private void AddSingleToCart(Buyable b)
        {
            Purchase toBuy = new Purchase(b.Prod.ProductID, b.Prod.Name, b.Prod.Price, b.Amount);
            if (b.Amount < 0)
                MessageBox.Show("Invalid product amount");
            else if (b.Amount == 0)
                MessageBox.Show("please choose an amount to buy");
            else if (b.Amount > b.Prod.StockCount)
                MessageBox.Show("We don't have so many " + b.Prod.Name + "s at E-MART!");
            else
            {
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

                // update the stock of the product
                b.LeftInStock = b.LeftInStock - b.Amount;
                

                // update the total price and total amount of products in the shopping cart
                String total = Convert.ToString(int.Parse(totalPrice1.Text) + (b.Prod.Price) * (b.Amount));
                totalPrice1.Text = total;
                totalPrice2.Text = total;
                totalAmount.Text = Convert.ToString(int.Parse(totalAmount.Text) + b.Amount);

                // zero the amount in main table
                b.ZeroAmount();
                // refresh the products datagrid
                ProductGrid.Items.Refresh();

                emptyCart.Visibility = Visibility.Collapsed;
                removals.Visibility = Visibility.Visible;
            }
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
            ProductGrid.Items.Refresh();

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
            ProductGrid.Items.Refresh();

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



        






    }
}
