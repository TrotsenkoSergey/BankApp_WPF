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
using BankApp_WPF.Models;
using BankApp_WPF.View;

namespace BankApp_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bank bank;

        public MainWindow()
        {
            InitializeComponent();
            CreateNameForNewBankApp newWindow = new CreateNameForNewBankApp();
            newWindow.ShowDialog();
            bank = new Bank(newWindow.tbBankName.Text);
            tbBankName.DataContext = bank;
            spTimer.DataContext = bank.Timer;
            tabCntrl.ItemsSource = bank.Items;
            
            CreateDepartments();
            
                        
        }

        private void CreateDepartments()
        {
            bank.CreateDepartment(AttributeDepartment.Persons);
            //bank.CreateDepartment(AttributeDepartment.Organizations);
            //bank.CreateDepartment(AttributeDepartment.VipPersons);
            //bank.CreateDepartment(AttributeDepartment.VipOrganizations);
        }

        private void butTimer_Click(object sender, RoutedEventArgs e)
        {
            int months = default;
            bool goodNum = Int32.TryParse(tbTimerNewNum.Text, out months);
            bank.Timer.NextTime(months);
        }

        private void tbTimerNewNum_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            tbTimerNewNum.Text = string.Empty;
        }

        private void tabCntrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page selectedTabItem = new TabItemPage(tabCntrl.SelectedItem as Department);
            MainFrame.Content = selectedTabItem;
            (tabCntrl.SelectedItem as Department).AddNewCustomer("Sergey");
            (tabCntrl.SelectedItem as Department).Items[0].DepositeMoney(2000m);
        }
    }
}
