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

        public TabItemPage(Department department)
        {
            InitializeComponent();
            this.department = department;
            lbCustomer.ItemsSource = this.department.Items;
        }

        private void lbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var fullBalanceGraphPage = new PointShapeLineFullBalancePage();
            FrameFullBalanceGraph.Content = fullBalanceGraphPage;
            lbAccounts.ItemsSource = (lbCustomer.SelectedItem as Customer).Items;
            department.Items[0].GetCredit(2000m);
            department.Items[0].AddNewDeposit(2000m);
            
            var initial = (lbCustomer.SelectedItem as Customer).Items[0] as InitialAccount;
            var credit = (lbCustomer.SelectedItem as Customer).Items[1] as Credit;
            var deposit = (lbCustomer.SelectedItem as Customer).Items[2] as Deposit;

            credit.BalanceChanged += initial.OnBalanceChanged;
            deposit.BalanceChanged += initial.OnBalanceChanged;
        }
                
    }
}
