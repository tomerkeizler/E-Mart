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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>

    public partial class QueryClubMember : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private IBL itsClubMemberBL;

        //constructor
        public QueryClubMember(PL_GUI _parentWindow, IBL _itsClubMemberBL)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            itsClubMemberBL = _itsClubMemberBL;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { firstName, lastName, ID, specificMemberID, rangeMemberID, fromMemberID, toMemberID, specificTranID, rangeTranID, fromTranID, toTranID, male, female, dateOfBirth };
            Helper.ClearForm(lst);
        }
    }
}
