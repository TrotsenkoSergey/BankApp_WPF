﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using BankApp_WPF.View;
using BankApp;
using CustomerExtensions;

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
                          .ToIntroduceNewMethodForGettingLogs();
                departmentsKey[department].AddDefaultCustomer("SecondPerson_Name")
                         .FundInitialAccount(1000m)
                         .AddNewDeposit(500m)
                         .AddNewDeposit(500m)
                         .ToIntroduceNewMethodForGettingLogs(); 

                department = bank.CreateDepartment(AttributeDepartment.Organizations);
                tabItem = new TabItemDepartment(department);
                departmentsKey.Add(department, tabItem);
                departmentsKey[department].AddDefaultCustomer("ORGANIZATION")
                    .ToIntroduceNewMethodForGettingLogs(); 
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
            bool isCorrectValue= (bool)departmentCreationWindow.ShowDialog();
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
            if (tabCntrl.SelectedItem is Department  department &&
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
        /// Calls IO tools to Load file bank.json.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Dialog window Json repository";
            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == true)
            {
                string json = System.IO.File.ReadAllText(openFileDialog.FileName);
                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                };
                bank = Newtonsoft.Json.JsonConvert.DeserializeObject<Bank>(json, settings);

                tbBankName.DataContext = bank;
                spTimer.DataContext = bank.Timer;
                tabCntrl.ItemsSource = bank.Items;
            }
        }

        /// <summary>
        /// Calls IO tools to Save bank.json file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Dialog window Json repository";
            saveFileDialog.Filter = "Json files (*.json)|*.json";
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (saveFileDialog.ShowDialog() == true)
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(bank, settings);

                string fileName = Path.Combine(saveFileDialog.InitialDirectory, saveFileDialog.FileName);

                System.IO.File.WriteAllText(fileName, json);
            }
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

    }
}
