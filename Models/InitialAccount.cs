﻿using System;
using System.ComponentModel;

namespace BankApp_WPF.Models
{
    /// <summary>
    /// Initial account.
    /// </summary>
    public class InitialAccount : Account, INotifyPropertyChanged
    {
        private const string DEFAULT_NAME = "Initial";
       
        public event Action<decimal> NewBalance;

        public override decimal Balance
        {
            get { return balance; }
            protected set
            {
                balance = value;
                HistoryOfBalance.Add(Math.Round(value, 2));
                NewBalance?.Invoke(value);
                OnPropertyChanged(nameof(Balance));
            }
        }

        public InitialAccount() : base()
        {
            Name = DEFAULT_NAME;
        }
    }
}
