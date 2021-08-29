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
using System.Windows.Shapes;

namespace BankApp_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для GetSetDepositCredit.xaml
    /// </summary>
    public partial class GetSetDepositCredit : Window
    {
        public decimal Amount { get; set; }
        public GetSetDepositCredit()
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
