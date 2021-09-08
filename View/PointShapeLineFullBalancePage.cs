using LiveCharts;
using LiveCharts.Wpf;
using System;

namespace BankApp_WPF.View
{

    public partial class GraphFrame
    {

        //private Customer customer;
        //private Action<decimal> initAccountNewBalace;

        public GraphFrame()
        {

            InitializeComponent();
            //this.customer = customer;

            //this.initAccountNewBalace += InitialAccount_NewBalance;


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
            //this.customer.InitialAccount.NewBalance += InitialAccount_NewBalance;

        }

        public void InitialAccount_NewBalance(decimal newBalance)
        {
            SeriesCollections[0].Values.Add(newBalance);
        }

        //public string[] Labels { get; set; }

        public Func<decimal, string> YFormatter { get; set; }

        public SeriesCollection SeriesCollections { get; set; }
    }
}
