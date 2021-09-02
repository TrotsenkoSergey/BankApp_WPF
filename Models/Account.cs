using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp_WPF.Models
{

    /// <summary>
    /// Implements the essence of a bank account.
    /// </summary>
    public abstract class Account : INotifyPropertyChanged
    {
        protected private decimal balance;
        protected private List<decimal> historyOfBalance = new List<decimal>();

        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Balance.
        /// </summary>
        public virtual decimal Balance
        {
            get { return balance; }
            protected private set
            {
                balance = value;
                HistoryOfBalance.Add(Math.Round(value, 2));
                OnPropertyChanged(nameof(Balance));
            }
        }

        /// <summary>
        /// Changes the current balance by a certain value.
        /// </summary>
        /// <param name="changedAmount"></param>
        public virtual void OnBalanceChanged(decimal changedAmount)
        {
            this.Balance += changedAmount;
        }

        /// <summary>
        /// Collection of balance change values.
        /// </summary>
        public virtual List<decimal> HistoryOfBalance
        {
            get { return historyOfBalance; }
            private set
            {
                historyOfBalance = value;
                OnPropertyChanged(nameof(this.HistoryOfBalance));
            }
        }

        protected private virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
