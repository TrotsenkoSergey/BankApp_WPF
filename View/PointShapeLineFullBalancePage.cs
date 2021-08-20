﻿using LiveCharts;
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

            Seriescollections = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<decimal> ()
                },
            };

            DataContext = this;
        }

        public SeriesCollection Seriescollections { get; set; }
    }
}
