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
using Backend;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class AddDepartment : Window
    {
       // attributes
        private PL_GUI parentWindow;
        
        // constructor
        public AddDepartment(PL_GUI _parentWindow)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { depName };
            Helper.ClearForm(lst);
        }


        // Add a new Department
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Department newDepartment = new Department(depName.Text);

            //adding action
            if (parentWindow.AddDataEntity(newDepartment, null, 3))
                this.Close();
        }


    }
}
