using System.Windows;

namespace BankApp_WPF.View
{
   
    public partial class CustomerCreationWindow : Window
    {
        public CustomerCreationWindow()
        {
            InitializeComponent();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
