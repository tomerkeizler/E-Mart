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
    public partial class PL_GUI : Window, IPL
    {
        // static attributes
        public static int[][] allPermissions = new int[4][];
        public static String[] usersTypes = new String[4];

        // static constructor
        static PL_GUI()
        {
            /* Permissions array
             * 1st dimension: Admin, Manager, Worker, Customer
             * 2nd dimension: nothing, ClubMember, Customer, Department, Employee, Product, Transaction, User, self-viewing
             * 0=no permission || 1=view only || 2=full permission */
            allPermissions[0] = new int[9] { 0, 2, 2, 2, 2, 2, 2, 2, 2 }; // Admin
            allPermissions[1] = new int[9] { 0, 2, 2, 0, 2, 2, 2, 0, 1 }; // Manager
            allPermissions[2] = new int[9] { 0, 1, 1, 0, 0, 0, 1, 0, 1 }; // Worker
            allPermissions[3] = new int[9] { 0, 0, 0, 0, 0, 1, 1, 0, 1 }; // Customer
            // User types  array
            usersTypes = new String[4] { "Administrator", "Manager", "Worker", "Customer" };
        }

        // static methods
        public static void ClearForm(List<Control> lst)
        {
            foreach (Control control in lst)
            {
                if (control is TextBox)
                {
                    TextBox txtbox = (TextBox)control;
                    txtbox.Clear();
                }
                else if (control is PasswordBox)
                {
                    PasswordBox passbox = (PasswordBox)control;
                    passbox.Clear();
                }
                else if (control is CheckBox)
                {
                    CheckBox chkbox = (CheckBox)control;
                    chkbox.IsChecked = false;
                }
                else if (control is RadioButton)
                {
                    RadioButton rdbtn = (RadioButton)control;
                    rdbtn.IsChecked = false;
                }
                else if (control is DatePicker)
                {
                    DatePicker datePckr = (DatePicker)control;
                    datePckr.ClearValue(DatePicker.SelectedDateProperty);
                }
                else if (control is ComboBox)
                {
                    ComboBox cmbbox = (ComboBox)control;
                    cmbbox.ClearValue(ComboBox.SelectedItemProperty);
                }
            }
        }

        // attributes
        private IBL[] cats;
        private string[] catsNames;
        private DataGrid[] grids;
        private ObservableCollection<Object>[] data;
        
        public User user;
        public Rank rank;
        public bool[] viewPermissions;
        public bool[] fullPermissions;
        private int currentCategory;

        // constructor
        public PL_GUI(IBL itsClubMemberBL, IBL itsCustomerBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            InitializeComponent();
            cats = new IBL[8] { null, itsClubMemberBL, itsCustomerBL, itsDepartmentBL, itsEmployeeBL, itsProductBL, itsTransactionBL, itsUserBL };
            catsNames = new string[8] { "", "Club member", "Customer", "Department", "Employee", "Product", "Transaction", "User" };
            grids = new DataGrid[8] { null, clubMemberGrid, CustomerGrid, DepartmentGrid, EmployeeGrid, ProductGrid, TransactionGrid, UserGrid };
            data = new ObservableCollection<Object>[8];
            // generate all lists of data entities
            for (int i = 1; i < 8; i++)
                data[i] = new ObservableCollection<Object>(cats[i].GetAll());
            // bind datagrids to lists
            for (int i = 1; i < 8; i++)
                grids[i].DataContext = data[i];
            // default category is ClubMember = 1
            currentCategory = 1;
        }

        // Main method
        public void Run()
        {
            InitializeComponent();

            // Login
            // the main window is being opened by the Login window when logging in succeeds
            Window login = new Login(cats[7], this);
            login.ShowDialog();

            // resetting selling quantities
            ((Product_BL)cats[5]).GenerateTopSeller();

            /////////////
            //viewPermissions[1] = false;
            /////////////

   

   

        }



        /////////////////////
        // Display methods //
        /////////////////////

        private void DisplayData(List<Object> results, int categoryNum)
        {
            // generate a list of data 
            data[categoryNum] = new ObservableCollection<Object>(results);
            // bind datagrid to list
            grids[categoryNum].DataContext = data[categoryNum];
        }

        private void ResetRecords(object sender, RoutedEventArgs e)
        {
            // generate a list of data 
            data[currentCategory] = new ObservableCollection<Object>(cats[currentCategory].GetAll());
            // bind datagrid to list
            grids[currentCategory].DataContext = data[currentCategory];
        }


        ////////////////////
        // Search methods //
        ////////////////////
        private void CallSearchClubMember(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryClubMember(this);
            addForm.ShowDialog();
        }

        private void CallSearchCustomer(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryCustomer(this);
            addForm.ShowDialog();
        }

        private void CallSearchDepartment(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryDepartment(this);
            addForm.ShowDialog();
        }

        private void CallSearchEmployee(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryEmployee(this);
            addForm.ShowDialog();
        }

        private void CallSearchProduct(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryProduct(this);
            addForm.ShowDialog();
        }

        private void CallSearchTransaction(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryTransaction(this);
            addForm.ShowDialog();
        }

        private void CallSearchUser(object sender, RoutedEventArgs e)
        {
            Window addForm = new QueryUser(this);
            addForm.ShowDialog();
        }

        public bool SearchDataEntity(Object field, Object value1, Object value2, int categoryNum)
        {
            bool done = true;
            List<Object> results = null;
            try
            {
                if (field is StringFields)
                    results = cats[categoryNum].FindByName((String)value1, (StringFields)field);
                else if (field is IntFields)
                    results = cats[categoryNum].FindByNumber((IntFields)field, (int)value1, (int)value2);
                else
                    results = cats[categoryNum].FindByType((ValueType)value1);
            }
            catch (ArgumentNullException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (ArgumentException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (InvalidDataException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (System.Data.DataException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }

            if (done) // Search was successful
            {
                MessageBox.Show("Search of " + catsNames[categoryNum] + " was done successfully!\nPlease click OK to view the results");
                DisplayData(results, categoryNum);
            }
            return done;
        }


        /////////////////
        // Add methods //
        /////////////////
        private void CallAddClubMember(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddEditClubMember(this, true, null);
            addForm.ShowDialog();
        }

        private void CallAddCustomer(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddEditCustomer(this, false, true, null);
            addForm.ShowDialog();
        }

        private void CallAddDepartment(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddEditDepartment(this, true, null);
            addForm.ShowDialog();
        }

        private void CallAddEmployee(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddEditEmployee(this, cats[3], cats[4], true, null);
            addForm.ShowDialog();
        }

        private void CallAddProduct(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddEditProduct(this, cats[3], true, null);
            addForm.ShowDialog();
        }

        public bool AddDataEntity(Object newObj, User newUser, int categoryNum)
        {
            bool done = true;
            try
            {
                cats[categoryNum].Add(newObj);
                if (newUser != null)
                    cats[7].Add(newUser);
            }
            catch (System.Data.DataException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (ArgumentNullException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            if (done)
                MessageBox.Show(catsNames[categoryNum] + " was added successfully!\nPlease click OK to continue");
            return done;
        }


        //////////////////
        // Edit methods //
        //////////////////
        private void CallEdit(object sender, RoutedEventArgs e)
        {
            Object selectedRow = grids[currentCategory].SelectedItem;
            if (selectedRow != null)
            {
                Type type = selectedRow.GetType();
                Window editForm = null;
                if (type.Equals(typeof(ClubMember)))
                    editForm = new AddEditClubMember(this, false, selectedRow);
                else if (type.Equals(typeof(Customer)))
                    editForm = new AddEditCustomer(this, false, false, selectedRow);
                else if (type.Equals(typeof(Department)))
                    editForm = new AddEditDepartment(this, false, selectedRow);
                else if (type.Equals(typeof(Employee)))
                    editForm = new AddEditEmployee(this, cats[3], cats[4], false, selectedRow);
                else if (type.Equals(typeof(Product)))
                    editForm = new AddEditProduct(this, cats[3], false, selectedRow);
                /*
            else if (type.Equals(typeof(Transaction)))
                editForm = new AddEditTransaction(this, false, selectedRow);
            else (type.Equals(typeof(User)))
                editForm = new AddEditUser(this, false, selectedRow);
                 * */
                editForm.ShowDialog();
            }
            else
                MessageBox.Show("You must choose a " + catsNames[currentCategory] + " first");
        }

        public bool EditDataEntity(Object oldObj, Object newObj, int categoryNum)
        {
            bool done = true;
            try
            {
                cats[categoryNum].Edit(oldObj, newObj);
                ///////////////////////////////////////
                // need to check if this is a ClubMember/Customer/Employee and edit its user also
                ///////////////////////////////////////
            }
            catch (ArgumentNullException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (ArgumentException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (NullReferenceException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (IndexOutOfRangeException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (System.InvalidCastException e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (System.Data.DataException e)
            {   
                done = false;
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                done = false;
                MessageBox.Show(e.Message);
            }
            if (done)
                MessageBox.Show(catsNames[categoryNum] + " was edited successfully!\nPlease click OK to continue");
            return done;
        }



        ////////////////////
        // Remove methods //
        ////////////////////
        public void CallRemoveFromDatagrid(object sender, RoutedEventArgs e)
        {
            Object selectedRow = grids[currentCategory].SelectedItem;
            if (selectedRow != null)
                RemoveDataEntity(selectedRow);
            else
                MessageBox.Show("You must choose a " + catsNames[currentCategory] + " first");
        }

        public void CallRemoveFromWindow(object sender, RoutedEventArgs e)
        {
            //RemoveDataEntity();
        }

        public void RemoveDataEntity(Object _objToDelete)
        {
            bool done = true;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove the selected " + catsNames[currentCategory], "Remove" + catsNames[currentCategory], MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        try
                        {
                            cats[currentCategory].Remove(_objToDelete);
                            ///////////////////////////////////////
                            // need to check if this is a ClubMember/Customer/Employee and remove its user also
                            ///////////////////////////////////////
                        }
                        catch (ArgumentNullException error)
                        {
                            done = false;
                            MessageBox.Show(error.Message);
                        }
                        catch (NullReferenceException error)
                        {
                            done = false;
                            MessageBox.Show(error.Message);
                        }
                        catch (IndexOutOfRangeException error)
                        {
                            done = false;
                            MessageBox.Show(error.Message);
                        }
                        catch (Exception error)
                        {
                            done = false;
                            MessageBox.Show(error.Message);
                        }
                        if (done)
                            MessageBox.Show(catsNames[currentCategory] + " was removed successfully!\nPlease click OK to continue");
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }





        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            Window changePass = new ChangePassword(this, user);
            changePass.ShowDialog();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;
            switch (tabItem)
            {
                case "Club Members":
                    currentCategory = 1;
                    break;
                case "Customers":
                    currentCategory = 2;
                    break;
                case "Departments":
                    currentCategory = 3;
                    break;
                case "Employees":
                    currentCategory = 4;
                    break;
                case "Products":
                    currentCategory = 5;
                    break;
                case "Transactions":
                    currentCategory = 6;
                    break;
                case "Users":
                    currentCategory = 7;
                    break;
                default:
                    return;
            }
        }


    }
}
