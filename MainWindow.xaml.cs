using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            CreateBank();

            tbBankName.DataContext = bank;
            spTimer.DataContext = bank.Timer;
            tabCntrl.ItemsSource = bank.Items;
        }

        private void CreateBank()
        {
            CreateNameForNewBankApp newWindow = new CreateNameForNewBankApp();
            newWindow.ShowDialog();
            bank = new Bank(newWindow.tbBankName.Text);
            if ((bool)newWindow.checkBoxRandom.IsChecked)
            {
                bank.Name = "BANK_FOR_TESTING";
                bank.CreateDepartment(AttributeDepartment.Persons)
                    .AddNewCustomer("FirstPerson_Name")
                    .GetCredit(1000m)
                    .AddNewDeposit(900m);

                bank.Items[0].AddNewCustomer("SecondPerson_Name")
                             .DepositeMoney(1000m)
                             .AddNewDeposit(500m)
                             .AddNewDeposit(500m);

                bank.CreateDepartment(AttributeDepartment.Organizations)
                    .AddNewCustomer("ORGANIZATION");
            }
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
        }
    }
}
