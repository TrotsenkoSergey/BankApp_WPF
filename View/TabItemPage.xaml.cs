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
            if ((lbCustomer.SelectedItem as Customer).Items[0].HistoryOfBalance.Count != 0)
            {
                //fullBalanceGraphPage.SeriesCollections[0].Values = (lbCustomer.SelectedItem as Customer).Items[0].HistoryOfBalance as IChartValues;
                foreach (var item in (lbCustomer.SelectedItem as Customer).Items[0].HistoryOfBalance)
                {
                    fullBalanceGraphPage.SeriesCollections[0].Values.Add(item);
                }
            }
        }
                
    }
}
