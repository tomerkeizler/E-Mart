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


namespace PL
{
    /// <summary>
    /// Interaction logic for PL_GUI.xaml
    /// </summary>
    public partial class PL_GUI : Window, IPL
    {
        
        // attributes
        private IBL[] cats;
        public static int[][] permissions = new int[4][];

        // constructors
        static PL_GUI()
        {
            /* Permissions array
             * 
             * 1st dimension represents user types:
             * Admin, Manager, Worker, Customer
             * 
             * 2nd dimension represents data entities:
             * ClubMember, Customer, Department, Employee, Product, Transaction, User, self-viewing
             * 
             * Meaning of cell values:
             * 0 = no permission
             * 1 = permission to view
             * 2 = permission to view, add, edit, remove */
            permissions[0] = new int[8] { 2, 2, 2, 2, 2, 2, 2, 2 }; //Admin
            permissions[1] = new int[8] { 2, 2, 0, 2, 2, 2, 0, 1 }; // Manager
            permissions[2] = new int[8] { 1, 1, 0, 0, 0, 1, 0, 1 }; // Worker
            permissions[3] = new int[8] { 0, 0, 0, 0, 1, 1, 0, 1 }; // Customer

        }

        public PL_GUI(IBL itsClubMemberBL, IBL itsCustomerBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            InitializeComponent();
            cats = new IBL[8];
            cats[0] = null;
            cats[1] = itsClubMemberBL;
            cats[2] = itsCustomerBL;
            cats[3] = itsDepartmentBL;
            cats[4] = itsEmployeeBL;
            cats[5] = itsProductBL;
            cats[6] = itsTransactionBL;
            cats[7] = itsUserBL;
            //catsNames = new string[7] { "", "Club member", "Department", "Employee", "Product", "Transaction", "User" };
        }



        public void Run()
        {
            InitializeComponent();
            Window login = new Login();
            //login.Show();
            this.Show();



            //List<Object> objList = cats[1].GetAll();
            List<Object> objList = new List<Object>();
            for (int i = 0; i < 10; i++)
                objList.Add(new ClubMember(203608096, "Tomer", "Keizler", new DateTime(1991, 9, 5), Gender.Male, 0, null));
            ObservableCollection<Object> clubMemberList = new ObservableCollection<Object>(objList);
            clubMemberGrid.DataContext = clubMemberList;





        }



        private void ShowAll()
        {
            List<Object> objList = cats[1].GetAll();
            objList.Add(new ClubMember(203608096, "Tomer", "Keizler", new DateTime(1991, 9, 5), Gender.Male, 0, null));
            ObservableCollection<Object> clubMemberList = new ObservableCollection<Object>(objList);


        }
    }
}
