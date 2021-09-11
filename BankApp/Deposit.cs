using System;

namespace BankApp
{
    /// <summary>
    /// Deposit account.
    /// </summary>
    public class Deposit : Account
    {

        private const decimal MONTHLY_RATE = 1.12m;
        private const string DEFAULT_NAME = "Deposit";

        /// <summary>
        /// Balance changed event.
        /// </summary>
        public event Action<decimal, Account> BalanceChanged;

        /// <summary>
        /// Constructor of a new deposit entity.
        /// </summary>
        /// <param name="amount"></param>
        public Deposit(decimal amount) : base()
        {
            Name = DEFAULT_NAME;
            Balance = amount;
            Timer.AddMonths += OnTimer_NewTime;
        }

        /// <summary>
        /// Handles the time change event.
        /// </summary>
        /// <param name="monthsCount"></param>
        private void OnTimer_NewTime(int monthsCount)
        {
            decimal tempBalance = Balance;
            for (int i = 0; i < monthsCount; i++)
            {
                tempBalance *= MONTHLY_RATE;
            }
            BalanceChanged?.Invoke(tempBalance - Balance, this);
            Balance = tempBalance;
        }
    }
}
