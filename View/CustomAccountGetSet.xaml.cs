using System;
using System.Windows;

namespace BankApp_WPF.View
{
    
    public partial class CustomAccountGetSetWindow 
    {

        public decimal Amount { get; set; }

        public CustomAccountGetSetWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal amount = default;
            bool correctNum = Decimal.TryParse(tbAmount.Text, out amount);
            if (!correctNum)
            { MessageBox.Show("Incorrect amount, try again."); }
            else
            {
                Amount = amount;
                DialogResult = true;
                Close();
            }
        }
    }
}
