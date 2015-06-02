﻿using System;
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
        private IBL itsProductBL;
        private Dictionary<ComboBox, int> currentTypes;
        private ObservableCollection<Product> currentList;
        private ObservableCollection<Product> purchasesList;

        // constructor
        public PurchaseWindow(IBL _itsProductBL)
        {
            InitializeComponent();
            itsProductBL = _itsProductBL;

            // initializing the dictionary with value 3="none" for each combobox
            currentTypes = new Dictionary<ComboBox, int>(3);
            currentTypes.Add(pType1, 3);
            currentTypes.Add(pType2, 3);
            currentTypes.Add(pType3, 3);

            // bind the datagrid to currentList
            currentList = new ObservableCollection<Product>();
            ProductGrid.DataContext = currentList;

            purchasesList = new ObservableCollection<Product>();
            purchaseGrid.DataContext = purchasesList;
        }


        private void UpdateProducts(object sender, SelectionChangedEventArgs e)
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
            // to call UpdateProducts() or its being called automatically?
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


    }
}
