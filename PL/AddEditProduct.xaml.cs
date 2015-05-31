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
    public partial class AddEditProduct : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private bool isAdd;
        private Object oldObj;

        // constructor
        public AddEditProduct(PL_GUI _parentWindow, IBL itsDepartmentBL, bool _isAdd, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            // Create departments list for choosing depID
            depID.ItemsSource = itsDepartmentBL.GetAll();

            isAdd = _isAdd;
            if (!isAdd)
            {
                AddEditTitle.Text = "Edit Product";
                AddEditButton.Content = "Edit Product";
                oldObj = _oldObj;
                ResetToDefault();
            }
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            productName.Text = ((Product)oldObj).Name;
            price.Text = Convert.ToString(((Product)oldObj).Price);
            stockcount.Text = Convert.ToString(((Product)oldObj).StockCount);
            depID.Text = Convert.ToString(((Product)oldObj).Location);

            if (((Product)oldObj).Type.Equals(PType.Electronics))
                productType.Text = "Electronics";
            else if (((Product)oldObj).Type.Equals(PType.Food))
                productType.Text = "Food";
            else
                productType.Text = "Clothes";
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { productName, productType, price, stockcount, depID };
            if (isAdd)
                PL_GUI.ClearForm(lst);
            else
                ResetToDefault();
        }

        // Add or edit
        private void AddOrEdit(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                PType myPType;
                var selectedItem = productType.SelectedItem as ComboBoxItem;
                if (selectedItem.Name.Equals("Clothes"))
                    myPType = PType.Clothes;
                else if (selectedItem.Name.Equals("Electronics"))
                    myPType = PType.Electronics;
                else
                    myPType = PType.Food;
                Product newObj = new Product(productName.Text, myPType, int.Parse(depID.Text), int.Parse(stockcount.Text), int.Parse(price.Text));

                //adding action
                if (isAdd)
                {
                    if (parentWindow.AddDataEntity(newObj, null, 5))
                        this.Close();
                }
                //editing action
                else
                {
                    if (parentWindow.EditDataEntity(oldObj, newObj, 5))
                        this.Close();
                }
            }
        }


        private bool IsValid()
        {
            bool flag = true;
            flag = PL_GUI.RegExp(productName.Text, "Product name", 1);
            if (flag)
                if (productType.SelectedIndex == -1)
                {
                    MessageBox.Show("Product type must be selected");
                    flag = false;
                }
            if (flag)
                flag = PL_GUI.RegExp(price.Text, "Price", 2);
            if (flag)
                flag = PL_GUI.RegExp(stockcount.Text, "Stock count", 2);
            if (flag)
                if (depID.SelectedIndex == -1)
                {
                    MessageBox.Show("Department ID must be selected");
                    flag = false;
                }
            return flag;
        }



    }
}
