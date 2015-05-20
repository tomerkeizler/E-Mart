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
            InitializeComponent();

            IDAL myDal = new LINQ_DAL();

            IBL clubMember = new ClubMember_BL(myDal);
            //IBL customer = new Customer_BL(myDal);
            IBL department = new Department_BL(myDal);
            IBL employee = new Employee_BL(myDal);
            IBL product = new Product_BL(myDal);
            IBL transaction = new Transaction_BL(myDal);
            IBL user = new User_BL(myDal);

            //IPL myPl = new PL_CLI(clubMember, department, employee, product, transaction, user);
            //myPl.Run();

            Login lg = new Login();
            lg.Show();
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
