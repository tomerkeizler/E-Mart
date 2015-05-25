﻿using System;
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
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        // attributes
        private PL_GUI parentWindow;
        private User oldUser;

        // constructor
        public ChangePassword(PL_GUI _parentWindow, User _oldUser)
        {
            InitializeComponent();
            parentWindow = _parentWindow;
            oldUser = _oldUser;
        }

        private void ClearForm(object sender, RoutedEventArgs e)
        {
            List<Control> lst = new List<Control>() { newPass };
            Helper.ClearForm(lst);
        }


        // perform the password change
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User newUser = new User(oldUser);
            newUser.Password = newPass.Password;
            // editing action
            if (parentWindow.EditDataEntity(oldUser, newUser, 7))
                this.Close();
        }


    }
}
