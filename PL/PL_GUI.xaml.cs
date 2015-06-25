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
using System.ComponentModel;
using System.Xml;

namespace PL
{
    public partial class PL_GUI : Window, IPL
    {
        // static attributes
        public static int[][] allPermissions = new int[5][];
        public static string[][] inputsInfo = new string[6][];
        public static Dictionary<string, string> stores_list = new Dictionary<string, string>();

        // static constructor
        static PL_GUI()
        {
            /* Permissions array
             * 1st dimension: Admin, Manager, Worker, Customer
             * 2nd dimension: nothing, ClubMember, Customer, Department, Employee, Product, Transaction, User, self-viewing
             * 0=no permission || 1=view only || 2=full permission */
            allPermissions[0] = new int[9] { 0, 2, 2, 2, 2, 2, 2, 2, 2 }; // Admin
            allPermissions[1] = new int[9] { 0, 2, 2, 0, 2, 2, 2, 0, 1 }; // Manager
            allPermissions[2] = new int[9] { 0, 1, 1, 0, 0, 1, 1, 0, 1 }; // Worker
            allPermissions[3] = new int[9] { 0, 0, 0, 0, 0, 1, 1, 0, 1 }; // Customer
            allPermissions[4] = new int[9] { 0, 0, 0, 0, 0, 1, 0, 0, 0 }; // guest

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

            // credit card number - 16 digits
            inputsInfo[5] = new string[2] { "^[0-9]{8}$", "exactly 8 digits (0-9)" };


            ////////// stores list
            stores_list.Add("Tel Aviv", "Derech HaShalom 4 Tel Aviv Israel");
            stores_list.Add("Eilat", "Derekh Yotam 55 Eilat Israel");
            stores_list.Add("Kfar Saba", "Ben Yehuda 10 Kfar Saba Israel");
            stores_list.Add("Barcelona", "La Rambla 20 Barcelona Spain");
            stores_list.Add("Rome", "Via Torino 44 Rome, Metropolitan City of Rome, Italy");
            stores_list.Add("Warsaw", "Stefana Batorego 31B, Warsaw, Poland");
            stores_list.Add("Berlin", "Stralauer Straße 44, Berlin, Germany");
            stores_list.Add("Miami", "W Flagler 55, Miami, FL, United States");
            stores_list.Add("New York", "West 45th 4, New York, NY, United States");
            stores_list.Add("Washington", "New York Avenue Northwest 444, Washington, DC, United States");
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
        private DataGrid[] grids;
        private List<Object>[] data;

        public User user;
        public int rank;
        public int currentCategory;
        private bool isEndProccess;

        // constructor
        public PL_GUI(IBL itsClubMemberBL, IBL itsCustomerBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            InitializeComponent();
            cats = new IBL[8] { null, itsClubMemberBL, itsCustomerBL, itsDepartmentBL, itsEmployeeBL, itsProductBL, itsTransactionBL, itsUserBL };
            grids = new DataGrid[8] { null, clubMemberGrid, CustomerGrid, DepartmentGrid, EmployeeGrid, ProductGrid, TransactionGrid, UserGrid };
            data = new List<Object>[8];

            // default user and rank
            user = null;
            rank = 4;

            // default category is ClubMember = 1
            currentCategory = 1;
            isEndProccess = true;

            // generate all lists of data entities and bind datagrids to lists
            for (int i = 1; i < 8; i++)
                DisplayData(new List<Object>(cats[i].GetAll()), i);

            // generate the list of stores
            foreach (string city in stores_list.Keys)
                stores_cmb.Items.Add(city);
        }

        // Main method
        public void Run()
        {
            // Login - the main window is being opened by the Login window when logging in succeeds
            Window login = new Login(cats[7], this);
            login.ShowDialog();

            // resetting selling counters
            ((Product_BL)cats[5]).GenerateTopSeller();

            Permissions();
        }

        public void Permissions()
        {
            ///////////////////////////////////////////////////////////
            //////////////////////  permissions  //////////////////////
            ///////////////////////////////////////////////////////////

            // for anonymous guests and customers
            if (rank == 3 || rank == 4)
            {
                // anonymous guests and customers - both start with watching products
                currentCategory = 5;
                (allTabs as TabControl).SelectedIndex = currentCategory - 1;
                ResetRecords();

                // for anonymous guests only
                if (rank == 4)
                {
                    account_menu.Visibility = Visibility.Collapsed; // anonymous guests have no account
                    search_menu.IsExpanded = true; // anonymous guests can anyway watch only products so expand it..
                }
            }

            // hide addition menu and edit/remove buttons from workers, customers, guests
            if (rank > 1)
            {
                add_menu.Visibility = Visibility.Collapsed;
                editButton.Visibility = Visibility.Collapsed;
                removeButton.Visibility = Visibility.Collapsed;
            }

            List<Button> addButtons = new List<Button>() { null, addClubmemberButton, addCustomerButton, addDepartmentButton, addEmployeeButton, addProductButton };
            List<Button> searchButtons = new List<Button>() { null, searchClubmemberButton, searchCustomerButton, searchDepartmentButton, searchEmployeeButton, searchProductButton, searchTransactionButton, searchUserButton };

            for (int i = 1; i < 8; i++)
                if (allPermissions[rank][i] < 2)
                {
                    // permissions - add buttons
                    if (i < 6) // there are no windows for adding transactions and users
                        addButtons[i].Visibility = Visibility.Collapsed;

                    // permissions - view records
                    if (allPermissions[rank][i] == 0)
                    {
                        // permissions - tabs
                        (allTabs.Items.GetItemAt(i - 1) as TabItem).Visibility = Visibility.Collapsed;
                        //permissions - search buttons
                        searchButtons[i].Visibility = Visibility.Collapsed;
                    }
                }
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
                actionButtons.Visibility = Visibility.Visible;
                viewButton.Visibility = Visibility.Visible;
                resetButton.Visibility = Visibility.Visible;
            }
            else
            {
                categoryEmpty.Text = "There are no " + cats[currentCategory].GetEntityName() + "s";
                categoryEmpty.Visibility = Visibility.Visible;
                grids[currentCategory].Visibility = Visibility.Collapsed;
                actionButtons.Visibility = Visibility.Collapsed;
                viewButton.Visibility = Visibility.Collapsed;
                resetButton.Visibility = Visibility.Collapsed;
            }

            TabItem tab = (allTabs.SelectedItem as TabItem);
            if (tab != null && (tab.Header as string).Equals("Stores"))
                if (!string.IsNullOrEmpty(stores_cmb.Text))
                    categoryEmpty.Visibility = Visibility.Collapsed;
        }

        private void DisplayData(List<Object> results, int categoryNum)
        {
            // a manager can manage only his workers
            if (rank == 1 && currentCategory == 4)
            {
                List<Employee> onlyMyWorkers = ((Employee_BL)cats[4]).GetAllWorkers(results.Cast<Employee>().ToList(), ((Employee)(user.Person)).Id);
                results = onlyMyWorkers.Cast<Object>().ToList();
            }
            // a Customer/ClubMember can only see his personal transactions
            else if (rank == 3 && currentCategory == 6)
                results = ((Customer)user.Person).TranHistory.Cast<Object>().ToList();

            // check if there are any records
            showHideEmptyTitle();

            // generate a list of data
            data[categoryNum] = results;

            // bind datagrid to list
            grids[categoryNum].DataContext = data[categoryNum];
        }

        private void CallReset(object sender, RoutedEventArgs e)
        {
            ResetRecords();
        }

        public void ResetRecords()
        {
            DisplayData(new List<Object>(cats[currentCategory].GetAll()), currentCategory);
        }

        private void CallView(object sender, RoutedEventArgs e)
        {
            Object selectedRow = grids[currentCategory].SelectedItem;
            if (selectedRow != null)
            {
                Window viewWindow = new View(this, selectedRow, currentCategory, false);
                viewWindow.ShowDialog();
            }
            else
                MessageBox.Show("You must choose a " + cats[currentCategory].GetEntityName() + " first");
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
                MessageBox.Show("There are no " + cats[catToSearch].GetEntityName() + "s to serach for");
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
                MessageBox.Show("Search of " + cats[categoryNum].GetEntityName() + " was done successfully!\nPlease click OK to view the results");
                // update the tab that is selected
                allTabs.SelectedIndex = categoryNum - 1; 
                DisplayData(results, categoryNum);
                currentCategory = categoryNum;
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
                    addForm = new AddEditCustomer(null, this, false, true, null);
                    break;
                case 3:
                    addForm = new AddEditDepartment(this, true, null);
                    break;
                case 4:
                    if (AreThereDepartments())
                        addForm = new AddEditEmployee(this, cats[3], cats[4], true, null);
                    else
                        MessageBox.Show("You cannot add an employee since there are no departments");
                    break;
                case 5:
                    if (AreThereDepartments())
                        addForm = new AddEditProduct(this, cats[3], true, null);
                    else
                        MessageBox.Show("You cannot add a product since there are no departments");
                    break;
            }
            if (addForm != null)
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
                if (categoryNum != 2) // don't display a message for customers because it's displayed from AddEditCustomer window
                    MessageBox.Show(cats[categoryNum].GetEntityName() + " was added successfully!\nPlease click OK to continue");
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
                    editForm = new AddEditCustomer(null, this, false, false, selectedRow);
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
                MessageBox.Show("You must choose a " + cats[currentCategory].GetEntityName() + " first");
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
                MessageBox.Show(cats[categoryNum].GetEntityName() + " was edited successfully!\nPlease click OK to continue");
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
            if (user.Person.Equals(selectedRow))
                MessageBox.Show("You cannot delete yourself!");
            else
            {
                if (selectedRow != null)
                    RemoveDataEntity(selectedRow);
                else
                    MessageBox.Show("You must choose a " + cats[currentCategory].GetEntityName() + " first");
            }
        }

        public bool RemoveDataEntity(Object _objToDelete)
        {
            bool done = true;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove the selected " + cats[currentCategory].GetEntityName() + "?", "Remove" + cats[currentCategory].GetEntityName(), MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        try
                        {
                            cats[currentCategory].Remove(_objToDelete);
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
                            MessageBox.Show(cats[currentCategory].GetEntityName() + " was removed successfully!\nPlease click OK to continue");
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


        private void Logout(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Log out", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    isEndProccess = false;
                    this.Close();
                    IPL myPL = new PL_GUI(cats[1], cats[2], cats[3], cats[4], cats[5], cats[6], cats[7]);
                    myPL.Run();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CallPurchase(object sender, RoutedEventArgs e)
        {
            Object buyer;
            if (user == null)
                buyer = null;
            else
                buyer = user.Person;

            Window purchase = new PurchaseWindow(this, buyer);
            purchase.ShowDialog();
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            if (!isDefaultAdmin())
            {
                Window changePass = new ChangePassword(this, user);
                changePass.ShowDialog();
            }
        }

        private bool isDefaultAdmin()
        {
            if (user.UserName.Equals("administrator") && user.Password.Equals("password"))
            {
                MessageBox.Show("No can do...\nYou are a default administrator!");
                return true;
            }
            else
                return false;
        }

        private void ViewProfile(object sender, RoutedEventArgs e)
        {
            if (!isDefaultAdmin())
            {
                Object myPerson = user.Person;
                int myCategory;
                if (myPerson.GetType().Equals(typeof(ClubMember)))
                    myCategory = 1;
                else if (myPerson.GetType().Equals(typeof(Customer)))
                    myCategory = 2;
                else // so this is an Employee
                    myCategory = 4;

                Window profileWindow = new View(this, myPerson, myCategory, true);
                profileWindow.ShowDialog();
            }
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
                case "Stores":
                    break;
                default:
                    return;
            }
            ResetRecords();


            ///////////////////////////////////////////////////////////
            //////////////////////  permissions  //////////////////////
            ///////////////////////////////////////////////////////////

            if (allPermissions[rank][currentCategory] < 2)
            {
                // permissions - view window and reset button
                if (allPermissions[rank][currentCategory] == 0)
                    actionButtons.Visibility = Visibility.Collapsed;
                else
                    actionButtons.Visibility = Visibility.Visible;

                // permissions - edit and remove buttons
                editButton.Visibility = Visibility.Collapsed;
                removeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // permissions - edit and remove buttons
                editButton.Visibility = Visibility.Visible;
                removeButton.Visibility = Visibility.Visible;
            }
            ///////////////////////////////////////////////////////////

            string str = ((sender as TabControl).SelectedItem as TabItem).Header as string;
            if (str.Equals("Stores"))
                if (string.IsNullOrEmpty(stores_cmb.Text))
                    categoryEmpty.Text = "Please choose a city from the list";
        }

        private void add_menu_Expanded(object sender, RoutedEventArgs e)
        {
            search_menu.IsExpanded = false;
        }

        private void search_menu_Expanded(object sender, RoutedEventArgs e)
        {
            add_menu.IsExpanded = false;
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isEndProccess)
                Application.Current.Shutdown();
        }

        private void CallStore(object sender, SelectionChangedEventArgs e)
        {
            web.Visibility = Visibility.Visible;
            allTabs.SelectedIndex = 7;
            allTabs.Height = 420;
            ComboBox cmb = ((ComboBox)sender);
            string selectedCity = cmb.SelectedItem.ToString();

            string str = "<html><body><table cellspacing='0' cellpadding='0' border='1' bordercolor='black'><tr><td><iframe src='http://www.yourmapmaker.com/preview.php?a=";
            str += stores_list[selectedCity];
            str += "&w=600&h=270&n=&z=14&t=Map' height='270' width='600' scrolling='no' frameborder='0'></iframe></td></tr></table></body></html>";
            web.NavigateToString(str);

            GetWeather(selectedCity, "metric");

            categoryEmpty.Visibility = Visibility.Collapsed;
        }

        public void GetWeather(string city, string unit)
        {
            try
            {
                string m_strFilePath = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&units=" + unit;//URL to API (including request)
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(m_strFilePath); // load the web service's reply -XML

                // look for Temperature
                XmlNode node = myXmlDocument.DocumentElement.ChildNodes[1];
                if (node != null)
                    if (node.Attributes != null)
                    {
                        temp.Text = "Temperature in " + city + ": ";
                        temp.Text += node.Attributes["value"].Value + " " + node.Attributes["unit"].InnerText; //get node's value for the tempurature
                    }

                // look for Humidity
                node = myXmlDocument.DocumentElement.ChildNodes[2];
                if (node != null)
                    if (node.Attributes != null)
                    {
                        temp.Text += "\nHumidity in " + city + ": ";
                        temp.Text += node.Attributes["value"].Value + " " + node.Attributes["unit"].InnerText; //get node's value for the tempurature
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine("could not get the weather" + e);
            }
        }


    }
}








