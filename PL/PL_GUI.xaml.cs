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

namespace PL
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PL_GUI : Window, IPL
    {
        // attributes
        private IBL[] cats;

        // constructors
        public PL_GUI(IBL itsClubMemberBL, IBL itsCustomerBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            //InitializeComponent();
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

        public void run()
        {
            this.Show();
        }




    }
}
