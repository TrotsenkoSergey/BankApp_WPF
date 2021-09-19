﻿using System;
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
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Extensions;

namespace BankApp_WPF
{
    /// <summary>
    /// Main App window.
    /// </summary>
    public partial class MainWindow
    {
        private Bank bank;
        private Dictionary<Department, TabItemDepartment> departmentsKey = new Dictionary<Department, TabItemDepartment>();

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
            CreateNewBankApp newWindow = new CreateNewBankApp();
            newWindow.ShowDialog();
            bank = new Bank(newWindow.tbBankName.Text);

            if ((bool)newWindow.checkBoxRandom.IsChecked)
            {
                bank.Name = "BANK_FOR_TESTING";

                var department = bank.CreateDepartment(AttributeDepartment.Persons);
                var tabItem = new TabItemDepartment(department);
                departmentsKey.Add(department, tabItem);
                departmentsKey[department].AddDefaultCustomer("FirstPerson_Name")
                          .GetCredit(1000m)
                          .AddNewDeposit(900m)
                          .IntroduceLogsDefaultCustomer(); //New Extensions property
                departmentsKey[department].AddDefaultCustomer("SecondPerson_Name")
                         .FundInitialAccount(1000m)
                         .AddNewDeposit(500m)
                         .AddNewDeposit(500m)
                         .IntroduceLogsDefaultCustomer(); //New Extensions property

                department = bank.CreateDepartment(AttributeDepartment.Organizations);
                tabItem = new TabItemDepartment(department);
                departmentsKey.Add(department, tabItem);
                departmentsKey[department].AddDefaultCustomer("ORGANIZATION")
                    .IntroduceLogsDefaultCustomer();
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
            if (tabCntrl.SelectedItem is Department department)
            {
                var selectedTabItem = departmentsKey[department];
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
                var department = bank.CreateDepartment((AttributeDepartment)type);
                if (!String.IsNullOrEmpty(departmentCreationWindow.tbDepartmentName.Text))
                {
                    department.Name = departmentCreationWindow.tbDepartmentName.Text;
                }
                var tabItem = new TabItemDepartment(department);
                departmentsKey.Add(department, tabItem);
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
                    departmentsKey.Remove(department);
                    bank.Remove(department);
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
                departmentsKey[department].AddCustomer_MenuMainWindowClick(this);
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
                departmentsKey[department].RemoveCustomer_MenuMainWindowClick();
            }
        }

        private void MakeInitialDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[department].MakeInitialDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        public void WithDrawInitialMoney_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[department].WithDrawInitialMoney_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void AddNewDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[department].AddNewDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void WithDrawDeposit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer &&
                departmentsKey[department].lbAccounts.SelectedItem is Deposit)
            {
                departmentsKey[department].WithDrawDeposit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete deposit account.");
            }
        }

        private void GetCredit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer)
            {
                departmentsKey[department].GetCredit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department and customer.");
            }
        }

        private void RepayCredit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer &&
                departmentsKey[department].lbAccounts.SelectedItem is Credit)
            {
                departmentsKey[department].RepayCredit_MenuItemClick(this);
            }
            else
            {
                MessageBox.Show("You must select the department, concrete customer and concrete credit account.");
            }
        }

        private void CloseAccount_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (tabCntrl.SelectedItem is Department department &&
                departmentsKey[department].lbCustomers.SelectedItem is Customer &&
                departmentsKey[department].lbAccounts.SelectedItem is Account)
            {
                departmentsKey[department].CloseAccount_MenuItemClick();
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

        private async void Save_Click(object o, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Dialog Save window Json Bank repository";
            saveFileDialog.Filter = "Json files (*.json)|*.json";
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (saveFileDialog.ShowDialog() == true)
            {
                //var settings = new JsonSerializerSettings
                //{
                //    TypeNameHandling = TypeNameHandling.Objects,
                //    Formatting = Formatting.Indented,
                //    ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                //};

                //var taskBank = Task.Run(() => JsonConvert.SerializeObject(bank, settings));
                //var taskLogs = Task.Run(() => JsonConvert.SerializeObject(CustomerExtension.LogsRepository, settings));

                //string jsonBank = await taskBank;
                //string stringLogs = await taskLogs;

                //string fileBankName = Path.Combine(saveFileDialog.InitialDirectory, saveFileDialog.FileName);
                //await Task.Run(() => File.WriteAllText(fileBankName, jsonBank));

                //string fileLogsName = Path.Combine(saveFileDialog.InitialDirectory, "LOGS.json");

                //await Task.Run(() => File.WriteAllText(fileLogsName, stringLogs));

                string pathToSave = Path.Combine(saveFileDialog.InitialDirectory, saveFileDialog.FileName);
                var serializer = new DataSerializer();
                serializer.JsonSerialize(bank, pathToSave);
            }
        }

        private async void Load_Click(object s, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Dialog Load window Json Bank repository";
            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (openFileDialog.ShowDialog() == true)
            {
                //var settings = new JsonSerializerSettings
                //{
                //    TypeNameHandling = TypeNameHandling.Objects,
                //    Formatting = Formatting.Indented,
                //    //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                //};

                //var jsonBank = await Task.Run(() => File.ReadAllText(openFileDialog.FileName));
                //bank = JsonConvert.DeserializeObject<Bank>(jsonBank, settings);

                //var fileLogsName = Path.Combine(openFileDialog.InitialDirectory, "LOGS.json");
                //var jsonLogs = await Task.Run(() => File.ReadAllText(fileLogsName));
                //CustomerExtension.LogsRepository = JsonConvert.DeserializeObject<Dictionary<string, RepLogs>>(jsonLogs, settings);
                var deserializer = new DataSerializer();
                bank = await deserializer.JsonDeserialize(openFileDialog.FileName);
            }

            tbBankName.DataContext = bank;
            spTimer.DataContext = bank.Timer;
            tabCntrl.ItemsSource = bank.Items;

            if (bank.Items.Count != 0)
                foreach (var department in bank.Items)
                {
                    var tabItem = new TabItemDepartment(department);
                    departmentsKey.Add(department, tabItem);

                    if (department.Items.Count != 0)
                        foreach (var customer in department.Items)
                        {
                            var customerBalanceGraphPage = new GraphFrame();
                            customer.InitialAccount.NewBalance += customerBalanceGraphPage.InitialAccount_NewBalance;
                            departmentsKey[department].CustomerKey.Add(customer, customerBalanceGraphPage);
                        }
                }


        }
    }
}
