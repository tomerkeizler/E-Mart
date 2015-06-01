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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class QueryUser : Window
    {
        // attributes
        private PL_GUI parentWindow;

        //constructor
        public QueryUser(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { username, rank };
            PL_GUI.ClearForm(lst);
        }


        private void SearchByUserame(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.RegExp(username.Text, "User name", 1))
                if (parentWindow.SearchDataEntity(StringFields.username, username.Text, null, 7))
                    this.Close();
        }

        private void SearchByRank(object sender, RoutedEventArgs e)
        {
            if (PL_GUI.ComboboxValidate(rank, "Rank"))
            {
                Rank rnk;
                if (rank.Text.Equals("Administrator"))
                    rnk = Rank.Administrator;
                else if (rank.Text.Equals("Manager"))
                    rnk = Rank.Manager;
                else if (rank.Text.Equals("Worker"))
                    rnk = Rank.Worker;
                else
                    rnk = Rank.Customer;
                if (parentWindow.SearchDataEntity(TypeFields.rank, rnk, null, 7))
                    this.Close();
            }
        }

    }
}
