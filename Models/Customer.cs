﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp_WPF.Models
{
    public class Customer : IConstruct<Account>, ICredit, IDeposit, INotifyPropertyChanged
    {
        private ObservableCollection<Account> accounts;
        private InitialAccount initialAccount;

        public event PropertyChangedEventHandler PropertyChanged;

        public InitialAccount InitialAccount { get { return initialAccount; } }

        public decimal InitialBalance
        {
            get { return initialAccount.Balance; }
            private set
            {
                initialAccount.OnBalanceChanged(value);
                OnPropertyChanged(nameof(InitialBalance));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Account> Items
        {
            get { return accounts; }
            private set { accounts = value; }
        }

        public string Name { get; set; }

        public Customer(string name)
        {
            this.Name = name;
            accounts = new ObservableCollection<Account>();
            initialAccount = new InitialAccount();
            accounts.Add(initialAccount);
        }

        public Customer DepositeInitialMoney(decimal amount)
        {
            if (amount > 0)
            {
                this.InitialBalance = amount;
            }
            return this;
        }

        public Customer WithDrawInitialMoney(decimal amount)
        {
            if (amount > 0 && InitialBalance >= amount)
            {
                this.InitialBalance = -amount;
            }
            return this;
        }

        public void Remove(Account concreteAccount)
        {
            if (concreteAccount is Credit && InitialBalance >= -(concreteAccount as Credit).Balance)
            {
                InitialBalance = (concreteAccount as Credit).Balance;
                (concreteAccount as Credit).BalanceChanged -= initialAccount.OnBalanceChanged;
                this.Items.Remove(concreteAccount);
            }
            else if (concreteAccount is Deposit)
            {
                InitialBalance = (concreteAccount as Deposit).Balance;
                (concreteAccount as Deposit).BalanceChanged -= initialAccount.OnBalanceChanged;
                this.Items.Remove(concreteAccount);
            }
            else
            {
                this.Items.Remove(concreteAccount);
            }
        }

        public Customer GetCredit(decimal amount)
        {
            if (amount > 0)
            {
                Credit credit = new Credit(amount);
                Items.Add(credit);
                InitialBalance = amount;
                credit.BalanceChanged += initialAccount.OnBalanceChanged;
            }
            return this;
        }

        public Customer RepayLoan(object concreteCredit, decimal amount)
        {
            if (InitialBalance >= amount && amount > 0)
            {
                (concreteCredit as Credit).OnBalanceChanged(amount);
                this.InitialBalance = -amount;
            }
            return this;
        }

        public Customer WithDrawDeposit(object concreteDeposit, decimal amount)
        {
            if (amount > 0 && (concreteDeposit as Deposit).Balance >= amount)
            {
                (concreteDeposit as Deposit).OnBalanceChanged(-amount);
                this.InitialBalance = amount;
            }
            return this;
        }

        public Customer AddNewDeposit(decimal amount)
        {
            if (amount > 0 && InitialBalance >= amount)
            {
                Deposit deposit = new Deposit(amount);
                Items.Add(deposit);
                InitialBalance = -amount;
                deposit.BalanceChanged += initialAccount.OnBalanceChanged;
            }
            return this;
        }

        public Customer AddAmountExistingDeposit(object concreteDeposit, decimal amount)
        {
            if (amount > 0 && InitialBalance >= amount)
            {
                (concreteDeposit as Deposit).OnBalanceChanged(amount);
                InitialBalance = -amount;
            }
            return this;
        }
    }
}
