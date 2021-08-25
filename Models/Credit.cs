using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{

    public class Credit : Account, INotifyPropertyChanged
    {

        private const decimal MONTHLY_RATE = 1.12m;
        private const string DEFAULT_NAME = "Credit";

        public event Action<decimal> BalanceChanged;

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

        public Credit(decimal amount) : base()
        {
            Name = DEFAULT_NAME;
            this.Balance = -amount;
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
