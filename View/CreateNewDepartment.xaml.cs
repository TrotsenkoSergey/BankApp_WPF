using BankApp;
using System;
using System.Windows;

namespace BankApp_WPF.View
{
    
    public partial class CreateNewDepartment
    {
        public CreateNewDepartment()
        {
            InitializeComponent();
            cbAtribute.ItemsSource = Enum.GetNames(typeof(AttributeDepartment));
            cbAtribute.SelectedItem = Enum.GetName(typeof(AttributeDepartment), AttributeDepartment.Persons);
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
