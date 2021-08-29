using BankApp_WPF.Models;
using LiveCharts;
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

namespace BankApp_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для TabItemPage.xaml
    /// </summary>
    public partial class TabItemPage : Page
    {
        private Department department;
        private Dictionary<Customer, PointShapeLineFullBalancePage> customerKey = new Dictionary<Customer, PointShapeLineFullBalancePage>();

        public TabItemPage(Department department)
        {
            InitializeComponent();
            this.department = department;
            lbCustomers.ItemsSource = this.department.Items;
        }

        public Customer AddRandomCustomer(string name)
        {
            Customer customer = department.AddNewCustomer(name);
            var fullBalanceGraphPage = new PointShapeLineFullBalancePage();
            customer.InitialAccount.NewBalance += fullBalanceGraphPage.InitialAccount_NewBalance;
            customerKey.Add(customer, fullBalanceGraphPage);
            return customer;
        }

        public void RemoveCustomer_MenuMainWindowClick()
        {
            if (!(lbCustomers.SelectedItem is Customer))
            {
                MessageBox.Show("You must select the customer to remove.");
            }
            else if ((lbCustomers.SelectedItem as Customer).Items.Count > 1)
            {
                MessageBox.Show("Before deleting a customer record, you must close all customer accounts except the main one.");
            }
            else
            {
                Customer concreteCustomer = lbCustomers.SelectedItem as Customer;

                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure to remove " +
                    $"'{concreteCustomer.Name}' customer? InitialBalance of '{concreteCustomer.Name}' = {concreteCustomer.InitialBalance:C2}.",
                    "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    concreteCustomer.InitialAccount.NewBalance -= customerKey[concreteCustomer].InitialAccount_NewBalance;
                    concreteCustomer.Remove(concreteCustomer.InitialAccount);
                    customerKey.Remove(concreteCustomer);
                    department.Remove(concreteCustomer);
                }
            }
        }

        public void AddCustomer_MenuMainWindowClick(MainWindow window)
        {
            var customerAddingWindow = new CustomerCreationWindow();
            customerAddingWindow.Owner = window;
            bool isClicked = (bool)customerAddingWindow.ShowDialog();

            var department = window.tabCntrl.SelectedItem as Department;
            if (isClicked)
            {
                var customer = department.AddNewCustomer(customerAddingWindow.tbCustomerName.Text);
                var customerBalanceGraphPage = new PointShapeLineFullBalancePage();
                customer.InitialAccount.NewBalance += customerBalanceGraphPage.InitialAccount_NewBalance;
                customerKey.Add(customer, customerBalanceGraphPage);
            }
        }

        private void lbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbCustomers.SelectedItem is Customer)
            {
                FrameFullBalanceGraph.Content = customerKey[lbCustomers.SelectedItem as Customer];
                lbAccounts.ItemsSource = (lbCustomers.SelectedItem as Customer).Items;

                //if ((lbCustomers.SelectedItem as Customer).Items[0].HistoryOfBalance.Count != 0)
                //{
                //    foreach (var item in (lbCustomers.SelectedItem as Customer).Items[0].HistoryOfBalance)
                //    {
                //        fullBalanceGraphPage.SeriesCollections[0].Values.Add(item);
                //    }
                //}
            }
        }

        public void MakeInitialDeposit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new GetSetDepositCredit();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                customer.DepositeInitialMoney(accountWindow.Amount);
            }
        }

        public void WithDrawInitialMoney_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new GetSetDepositCredit();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                if (customer.InitialBalance >= accountWindow.Amount)
                {
                    customer.WithDrawInitialMoney(accountWindow.Amount);
                }
                else MessageBox.Show("You don't have enough money, you can get a loan first.");
            }
        }

        public void AddNewDeposit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new GetSetDepositCredit();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                if (customer.InitialBalance >= accountWindow.Amount)
                {
                    customer.AddNewDeposit(accountWindow.Amount);
                }
                else MessageBox.Show("You don't have enough money, you can get a loan first.");
            }
        }

        public void WithDrawDeposit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;
            var deposit = lbAccounts.SelectedItem as Deposit;

            var accountWindow = new GetSetDepositCredit();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                if (deposit.Balance >= accountWindow.Amount)
                {
                    customer.WithDrawDeposit(deposit, accountWindow.Amount);
                }
                else MessageBox.Show("There is not enough money on your deposit to withdraw.");
            }
        }

        

    }
}
