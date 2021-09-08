﻿using BankApp;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BankApp_WPF.View
{
    public partial class TabItemDepartment
    {
        private Department department;
        private Dictionary<Customer, GraphFrame> customerKey = new Dictionary<Customer, GraphFrame>();

        public TabItemDepartment(Department department)
        {
            InitializeComponent();
            this.department = department;
            lbCustomers.ItemsSource = this.department.Items;
        }

        public Customer AddRandomCustomer(string name)
        {
            Customer customer = department.AddNewCustomer(name);
            var fullBalanceGraphPage = new GraphFrame();
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
                var customerBalanceGraphPage = new GraphFrame();
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
            }
        }

        public void MakeInitialDeposit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                customer.FundInitialAccount(accountWindow.Amount);
            }
        }

        public void WithDrawInitialMoney_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                if (customer.InitialBalance >= accountWindow.Amount)
                {
                    customer.WithDrawInitialAccount(accountWindow.Amount);
                }
                else MessageBox.Show("You don't have enough money, you can get a loan first.");
            }
        }

        public void AddNewDeposit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new CustomAccountGetSetWindow();
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

            var accountWindow = new CustomAccountGetSetWindow();
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

        public void GetCredit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                customer.GetCredit(accountWindow.Amount);
            }
        }

        public void RepayCredit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;
            var credit = lbAccounts.SelectedItem as Credit;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isClicked = (bool)accountWindow.ShowDialog();
            if (isClicked)
            {
                if (-credit.Balance >= accountWindow.Amount && customer.InitialBalance >= accountWindow.Amount)
                {
                    customer.RepayLoan(credit, accountWindow.Amount);
                }
                else MessageBox.Show("There is not enough money on your initial deposit to repay loan OR\n" +
                    "|credit account Balance| less then amount you want to repay.");
            }
        }

        public void CloseAccount_MenuItemClick()
        {
            var customer = lbCustomers.SelectedItem as Customer;
            var account = lbAccounts.SelectedItem as Account;

            if (account is Credit)
            {
                if (-account.Balance > customer.InitialBalance) MessageBox.Show("There is not enough money on your initial deposit to close account.");
                else customer.Remove(account);
            }
            else if (account is Deposit)
            {
                customer.Remove(account);
            }
            else if (account is InitialAccount)
            {
                MessageBox.Show("You cannot close initial account.");
            }
        }
    }
}