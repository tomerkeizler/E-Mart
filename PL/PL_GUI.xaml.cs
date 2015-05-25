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

        // attributes
        private IBL[] cats;
        private string[] catsNames;
        private ObservableCollection<Object>[] data;
        public User user;
        public Rank rank;
        public bool[] viewPermissions;
        public bool[] fullPermissions;
        
        // constructor
        public PL_GUI(IBL itsClubMemberBL, IBL itsCustomerBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            InitializeComponent();
            cats = new IBL[8] { null, itsClubMemberBL, itsCustomerBL, itsDepartmentBL, itsEmployeeBL, itsProductBL, itsTransactionBL, itsUserBL };
            catsNames = new string[8] { "", "Club member", "Customer", "Department", "Employee", "Product", "Transaction", "User" };
            data = new ObservableCollection<Object>[8];
        }

        // Main method
        public void Run()
        {
            InitializeComponent();
            // Login
            // the main window is being opened by the Login window when logging in succeeds
            Window login = new Login(cats[7], this);
            login.ShowDialog();


            /////////////
            //viewPermissions[1] = false;
            /////////////



            /*
            String myName;
            if (user.Person is Customer)
                myName = ((Customer)user.Person).FirstName;
            if (user.Person is ClubMember)
                myName = ((ClubMember)user.Person).FirstName;
            else
                myName = ((Employee)user.Person).FirstName;

            title_name.Text = "Hey " + myName + "!";
            title_rank.Text = "Logged in as " + rank.ToString();
            */

            /*
             List<Object> objList = new List<Object>();
            for (int i = 0; i < 10; i++)
                objList.Add(new ClubMember(203608096, "Tomer", "Keizler", new DateTime(1991, 9, 5), Gender.Male, 0, null));
            */


            // generate all lists of data entities
            for (int i = 1; i < 8; i++)
                data[i] = new ObservableCollection<Object>(cats[i].GetAll());
            // bind DataGrids to lists
            clubMemberGrid.DataContext = data[1];
            CustomerGrid.DataContext = data[2];
            DepartmentGrid.DataContext = data[3];
            EmployeeGrid.DataContext = data[4];
            ProductGrid.DataContext = data[5];
            TransactionGrid.DataContext = data[6];
            UserGrid.DataContext = data[7];

        }


        


        private void ShowAll()
        {
            List<Object> objList = cats[1].GetAll();
            objList.Add(new ClubMember(203608096, "Tomer", "Keizler", new DateTime(1991, 9, 5), Gender.Male, 0, null));
            ObservableCollection<Object> clubMemberList = new ObservableCollection<Object>(objList);


        }
       
        /////////////////
        // Add methods //
        /////////////////
        private void CallAddClubMember(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddClubMembner(this);
            addForm.ShowDialog();
        }

        private void CallAddCustomer(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddCustomer(this, false);
            addForm.ShowDialog();
        }

        private void CallAddDepartment(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddDepartment(this);
            addForm.ShowDialog();
        }

        private void CallAddEmployee(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddEmployee(this, cats[3], cats[4]);
            addForm.ShowDialog();
        }

        private void CallAddProduct(object sender, RoutedEventArgs e)
        {
            Window addForm = new AddProduct(this);
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







    }
}
