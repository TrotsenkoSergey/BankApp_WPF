using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{

    class Deposit : Account, INotifyPropertyChanged
    {

        private const decimal MONTHLY_RATE = 1.12m;

        public event Action<decimal> BalanceChanged;

        public override string Name { get; set; } = "Deposit";

        public override decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                HistoryOfBalance.Add(Math.Round(value, 2));
                OnPropertyChanged();
            }
        }

        public Deposit(decimal amount) : base()
        {
            this.Balance = amount;
            Timer.AddMonths += OnTimer_NewTime;
        }

        private void OnTimer_NewTime(int monthsCount)
        {
            decimal tempBalance = Balance;
            for (int i = 0; i < monthsCount; i++)
            {
                tempBalance *= MONTHLY_RATE;
            }
            BalanceChanged?.Invoke(tempBalance - Balance);
            Balance = tempBalance;
        }

    }
}
