using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BankApp_WPF.Models;
using BankApp_WPF.View;

namespace BankApp_WPF
{

    /// <summary>
    /// Main App window.
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bank bank;
        private Dictionary<Department, TabItemPage> departmentsKey = new Dictionary<Department, TabItemPage>();

        public MainWindow()
        {
            InitializeComponent();
            CreateBank();

            tbBankName.DataContext = bank;
            spTimer.DataContext = bank.Timer;
            tabCntrl.ItemsSource = bank.Items;
        }

        /// <summary>
        /// Create Bank with random content.
        /// </summary>
        private void CreateBank()
        {
            CreateNameForNewBankApp newWindow = new CreateNameForNewBankApp();
            newWindow.ShowDialog();
            bank = new Bank(newWindow.tbBankName.Text);
            if ((bool)newWindow.checkBoxRandom.IsChecked)
            {
                bank.Name = "BANK_FOR_TESTING";

                var department = bank.CreateDepartment(AttributeDepartment.Persons);
                var tabItem = new TabItemPage(department);
                departmentsKey.Add(department, tabItem);
                departmentsKey[department].AddRandomCustomer("FirstPerson_Name")
                          .GetCredit(1000m)
                          .AddNewDeposit(900m);
                departmentsKey[department].AddRandomCustomer("SecondPerson_Name")
                         .DepositeInitialMoney(1000m)
                         .AddNewDeposit(500m)
                         .AddNewDeposit(500m);

                department = bank.CreateDepartment(AttributeDepartment.Organizations);
                tabItem = new TabItemPage(department);
                departmentsKey.Add(department, tabItem);
                departmentsKey[department].AddRandomCustomer("ORGANIZATION");
            }
        }

        /// <summary>
        /// Handles the next period event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butTimer_Click(object sender, RoutedEventArgs e)
        {
            int months;
            Int32.TryParse(tbTimerNewNum.Text, out months);
            bank.Timer.NextTime(months);
        }

        /// <summary>
        /// Handles GotKeyboardFocus event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTimerNewNum_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            tbTimerNewNum.Text = string.Empty;
        }

        /// <summary>
        /// Handles SelectionChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCntrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department)
            {
                var selectedTabItem = departmentsKey[tabCntrl.SelectedItem as Department];
                MainFrame.Content = selectedTabItem;
            }
        }

        private void CreateMenuDepartment_Click(object sender, RoutedEventArgs e)
        {
            var departmentCreationWindow = new CreateNewDepartment();
            departmentCreationWindow.Owner = this;
            bool isClicked = (bool)departmentCreationWindow.ShowDialog();
            if (isClicked)
            {
                var type = Enum.Parse(typeof(AttributeDepartment), departmentCreationWindow.cbAtribute.SelectedItem as String);
                var department = bank.CreateDepartment((AttributeDepartment)type);
                if (!String.IsNullOrEmpty(departmentCreationWindow.tbDepartmentName.Text))
                {
                    department.Name = departmentCreationWindow.tbDepartmentName.Text;
                }
                var tabItem = new TabItemPage(department);
                departmentsKey.Add(department, tabItem);
            }
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            tabCntrl.SelectedItem = (sender as TabItem).DataContext;
        }

        private void RemoveMenuDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (!(tabCntrl.SelectedItem is Department))
            {
                MessageBox.Show("You must select the department to remove.");
            }
            else if ((tabCntrl.SelectedItem as Department).Items.Count != 0)
            {
                MessageBox.Show($"Into '{(tabCntrl.SelectedItem as Department).Name}' department you have '{(tabCntrl.SelectedItem as Department).Items.Count}' customers.\n" +
                    $"First you must remove customers.");
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure to remove '{(tabCntrl.SelectedItem as Department).Name}' department?",
                     "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    //departmentsKey[tabCntrl.SelectedItem as Department].lbCustomer.ItemsSource = null;
                    //MainFrame.Content = null;
                    departmentsKey.Remove(tabCntrl.SelectedItem as Department);
                    bank.Remove(tabCntrl.SelectedItem as Department);
                }
            }

        }

        private void AddCustomer_MenuClick(object sender, RoutedEventArgs e)
        {
            if (!(tabCntrl.SelectedItem is Department))
            {
                MessageBox.Show("You must select the department where you want to add the customer record.");
            }
            else
            {
                departmentsKey[tabCntrl.SelectedItem as Department].AddCustomer_MenuMainWindowClick(this);
            }
        }

        private void RemoveCustomer_MenuClick(object sender, RoutedEventArgs e)
        {
            if (!(tabCntrl.SelectedItem is Department))
            {
                MessageBox.Show("You must select the department, then customer to remove.");
            }
            else
            {
                departmentsKey[tabCntrl.SelectedItem as Department].RemoveCustomer_MenuMainWindowClick();
            }
        }

        private void MakeInitialDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department && 
                departmentsKey[tabCntrl.SelectedItem as Department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[tabCntrl.SelectedItem as Department].MakeInitialDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        public void WithDrawInitialMoney_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department && 
                departmentsKey[tabCntrl.SelectedItem as Department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[tabCntrl.SelectedItem as Department].WithDrawInitialMoney_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void AddNewDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department && 
                departmentsKey[tabCntrl.SelectedItem as Department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[tabCntrl.SelectedItem as Department].AddNewDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void WithDrawDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department && 
                departmentsKey[tabCntrl.SelectedItem as Department].lbCustomers.SelectedItem is Customer && 
                departmentsKey[tabCntrl.SelectedItem as Department].lbAccounts.SelectedItem is Deposit)
            {
                departmentsKey[tabCntrl.SelectedItem as Department].WithDrawDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete deposit account.");
            }
        }

        private void GetCredit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department &&
                departmentsKey[tabCntrl.SelectedItem as Department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[tabCntrl.SelectedItem as Department].GetCredit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void RepayCredit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department &&
                departmentsKey[tabCntrl.SelectedItem as Department].lbCustomers.SelectedItem is Customer &&
                departmentsKey[tabCntrl.SelectedItem as Department].lbAccounts.SelectedItem is Credit)
            {
                departmentsKey[tabCntrl.SelectedItem as Department].RepayCredit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete credit account.");
            }
        }

    }
}
