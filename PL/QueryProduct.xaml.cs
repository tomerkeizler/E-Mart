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
    public partial class QueryProduct : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryProduct(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { name, pType, specificDepID, rangeDepID, fromDepID, toDepID, specificPrice, rangePrice, fromPrice, toPrice, specificPrdID, rangePrdID, fromPrdID, toPrdID, pStatus, specificStockCount, rangeStockCount, fromStockCount, toStockCount };
            PL_GUI.ClearForm(lst);
        }

        private void SearchByName(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(name.Text, "Product name", 1))
                if (parentWindow.SearchDataEntity(StringFields.name, name.Text, null, 5))
                    this.Close();
        }

        private void SearchByPType(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.ComboboxValidate(pType, "Product type"))
            {
                PType type;
                if (pType.Text.Equals("Electronics"))
                    type = PType.Electronics;
                else if (pType.Text.Equals("Food"))
                    type = PType.Food;
                else
                    type = PType.Clothes;
                if (parentWindow.SearchDataEntity(TypeFields.type, type, null, 5))
                    this.Close();
            }
        }


        private void SearchByDepID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromDepID.Text, toDepID.Text, "Department ID", rangeDepID, 2))
            {
                int min = int.Parse(fromDepID.Text);
                String max = toDepID.Text;
                if (parentWindow.SearchDataEntity(IntFields.location, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 5))
                    this.Close();
            }
        }

        private void SearchByProductID(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromPrdID.Text, toPrdID.Text, "Product ID", rangePrdID, 2))
            {
                int min = int.Parse(fromPrdID.Text);
                String max = toPrdID.Text;
                if (parentWindow.SearchDataEntity(IntFields.productID, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 5))
                    this.Close();
            }
        }

        private void SearchByPrice(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromPrice.Text, toPrice.Text, "Price", rangePrice, 2))
            {
                int min = int.Parse(fromPrice.Text);
                String max = toPrice.Text;
                if (parentWindow.SearchDataEntity(IntFields.price, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 5))
                    this.Close();
            }
        }

        private void SearchByPStatus(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.ComboboxValidate(pStatus, "Product status"))
            {
                PStatus sta;
                if (pStatus.Text.Equals("in stock"))
                    sta = PStatus.InStock;
                else if (pStatus.Text.Equals("low quantity"))
                    sta = PStatus.LowQuantity;
                else
                    sta = PStatus.Empty;
                if (parentWindow.SearchDataEntity(TypeFields.inStock, sta, null, 5))
                    this.Close();
            }
        }

        private void SearchByStockCount(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RangeSearchRegExp(fromStockCount.Text, toStockCount.Text, "Stock count", rangeStockCount, 2))
            {
                int min = int.Parse(fromStockCount.Text);
                String max = toStockCount.Text;
                if (parentWindow.SearchDataEntity(IntFields.stockCount, min, (max.Equals(String.Empty)) ? (min) : (int.Parse(max)), 5))
                    this.Close();
            }
        }


    }
}
