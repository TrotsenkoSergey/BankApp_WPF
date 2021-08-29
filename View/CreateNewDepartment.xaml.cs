using BankApp_WPF.Models;
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

namespace BankApp_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для CreateNewDepartment.xaml
    /// </summary>
    public partial class CreateNewDepartment : Window
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
