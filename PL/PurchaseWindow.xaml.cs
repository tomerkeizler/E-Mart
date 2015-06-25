﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading;
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
        private bool canDrag;
        private Dictionary<int, int> currentCart;

        private Dictionary<ComboBox, int> currentTypes;
        private bool areThereAnyProducts;
        private ObservableCollection<Buyable> currentList;
        private ObservableCollection<Purchase> purchasesList;

        // constructor
        public PurchaseWindow(PL_GUI _parentWindow, Object _buyer)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            buyer = _buyer;
            canDrag = true;
            currentCart = new Dictionary<int, int>();

            // initializing the dictionary with value 3="none" for each combobox
            currentTypes = new Dictionary<ComboBox, int>(3);
            currentTypes.Add(pType1, 3);
            currentTypes.Add(pType2, 3);
            currentTypes.Add(pType3, 3);

            // check if are there any products
            areThereAnyProducts = parentWindow.cats[5].GetAll().Any();
            if (!areThereAnyProducts)
            {
                ProductGrid.Visibility = Visibility.Collapsed;
                purchaseGrid.Visibility = Visibility.Collapsed;
                previewBox.Visibility = Visibility.Collapsed;
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
                {
                    myName.Text = "Name: " + ((Employee)buyer).FirstName + " " + ((Employee)buyer).LastName;
                    toSaveVisa.Visibility = Visibility.Collapsed;
                }
            }
            else
                toSaveVisa.Visibility = Visibility.Collapsed;
        }

        private void UpdateProducts(object sender, SelectionChangedEventArgs e)
        {
            //////////////////////////
            ////// progress bar //////
            //////////////////////////
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
            //////////////////////////

            if (areThereAnyProducts)
            {
                bool areAllNull = true;
                foreach (var comboBox in currentTypes.Keys)
                    if (comboBox.SelectedItem != null && comboBox.SelectedIndex != 3)
                        areAllNull = false;

                ComboBox selectedComboBox = ((ComboBox)sender);
                int TypesInStore = Enum.GetNames(typeof(PType)).Length;

                if (areAllNull)
                {
                    currentList.Clear();
                    if (selectedComboBox.SelectedIndex != -1)
                        currentTypes[selectedComboBox] = TypesInStore;
                }
                else
                {
                    int previousChoice = currentTypes[selectedComboBox];
                    if (!previousChoice.Equals(TypesInStore))
                        if (!isDuplicate(selectedComboBox, previousChoice))
                            ((Product_BL)parentWindow.cats[5]).FilterProducts(currentList, typeIndex[previousChoice], false);

                    int newChoice = selectedComboBox.SelectedIndex;
                    if (newChoice != -1)
                    {
                        if (!newChoice.Equals(TypesInStore))
                            if (!isDuplicate(selectedComboBox, newChoice))
                                ((Product_BL)parentWindow.cats[5]).FilterProducts(currentList, typeIndex[newChoice], true);
                        currentTypes[selectedComboBox] = newChoice;
                    }

                    // update products and amounts from a saved dictionary
                    if (currentCart.Any())
                        foreach (Buyable b in currentList)
                            foreach (KeyValuePair<int, int> inCart in currentCart)
                                if (b.Prod.ProductID == inCart.Key)
                                    b.LeftInStock = inCart.Value;
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

            String[] allTypes = Enum.GetNames(typeof(PType));
            for (int i = 0; i < allTypes.Length; i++)
                currentTypes[currentTypes.ElementAt(i).Key] = allTypes.Length;
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

        private bool isValid()
        {
            bool flag = true;
            if (creditCard.Visibility == Visibility.Visible)
            {
                flag = PL_GUI.RegExp(cardNumber.Text, "credit card number", 5);
                if (flag)
                    if (expDate.Text.Equals(""))
                    {
                        MessageBox.Show("Expiration date must be selected");
                        flag = false;
                    }
            }
            return flag;
        }

        private void CompletePurchase(object sender, RoutedEventArgs e)
        {
            if (!purchasesList.Any())
                MessageBox.Show("There are no products in your shopping cart!");
            else
            {
                if (isValid()) // input validation with regular expressions
                {
                    MessageBox.Show("Thank you for your purchase!");

                    // check the requested payment type
                    PaymentMethod myPaymentType;
                    if (paymentMethod.SelectedIndex == 0)
                        myPaymentType = PaymentMethod.Cash;
                    else if (paymentMethod.SelectedIndex == 1)
                        myPaymentType = PaymentMethod.Check;
                    else
                    {
                        myPaymentType = PaymentMethod.Visa;

                        // save the credit card details if the buyer asked so
                        if (toSaveVisa.IsChecked == true)
                        {
                            CreditCard myVisa = new CreditCard(((Customer)buyer).FirstName, ((Customer)buyer).LastName, int.Parse(cardNumber.Text), expDate.SelectedDate.Value);

                            if (buyer is ClubMember)
                            {
                                ClubMember oldClubMember = new ClubMember((ClubMember)buyer);
                                ((Customer)buyer).CreditCard = myVisa;
                                parentWindow.cats[1].Edit(oldClubMember, buyer);
                            }
                            else if (buyer is Customer)
                            {
                                Customer oldCustomer = new Customer((Customer)buyer);
                                ((Customer)buyer).CreditCard = myVisa;
                                parentWindow.cats[2].Edit(oldCustomer, buyer);
                            }
                        }
                    }

                    // create the transcation
                    Transaction newTran = new Transaction(0, Is_a_return.Purchase, new List<Purchase>(), myPaymentType);

                    // commiting the transaction for real
                    foreach (Purchase p in purchasesList)
                    {
                        newTran.Receipt.Add(p);
                        List<Object> bought = parentWindow.cats[5].FindByNumber(IntFields.productID, p.PrdID, p.PrdID);
                        if (bought.Any())
                        {
                            Product oldProd = ((Product)bought.First());
                            Product newProd = new Product(oldProd);
                            newProd.Buy(p.Amount);
                            parentWindow.cats[5].Edit(oldProd, newProd);
                        }
                    }

                    // add this transaction to the table of all transactions
                    parentWindow.cats[6].Add(newTran);

                    // add this transaction to the tranHistory of the buyer (if he is a customer/clubmember)
                    if (buyer is Customer)
                    {
                        if (buyer is ClubMember)
                        {
                            ClubMember oldClub = new ClubMember((ClubMember)buyer);
                            ((Customer)buyer).TranHistory.Add(newTran);
                            parentWindow.cats[1].Edit(oldClub, buyer);
                        }
                        else
                        {
                            Customer oldCus = new Customer((Customer)buyer);
                            ((Customer)buyer).TranHistory.Add(newTran);
                            parentWindow.cats[2].Edit(oldCus, buyer);
                        }
                    }

                    // resetting selling counters
                    ((Product_BL)parentWindow.cats[5]).GenerateTopSeller();

                    // resetting product tab in parent window
                    if (parentWindow.currentCategory == 5)
                        parentWindow.ResetRecords();

                    this.Close();
                }
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
        }

        private void AddSingleToCart(Buyable b)
        {
            Purchase toBuy = new Purchase(b.Prod.ProductID, b.Prod.Name, b.Prod.Price, b.Amount);
            if (b.Amount < 0)
                MessageBox.Show("Invalid product amount");
            else if (b.Amount == 0)
                MessageBox.Show("please choose an amount to buy");
            else if (b.Amount > b.LeftInStock)
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
                purchaseGrid.CancelEdit();
                purchaseGrid.Items.Refresh();

                // update the stock of the product
                b.LeftInStock = b.LeftInStock - b.Amount;


                // update the total price and total amount of products in the shopping cart
                String total = Convert.ToString(int.Parse(totalPrice1.Text) + (b.Prod.Price) * (b.Amount));
                totalPrice1.Text = total;
                totalPrice2.Text = total;
                totalAmount.Text = Convert.ToString(int.Parse(totalAmount.Text) + b.Amount);

                emptyCart.Visibility = Visibility.Collapsed;
                removals.Visibility = Visibility.Visible;

                // update product and amount selected in a dictionary
                bool flag = false;
                if (currentCart.Any())
                    foreach (int inCart in currentCart.Keys)
                        if (b.Prod.ProductID == inCart && !flag)
                            flag = true;
                if (flag)
                    currentCart[b.Prod.ProductID] = b.LeftInStock;
                else
                    currentCart.Add(b.Prod.ProductID, b.LeftInStock);

                // zero the amount in main table
                b.ZeroAmount();
                // refresh the products datagrid
                ProductGrid.CancelEdit();
                ProductGrid.Items.Refresh();
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
            ProductGrid.CancelEdit();
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


            // update product and amount selected in a dictionary
            if (currentCart.Any())
                currentCart.Remove(toRemove.PrdID);
        }

        private void RemoveManyFromCart(object sender, RoutedEventArgs e)
        {
            if (!purchasesList.Any())
                MessageBox.Show("The shopping cart is empty");
            else
            {
                // update the stock of the products
                foreach (Purchase p in purchasesList)
                    if (currentList.Any())
                        foreach (Buyable b in currentList)
                            if (p.PrdID == b.Prod.ProductID)
                                b.LeftInStock = b.LeftInStock + p.Amount;

                ProductGrid.CancelEdit();
                ProductGrid.Items.Refresh();

                purchasesList.Clear();

                // zero the total price and total amount of products in the shopping cart
                totalPrice1.Text = Convert.ToString(0);
                totalPrice2.Text = Convert.ToString(0);
                totalAmount.Text = Convert.ToString(0);

                currentCart.Clear();
            }
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


        //////////////////////////////////
        ////// progress bar methods //////
        //////////////////////////////////
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }
        //////////////////////////////////


    }
}
