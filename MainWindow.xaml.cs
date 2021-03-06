using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using BankApp_WPF.View;
using BankApp;
using CustomerExtensions;
using System.Threading.Tasks;
using Extensions;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Threading;

namespace BankApp_WPF
{
    /// <summary>
    /// Main App window.
    /// </summary>
    public partial class MainWindow
    {
        private Bank _bank;
        private Dictionary<Department, TabItemDepartment> _departmentsKey
            = new Dictionary<Department, TabItemDepartment>();
        private ProgressLoad _progressLoad;

        public MainWindow()
        {
            InitializeComponent();
            //CreateBank();
            _bank = new Bank();

            tbBankName.DataContext = _bank;
            spTimer.DataContext = _bank.Timer;
            tabCntrl.ItemsSource = _bank.Items;
        }

        /// <summary>
        /// Create Bank with random content.
        /// </summary>
        private async void CreateBank(object sender, RoutedEventArgs e)
        {
            var newWindow = new CreateNewBankApp();
            newWindow.ShowDialog();
            if (!String.IsNullOrEmpty(newWindow.tbBankName.Text))
            { _bank.Name = newWindow.tbBankName.Text; }

            if ((bool)newWindow.checkBoxRandMillion.IsChecked)
            {
                //Progress<int> progress = default;
                //await Dispatcher.BeginInvoke((Action)(() =>
                //{
                _progressLoad = new ProgressLoad();
                _progressLoad.Show();
                var progress = new Progress<int>(value =>
               {
                   _progressLoad.progressBar.Value = value;
               });
                //}));

                _bank.Name = "BANK_FOR_MILLION_CUSTOMER_TESTING";

                var department = _bank.CreateDepartment(AttributeDepartment.Persons);
                var tabItem = new TabItemDepartment(department);
                _departmentsKey.Add(department, tabItem);

                //CreateDefaultCustomers(department, progress);
                //Task t = CreateDefaultCustomersAsync(department, progress);
                //t.Wait();
                await CreateDefaultCustomersAsync(department, progress);
                //var t = Task.Factory.StartNew(() => CreateDefaultCustomersAsync(department, progress, _departmentsKey));
                //t.Wait();
            }
            else if ((bool)newWindow.checkBoxRandThree.IsChecked)
            {
                _bank.Name = "BANK_FOR_TESTING";
                CreateThreeCustomers();
            }
        }

        private void CreateThreeCustomers()
        {
            var department = _bank.CreateDepartment(AttributeDepartment.Persons);
            var tabItem = new TabItemDepartment(department);
            _departmentsKey.Add(department, tabItem);
            _departmentsKey[department].AddDefaultCustomer("FirstPerson_Name")
                    .GetCredit(1000m)
                    .AddNewDeposit(900m)
                    .IntroduceLogsDefaultCustomer(); //New Extensions property
            _departmentsKey[department].AddDefaultCustomer("SecondPerson_Name")
                     .FundInitialAccount(1000m)
                     .AddNewDeposit(500m)
                     .AddNewDeposit(500m)
                     .IntroduceLogsDefaultCustomer(); //New Extensions property

            department = _bank.CreateDepartment(AttributeDepartment.Organizations);
            tabItem = new TabItemDepartment(department);
            _departmentsKey.Add(department, tabItem);
            _departmentsKey[department].AddDefaultCustomer("ORGANIZATION")
                .IntroduceLogsDefaultCustomer();
        }

        private Customer CreateDefaultCustomer(Department department, int i)
        {
            var rand = new Random();
            var credit = rand.Next(100, 1000);
            var customer = department.AddNewCustomer($"Person_Name_{i}");
            customer.GetCredit(credit)
                    .AddNewDeposit(rand.Next(0, credit))
                    .IntroduceLogsDefaultCustomer();
            return customer;
        }

        private void CreateDefaultCustomers(Department department, IProgress<int> progress)
        {
            var rand = new Random();
            int length = 10_000;
            int value = 0;
            int step = length / 100;
            //Parallel.For(0, length, (i) =>
            //{
            //Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            //    () =>
            //    {
            for (int i = 0; i < length; i++)
            {
                var credit = rand.Next(100, 1000);
                _departmentsKey[department]
                  .AddDefaultCustomer($"Person_Name_{i}")
                      .GetCredit(credit)
                      .AddNewDeposit(rand.Next(0, credit))
                           .IntroduceLogsDefaultCustomer();
                value += step;
                progress.Report(value);
            }
            //});
        }

        private async Task CreateDefaultCustomersAsync(Department department, IProgress<int> progress)
        {
            var rand = new Random();
            int length = 10_000;
            int value = 0;
            int step = length / 100;

            await Dispatcher.BeginInvoke((Action)(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    var credit = rand.Next(100, 1000);
                    _departmentsKey[department]
                       .AddDefaultCustomer($"Person_Name_{i}")
                           .GetCredit(credit)
                           .AddNewDeposit(rand.Next(0, credit))
                                .IntroduceLogsDefaultCustomer();
                    value += step;
                    progress.Report(value);
                }
            }));
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
            _bank.Timer.NextTime(months);
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
            if (tabCntrl.SelectedItem is Department department)
            {
                var selectedTabItem = _departmentsKey[department];
                MainFrame.Content = selectedTabItem;
            }
        }

        private void CreateMenuDepartment_Click(object sender, RoutedEventArgs e)
        {
            var departmentCreationWindow = new CreateNewDepartment();
            departmentCreationWindow.Owner = this;
            bool isCorrectValue = (bool)departmentCreationWindow.ShowDialog();
            if (isCorrectValue)
            {
                var type = Enum.Parse(typeof(AttributeDepartment), departmentCreationWindow.cbAtribute.SelectedItem as String);
                var department = _bank.CreateDepartment((AttributeDepartment)type);
                if (!String.IsNullOrEmpty(departmentCreationWindow.tbDepartmentName.Text))
                {
                    department.Name = departmentCreationWindow.tbDepartmentName.Text;
                }
                var tabItem = new TabItemDepartment(department);
                _departmentsKey.Add(department, tabItem);
            }
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            tabCntrl.SelectedItem = (sender as TabItem).DataContext;
        }

        private void RemoveMenuDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (!(tabCntrl.SelectedItem is Department department))
            {
                MessageBox.Show("You must select the department to remove.");
            }
            else if (department.Items.Count != 0)
            {
                MessageBox.Show($"Into '{department.Name}' department you have '{department.Items.Count}' customers.\n" +
                    $"First you must remove customers.");
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure to remove '{department.Name}' department?",
                     "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    _departmentsKey.Remove(department);
                    _bank.Remove(department);
                }
            }

        }

        private void AddCustomer_MenuClick(object sender, RoutedEventArgs e)
        {
            if (!(tabCntrl.SelectedItem is Department department))
            {
                MessageBox.Show("You must select the department where you want to add the customer record.");
            }
            else
            {
                _departmentsKey[department].AddCustomer_MenuMainWindowClick(this);
            }
        }

        private void RemoveCustomer_MenuClick(object sender, RoutedEventArgs e)
        {
            if (!(tabCntrl.SelectedItem is Department department))
            {
                MessageBox.Show("You must select the department, then customer to remove.");
            }
            else
            {
                _departmentsKey[department].RemoveCustomer_MenuMainWindowClick();
            }
        }

        private void MakeInitialDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                _departmentsKey[department].MakeInitialDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        public void WithDrawInitialMoney_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                _departmentsKey[department].WithDrawInitialMoney_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void AddNewDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                _departmentsKey[department].AddNewDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void WithDrawDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer &&
                _departmentsKey[department].lbAccounts.SelectedItem is Deposit)
            {
                _departmentsKey[department].WithDrawDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete deposit account.");
            }
        }

        private void GetCredit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                _departmentsKey[department].GetCredit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void RepayCredit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer &&
                _departmentsKey[department].lbAccounts.SelectedItem is Credit)
            {
                _departmentsKey[department].RepayCredit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete credit account.");
            }
        }

        private void CloseAccount_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                _departmentsKey[department].lbCustomers.SelectedItem is Customer &&
                _departmentsKey[department].lbAccounts.SelectedItem is Account)
            {
                _departmentsKey[department].CloseAccount_MenuItemClick();
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete account.");
            }
        }

        /// <summary>
        /// Calls NotePad and opens the HelpENG.txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpENG_Click(object sender, RoutedEventArgs e)
        {
            //string s = Directory.GetCurrentDirectory();
            //Process.Start("notepad.exe", "HelpENG.txt");
            var directInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            string combined = Path.Combine(directInfo.Parent.Parent.FullName, "HelpENG.txt");
            Process.Start("notepad.exe", combined);
        }

        /// <summary>
        /// Calls NotePad and opens the HelpRUS.txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpRUS_Click(object sender, RoutedEventArgs e)
        {
            //string s = Directory.GetCurrentDirectory();
            //Process.Start("notepad.exe", "HelpRUS.txt");
            var directInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            string combined = Path.Combine(directInfo.Parent.Parent.FullName, "HelpRUS.txt");
            Process.Start("notepad.exe", combined);
        }

        /// <summary>
        /// Shows copyright. And allows you to follow the link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reference_Click(object sender, RoutedEventArgs e)
        {
            string link = @"https://github.com/TrotsenkoSergey/BankApp_WPF";
            var result = MessageBox.Show(
                "Copyright (c) Sergey Trotsenko. All rights reserved.\n\n" +
                $"{link}\n\n" +
                "Click OK button if you want to link...",
                "Reference",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information
                );
            if (result == MessageBoxResult.OK)
            {
                Process.Start($"{link}");
            }
        }

        private async void Save_ClickAsync(object o, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Dialog Save window Json Bank repository",
                Filter = "Json files (*.json)|*.json",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string pathToSave = Path.Combine(saveFileDialog.InitialDirectory, saveFileDialog.FileName);
                var serializer = new DataSerializer();
                await serializer.JsonSerializeAsync(_bank, pathToSave);
            }
        }

        private void NextStepLoad()
        {
            //await Task.Factory.StartNew(() =>
            //{
            if (_bank.Items.Count != 0)
                foreach (var department in _bank.Items)
                {
                    var tabItem = new TabItemDepartment(department);
                    _departmentsKey.Add(department, tabItem);

                    if (department.Items.Count != 0)
                        //Parallel.For(0, department.Items.Count, (int i) =>
                        //{
                        //    var customer = department.Items[i];
                        foreach (var customer in department.Items)
                        {
                            if (customer.Items.Count != 0)
                                foreach (var account in customer.Items)
                                {
                                    if (account is Credit credit)
                                    {
                                        credit.BalanceChanged += customer.InitialAccount.OnBalanceChanged;
                                        credit.BalanceChanged += customer.TransferLogs;
                                    }
                                    else if (account is Deposit deposit)
                                    {
                                        deposit.BalanceChanged += customer.InitialAccount.OnBalanceChanged;
                                        deposit.BalanceChanged += customer.TransferLogs;
                                    }
                                    else if (account is InitialAccount initialAccount)
                                    {
                                        customer.InitialAccount = initialAccount;
                                    }
                                }
                            var customerBalanceGraphPage = new GraphFrame();
                            customer.InitialAccount.NewBalance += customerBalanceGraphPage.InitialAccount_NewBalance;
                            _departmentsKey[department].CustomerKey.Add(customer, customerBalanceGraphPage);
                            if (customer.InitialAccount.HistoryOfBalance.Count != 0)
                                foreach (var num in customer.InitialAccount.HistoryOfBalance)
                                {
                                    customerBalanceGraphPage.InitialAccount_NewBalance(num);
                                }
                            CustomerExtension.LogsRepository.Add(customer.Name, new RepLogs());
                        }
                    //});
                }
            //});
        }

        private void LoadData(string path)
        {
            var deserializer = new DataSerializer();
            _bank = deserializer.JsonDeserialize(path);
        }

        private async Task<Bank> LoadDataAsync(string path)
        {
            var deserializer = new DataSerializer();
            var bank = await deserializer.JsonDeserializeAsync(path);
            return bank;
        }

        private async void LoadClickAsync(object s, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Dialog Load window Json Bank repository",
                Filter = "Json files (*.json)|*.json",
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            bool? isClicked = openFileDialog.ShowDialog();

            if (isClicked == true)
            {
                _bank = await LoadDataAsync(openFileDialog.FileName);
            }

            tbBankName.DataContext = _bank;
            spTimer.DataContext = _bank.Timer;
            tabCntrl.ItemsSource = _bank.Items;
            //await Dispatcher.BeginInvoke((Action)(() => NextStepLoad()));
            NextStepLoad();
        }

        private void LoadClick(object s, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Dialog Load window Json Bank repository",
                Filter = "Json files (*.json)|*.json",
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            bool? isClicked = openFileDialog.ShowDialog();

            if (isClicked == true)
            {
                LoadData(openFileDialog.FileName);
            }

            tbBankName.DataContext = _bank;
            spTimer.DataContext = _bank.Timer;
            tabCntrl.ItemsSource = _bank.Items;

            NextStepLoad();
        }
    }
}
