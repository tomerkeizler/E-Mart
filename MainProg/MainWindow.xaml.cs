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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using DAL;
using PL;

namespace MainProg
{   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IDAL myDal = new LINQ_DAL();

            IBL clubMember = new ClubMember_BL(myDal);
            IBL customer = new Customer_BL(myDal);
            IBL department = new Department_BL(myDal);
            IBL employee = new Employee_BL(myDal);
            IBL product = new Product_BL(myDal);
            IBL transaction = new Transaction_BL(myDal);
            IBL user = new User_BL(myDal);  
            
            IPL myPL = new PL_GUI(clubMember, customer, department, employee, product, transaction, user);
            myPL.Run();

            Window w1 = new QueryClubMember();
            w1.Show();
            Window w2 = new QueryCustomer();
            w2.Show();
            Window w3 = new QueryDepartment();
            w3.Show();
            Window w4 = new QueryEmployee();
            w4.Show();
            Window w5 = new QueryProduct();
            w5.Show();
            Window w6 = new QueryTransaction();
            w6.Show();
            Window w7 = new QueryUser();
            w7.Show();
    
    
        }

    }
}
