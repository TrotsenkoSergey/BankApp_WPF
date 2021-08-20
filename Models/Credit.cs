using System;

namespace BankApp_WPF.Models
{
    public class Credit : Account
    {

        private const decimal MONTHLY_RATE = 1.12m;
        public static event Action<decimal> BalanceChanged;

        public override decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                historyOfBalance.Add(value);
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
