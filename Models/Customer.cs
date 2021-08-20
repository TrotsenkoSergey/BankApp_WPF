using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_WPF.Models
{
    public class Customer : IConstruct<Account>, ICredit, IDeposit
    {
        private ObservableCollection<Account> accounts;
        private decimal balance;

        public decimal InitialBalance
        {
            get { return balance; }
            private set
            {
                balance = value;
            }
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
            Credit.BalanceChanged += OnBalanceChanged;
            Deposit.BalanceChanged += OnBalanceChanged;
        }

        private void OnBalanceChanged(decimal changedAmount)
        {
            this.InitialBalance += changedAmount;
        }

        public void DepositeMoney(decimal amount)
        {
            this.InitialBalance += amount;
        }

        public void WithDraw(decimal amount)
        {
            if (InitialBalance >= amount)
            {
                this.InitialBalance -= amount;
            }
        }

        public void Remove(Account concreteAccount)
        {
            if (concreteAccount is Credit && InitialBalance >= -(concreteAccount as Credit).Balance)
            {
                InitialBalance += (concreteAccount as Credit).Balance;
                this.Items.Remove(concreteAccount);
            }
            else if (concreteAccount is Deposit)
            {
                InitialBalance += (concreteAccount as Deposit).Balance;
                this.Items.Remove(concreteAccount);
            }
        }

        public void GetCredit(decimal amount)
        {
            Items.Add(new Credit(amount));
            InitialBalance += amount;
        }

        public void RepayLoan(object concreteCredit, decimal amount)
        {
            if (InitialBalance >= amount)
            {
                InitialBalance -= amount;
                (concreteCredit as Credit).Balance += amount;
            }
        }

        public void WithDraw(object concreteDeposit, decimal amount)
        {
            if ((concreteDeposit as Deposit).Balance >= amount)
            {
                InitialBalance += amount;
                (concreteDeposit as Deposit).Balance -= amount;
            }
        }

        public void AddNewDeposit(decimal amount)
        {
            if (InitialBalance >= amount)
            { 
                Items.Add(new Deposit(amount));
                InitialBalance -= amount;
            }
        }

        public void AddAmountExistingDeposit(object concreteDeposit, decimal amount)
        {
            if (InitialBalance >= amount)
            {
                (concreteDeposit as Deposit).Balance += amount;
                InitialBalance -= amount;
            }
        }
    }
}
