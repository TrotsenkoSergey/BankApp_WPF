using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp_WPF.Models
{
    public abstract class Account : INotifyPropertyChanged

    {
        protected decimal balance;
        protected List<decimal> historyOfBalance = new List<decimal>();

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string Name { get; set; }

        public virtual decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                HistoryOfBalance.Add(Math.Round(value, 2));
            }
        }

        public virtual List<decimal> HistoryOfBalance
        {
            get { return historyOfBalance; }
            private set
            {
                historyOfBalance = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
