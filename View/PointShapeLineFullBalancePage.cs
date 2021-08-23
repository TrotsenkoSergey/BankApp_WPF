using BankApp_WPF.Models;
using LiveCharts;
using LiveCharts.Wpf;
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
    
    public partial class PointShapeLineFullBalancePage : Page
    {
         
        public PointShapeLineFullBalancePage()
        {

            InitializeComponent();

            SeriesCollections = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<decimal> ()
                },
            };

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");
            DataContext = this;
            InitialAccount.NewBalance += InitialAccount_NewBalance;
        }

        private void InitialAccount_NewBalance(decimal newBalance)
        {
            SeriesCollections[0].Values.Add(newBalance);
        }

        //public string[] Labels { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public SeriesCollection SeriesCollections { get; set; }
    }
}
