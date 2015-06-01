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
using System.Text.RegularExpressions;


namespace PL
{
    public partial class PL_GUI : Window, IPL
    {
        // static attributes
        public static int[][] allPermissions = new int[4][];
        public static string[][] inputsInfo = new string[5][];

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

            ////////// regular expressions - used for user-input validation

            // ID - 9 digits
            inputsInfo[0] = new string[2] { "^[0-9]{9}$", "exactly 9 digits (0-9)" };

            // only letters and possibly more than one word
            // firstName, lastName, Product - name, Department - name
            inputsInfo[1] = new string[2] { "^[A-Za-z]{2,}(( )[A-Za-z]{2,})*$", "only letters (A-Z) or (a-z)" };

            // Number - unlimited digits
            inputsInfo[2] = new string[2] { "^[0-9]+$", "only digits (0-9)" };

            // at least 6 characters of letters and digits
            // username, password
            inputsInfo[3] = new string[2] { "^[A-Za-z0-9]{6,}$", "at least 6 characters.\nOnly letters (A-Z) or (a-z) and digits (0-9) are allowed" };

            // not empty
            inputsInfo[4] = new string[2] { "^.+$", "selected" };
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

        public static void EnableOrDisableForm(List<Control> lst, bool toEnable)
        {
            foreach (Control control in lst)
                control.IsEnabled = toEnable;
        }

        public static bool RegExp(String txt, String field, int check)
        {
            if (!Regex.IsMatch(txt, inputsInfo[check][0]))
            {
                MessageBox.Show(field + " must be " + inputsInfo[check][1]);
                return false;
            }
            else
                return true;
        }

        public static bool RangeSearchRegExp(String txtMin, String txtMax, String field, RadioButton isRangeSearch, int check)
        {
            bool flag = PL_GUI.RegExp(txtMin, field, check);
            if (flag)
                if (isRangeSearch.IsChecked == true)
                    flag = PL_GUI.RegExp(txtMax, "MAX " + field, check);
            return flag;
        }

        public static bool ComboboxValidate(ComboBox cmb, String field)
        {
            if (cmb.SelectedIndex == -1)
            {
                MessageBox.Show(field + " must be selected");
                return false;
            }
            else
                return true;
        }

        public static bool DoubleRadioValidate(RadioButton opt1, RadioButton opt2, String field)
        {
            if (opt1.IsChecked == false && opt2.IsChecked == false)
            {
                MessageBox.Show(field + " must be selected");
                return false;
            }
            else
                return true;
        }

        // attributes
        public IBL[] cats;
        private string[] catsNames;
        private DataGrid[] grids;
        private List<Object>[] data;

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
            data = new List<Object>[8];

            // default category is ClubMember = 1
            currentCategory = 1;

            // generate all lists of data entities and bind datagrids to lists
            for (int i = 1; i < 8; i++)
                DisplayData(new List<Object>(cats[i].GetAll()), i);
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

        public void showHideEmptyTitle()
        {
            if (cats[currentCategory].GetAll().Any())
            {
                categoryEmpty.Visibility = Visibility.Collapsed;
                grids[currentCategory].Visibility = Visibility.Visible;
            }
            else
            {
                grids[currentCategory].Visibility = Visibility.Collapsed;
                categoryEmpty.Text = "There are no " + catsNames[currentCategory] + "s";
                categoryEmpty.Visibility = Visibility.Visible;
            }
        }

        private void DisplayData(List<Object> results, int categoryNum)
        {
            showHideEmptyTitle();
            // generate a list of data
            data[categoryNum] = new List<Object>(results);
            // bind datagrid to list
            grids[categoryNum].DataContext = data[categoryNum];
        }

        private void CallReset(object sender, RoutedEventArgs e)
        {
            ResetRecords();
        }

        private void ResetRecords()
        {
            DisplayData(new List<Object>(cats[currentCategory].GetAll()), currentCategory);
        }

        private void CallView(object sender, RoutedEventArgs e)
        {
            Object selectedRow = grids[currentCategory].SelectedItem;
            if (selectedRow != null)
            {
                Window viewWindow = new View(this, selectedRow, currentCategory);
                viewWindow.ShowDialog();
            }
            else
                MessageBox.Show("You must choose a " + catsNames[currentCategory] + " first");
        }
    

        ////////////////////
        // Search methods //
        ////////////////////

        private void CallSearch(object sender, RoutedEventArgs e)
        {
            int catToSearch = int.Parse(((Button)sender).Uid);
            if (cats[catToSearch].GetAll().Any())
            {
                Window searchForm = null;
                switch (catToSearch)
                {
                    case 1:
                        searchForm = new QueryClubMember(this);
                        break;
                    case 2:
                        searchForm = new QueryCustomer(this);
                        break;
                    case 3:
                        searchForm = new QueryDepartment(this);
                        break;
                    case 4:
                        searchForm = new QueryEmployee(this);
                        break;
                    case 5:
                        searchForm = new QueryProduct(this);
                        break;
                    case 6:
                        searchForm = new QueryTransaction(this);
                        break;
                    case 7:
                        searchForm = new QueryUser(this);
                        break;
                }
                searchForm.ShowDialog();
            }
            else
                MessageBox.Show("There are no " + catsNames[catToSearch] + "s to serach for");
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
                currentCategory = categoryNum;
                // update the tab that is selected
                allTabs.SelectedIndex = categoryNum - 1;
            }
            return done;
        }

        /////////////////
        // Add methods //
        /////////////////

        public bool AreThereDepartments()
        {
            if (cats[3].GetAll().Any())
                return true;
            else
                return false;
        }

        private void CallAdd(object sender, RoutedEventArgs e)
        {
            int catToAdd = int.Parse(((Button)sender).Uid);
            Window addForm = null;
            switch (catToAdd)
            {
                case 1:
                    addForm = new AddEditClubMember(this, true, null);
                    break;
                case 2:
                    addForm = new AddEditCustomer(this, false, true, null);
                    break;
                case 3:
                    addForm = new AddEditDepartment(this, true, null);
                    break;
                case 4:
                    if (AreThereDepartments())
                        addForm = new AddEditEmployee(this, cats[3], cats[4], true, null);
                    else
                        MessageBox.Show("You cannot add a employee since there are no departments");
                    break;
                case 5:
                    if (AreThereDepartments())
                        addForm = new AddEditProduct(this, cats[3], true, null);
                    else
                        MessageBox.Show("You cannot add a product since there are no departments");
                    break;
            }
            addForm.ShowDialog();
        }

        public bool AddDataEntity(Object newObj, User newUser, int categoryNum)
        {
            bool done = true;
            try
            {
                cats[categoryNum].Add(newObj);
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
            {
                try
                {
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

                if (!done)
                    cats[categoryNum].Remove(newObj);
            }
                
            if (done)
            {
                MessageBox.Show(catsNames[categoryNum] + " was added successfully!\nPlease click OK to continue");
                currentCategory = categoryNum;
                ResetRecords();
                // update the tab that is selected
                allTabs.SelectedIndex = categoryNum - 1;
            }
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
                else if (type.Equals(typeof(Transaction)))
                    editForm = new AddEditTransaction(this, selectedRow);
                else
                    editForm = new AddEditUser(this, selectedRow);
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
            {
                MessageBox.Show(catsNames[categoryNum] + " was edited successfully!\nPlease click OK to continue");
                ResetRecords();
            }
            return done;
        }

        ////////////////////
        // Remove methods //
        ////////////////////

        public void CallRemove(object sender, RoutedEventArgs e)
        {
            Object selectedRow = grids[currentCategory].SelectedItem;
            if (selectedRow != null)
                RemoveDataEntity(selectedRow);
            else
                MessageBox.Show("You must choose a " + catsNames[currentCategory] + " first");
        }

        public bool RemoveDataEntity(Object _objToDelete)
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
                        {
                            MessageBox.Show(catsNames[currentCategory] + " was removed successfully!\nPlease click OK to continue");
                            ResetRecords();
                        }
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
            return done;
        }

        /////////////////////
        // General methods //
        /////////////////////

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CallPurchase(object sender, RoutedEventArgs e)
        {
            Window purchase = new PurchaseWindow(cats[5]);
            purchase.ShowDialog();
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
            ResetRecords();
            showHideEmptyTitle();
        }

        private void add_menu_Expanded(object sender, RoutedEventArgs e)
        {
            search_menu.IsExpanded = false;
        }

        private void search_menu_Expanded(object sender, RoutedEventArgs e)
        {
            add_menu.IsExpanded = false;
        }




    }
}
