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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class View : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private Object oldObj;
        private int currentCategory;
        private Grid[] allViews;

        // constructor
        public View(PL_GUI _parentWindow, Object _oldObj, int _currentCategory)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            oldObj = _oldObj;
            viewMode.DataContext = oldObj;
            currentCategory = _currentCategory;
            allViews = new Grid[8] { null, clubMemberView, customerView, departmentView, employeeView, productView, transactionView, userView };
            allViews[currentCategory].Visibility = Visibility.Visible;

            ///////////////////////////////////////////////////////////
            //////////////////////  permissions  //////////////////////
            ///////////////////////////////////////////////////////////
            // forbid edit/remove if there is no permission
            if (PL_GUI.allPermissions[parentWindow.rank][currentCategory] != 2)
                editRemoveButtons.Visibility = Visibility.Collapsed;
        }

        private void CallEdit(object sender, RoutedEventArgs e)
        {
            Window editForm = null;
            switch (currentCategory)
            {
                case 1:
                    editForm = new AddEditClubMember(parentWindow, false, oldObj);
                    break;
                case 2:
                    editForm = new AddEditCustomer(parentWindow, false, false, oldObj);
                    break;
                case 3:
                    editForm = new AddEditDepartment(parentWindow, false, oldObj);
                    break;
                case 4:
                    editForm = new AddEditEmployee(parentWindow, parentWindow.cats[3], parentWindow.cats[4], false, oldObj);
                    break;
                case 5:
                    editForm = new AddEditProduct(parentWindow, parentWindow.cats[3], false, oldObj);
                    break;
                case 6:
                    editForm = new AddEditTransaction(parentWindow, oldObj);
                    break;
                case 7:
                    editForm = new AddEditUser(parentWindow, oldObj);
                    break;
            }
            this.Close();
            editForm.ShowDialog();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (parentWindow.RemoveDataEntity(oldObj))
                this.Close();
        }


    }
}