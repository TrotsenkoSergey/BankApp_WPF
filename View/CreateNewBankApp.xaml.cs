﻿using System.Windows;

namespace BankApp_WPF.View
{
    
    public partial class CreateNewBankApp : Window
    {
        public CreateNewBankApp()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}