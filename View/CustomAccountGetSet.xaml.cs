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
            try
            {
                Amount = Convert.ToDecimal(tbAmount.Text);
                if (Amount <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                DialogResult = true;
                Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("The value must be greater than Zero");
            }
            catch (OverflowException) 
            {
                MessageBox.Show($"Values ​​greater than {decimal.MaxValue}\n" +
                                $"or less than {decimal.MinValue}\n" +
                                $"are not supported by current application.");
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect amount, try again.");
            }
        }
    }
}
