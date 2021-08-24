using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{
    public class InitialAccount : Account, INotifyPropertyChanged
    {

        public static event Action<decimal> NewBalance;

        public override string Name { get; set; } = "Initial";

        public override decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                HistoryOfBalance.Add(Math.Round(value, 2));
                NewBalance?.Invoke(value);
                OnPropertyChanged();
            }
        }

        public InitialAccount() : base()
        {
            
        }

        public void OnBalanceChanged(decimal changedAmount)
        {
            this.Balance += changedAmount;
        }
    }
}
