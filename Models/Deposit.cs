using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{

    public class Deposit : Account, INotifyPropertyChanged
    {

        private const decimal MONTHLY_RATE = 1.12m;
        private const string DEFAULT_NAME = "Deposit";

        public event Action<decimal> BalanceChanged;

        public Deposit(decimal amount) : base()
        {
            Name = DEFAULT_NAME;
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
