using BankApp;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CustomerExtensions;

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

        public Customer AddDefaultCustomer(string name)
        {
            Customer customer = department.AddNewCustomer(name);
            var fullBalanceGraphPage = new GraphFrame();
            customer.InitialAccount.NewBalance += fullBalanceGraphPage.InitialAccount_NewBalance;
            customerKey.Add(customer, fullBalanceGraphPage);
            return customer;
        }

        public void RemoveCustomer_MenuMainWindowClick()
        {
            if (!(lbCustomers.SelectedItem is Customer customer))
            {
                MessageBox.Show("You must select the customer to remove.");
            }
            else if (customer.Items.Count > 1)
            {
                MessageBox.Show("Before deleting a customer record, you must close all customer accounts except the main one.");
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure to remove " +
                    $"'{customer.Name}' customer? InitialBalance of '{customer.Name}' = {customer.InitialBalance:C2}.",
                    "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    customer.InitialAccount.NewBalance -= customerKey[customer].InitialAccount_NewBalance;
                    customer.Remove(customer.InitialAccount);
                    customerKey.Remove(customer);
                    CustomerExtension.LogsRepository.Remove(customer); //New Extensions property
                    department.Remove(customer);
                }
            }
        }

        public void AddCustomer_MenuMainWindowClick(MainWindow window)
        {
            var customerAddingWindow = new CustomerCreationWindow();
            customerAddingWindow.Owner = window;
            bool isClicked = (bool)customerAddingWindow.ShowDialog();

            if (isClicked)
            {
                var department = window.tabCntrl.SelectedItem as Department;
                var customer = department.AddNewCustomer(customerAddingWindow.tbCustomerName.Text);
                var customerBalanceGraphPage = new GraphFrame();
                customer.InitialAccount.NewBalance += customerBalanceGraphPage.InitialAccount_NewBalance;
                customerKey.Add(customer, customerBalanceGraphPage);
                CustomerExtension.LogsRepository.Add(customer, new RepLogs()); //New Extensions property
            }
        }

        private void lbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbCustomers.SelectedItem is Customer customer)
            {
                FrameFullBalanceGraph.Content = customerKey[customer];
                lbAccounts.ItemsSource = customer.Items;
                lbLogs.ItemsSource = CustomerExtension.LogsRepository[customer].CurrentLogs; //New Extensions property binding
            }
        }

        public void MakeInitialDeposit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isCorrectValue = (bool)accountWindow.ShowDialog();
            if (isCorrectValue)
            {
                customer.FundInitialAccount(accountWindow.Amount);
            }
        }

        public void WithDrawInitialMoney_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isCorrectValue = (bool)accountWindow.ShowDialog();
            if (isCorrectValue)
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
            bool isCorrectValue = (bool)accountWindow.ShowDialog();
            if (isCorrectValue)
            {
                if (customer.InitialBalance >= accountWindow.Amount)
                {
                    var customerAccountsCount = customer.Items.Count; //New Extensions subscription
                    customer.AddNewDeposit(accountWindow.Amount);
                    (customer.Items[customerAccountsCount] as Deposit).BalanceChanged += customer.TransferLogs; //New Extensions subscription
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
            bool isCorrectValue = (bool)accountWindow.ShowDialog();
            if (isCorrectValue)
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
                var customerAccountsCount = customer.Items.Count; //New Extensions subscription
                customer.GetCredit(accountWindow.Amount);
                (customer.Items[customerAccountsCount] as Credit).BalanceChanged += customer.TransferLogs; //New Extensions subscription
            }
        }

        public void RepayCredit_MenuItemClick(MainWindow window)
        {
            var customer = lbCustomers.SelectedItem as Customer;
            var credit = lbAccounts.SelectedItem as Credit;

            var accountWindow = new CustomAccountGetSetWindow();
            accountWindow.Owner = window;
            bool isCorrectValue = (bool)accountWindow.ShowDialog();
            if (isCorrectValue)
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

            if (account is Credit credit)
            {
                if (-account.Balance > customer.InitialBalance) 
                {
                    MessageBox.Show("There is not enough money on your initial deposit to close account."); 
                }
                else
                {
                    credit.BalanceChanged -= customer.TransferLogs; //New Extensions unsubscription
                    customer.Remove(account);
                }
            }
            else if (account is Deposit deposit)
            {
                deposit.BalanceChanged -= customer.TransferLogs; //New Extensions unsubscription
                customer.Remove(account);
            }
            else if (account is InitialAccount)
            {
                MessageBox.Show("You cannot close initial account.");
            }
        }
    }
}
