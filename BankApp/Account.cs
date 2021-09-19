using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace BankApp
{
    /// <summary>
    /// Implements the essence of a bank account.
    /// </summary>
    public abstract class Account : INotifyPropertyChanged
    {
        [JsonInclude]
        private protected decimal balance;
        [JsonInclude]
        private protected List<decimal> historyOfBalance = new List<decimal>();

        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Name.
        /// </summary>
        public virtual string Name { get; set; }

        [JsonInclude]
        /// <summary>
        /// Balance.
        /// </summary>
        public virtual decimal Balance
        {
            get => balance;
            private protected set
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
        /// <param name="account"></param>
        public virtual void OnBalanceChanged(decimal changedAmount, Account account)
        {
            Balance += changedAmount;
        }

        [JsonInclude]
        /// <summary>
        /// Collection of balance change values.
        /// </summary>
        public virtual List<decimal> HistoryOfBalance
        {
            get => historyOfBalance;
            set
            {
                historyOfBalance = value;
                OnPropertyChanged(nameof(HistoryOfBalance));
            }
        }

        private protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
