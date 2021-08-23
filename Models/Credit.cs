using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{
    public class Credit : Account, INotifyPropertyChanged
    {

        private const decimal MONTHLY_RATE = 1.12m;
        public event Action<decimal> BalanceChanged;

        public override string Name { get; set; } = "Credit";

        public override decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                HistoryOfBalance.Add(value);
                OnPropertyChanged();
            }
        }

        public Credit(decimal amount) : base()
        {
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
