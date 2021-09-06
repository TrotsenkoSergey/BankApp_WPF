using System.Windows;

namespace BankApp_WPF.View
{
    
    public partial class CreateNameForNewBankApp : Window
    {
        public CreateNameForNewBankApp()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
