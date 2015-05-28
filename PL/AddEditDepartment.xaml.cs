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
    public partial class AddEditDepartment : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private bool isAdd;
        private Object oldObj;

        // constructor
        public AddEditDepartment(PL_GUI _parentWindow, bool _isAdd, Object _oldObj)
        {
            InitializeComponent();
            parentWindow = _parentWindow;

            isAdd = _isAdd;
            if (!isAdd)
            {
                AddEditTitle.Text = "Edit Department";
                AddEditButton.Content = "Edit Department";
                oldObj = _oldObj;
                ResetToDefault();
            }
        }

        // Reset the form to the old object details
        public void ResetToDefault()
        {
            depName.Text = ((Department)oldObj).Name;
        }

        // Clear form if adding or Resetting to default if editing
        private void ResetForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { depName };
            if (isAdd)
                PL_GUI.ClearForm(lst);
            else
                ResetToDefault();
        }

        // Add or edit
        private void AddOrEdit(object sender, RoutedEventArgs e)
        {
            Department newObj = new Department(depName.Text);

            //adding action
            if (isAdd)
            {
                if (parentWindow.AddDataEntity(newObj, null, 3))
                    this.Close();
            }
            //editing action
            else
            {
                if (parentWindow.EditDataEntity(oldObj, newObj, 3))
                    this.Close();
            }
        }


    }
}
