using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{
    /// <summary>
    /// Initial (main) account.
    /// </summary>
    public class InitialAccount : Account, INotifyPropertyChanged
    {
        private const string DEFAULT_NAME = "Initial";

        /// <summary>
        /// New balance event.
        /// </summary>
        public event Action<decimal> NewBalance;

        /// <summary>
        /// Balance.
        /// </summary>
        public override decimal Balance
        {
            get { return balance; }
            protected private set
            {
                balance = value;
                HistoryOfBalance.Add(Math.Round(value, 2));
                NewBalance?.Invoke(value);
                OnPropertyChanged(nameof(Balance));
            }
        }

        /// <summary>
        /// Constructor of a new initial account entity.
        /// </summary>
        public InitialAccount() : base()
        {
            Name = DEFAULT_NAME;
        }
    }
}
