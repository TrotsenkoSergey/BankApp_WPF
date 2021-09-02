using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp_WPF.Models
{
    /// <summary>
    /// Bank client entity.
    /// </summary>
    public class Customer : IConstruct<Account>, ICredit, IDeposit, INotifyPropertyChanged
    {
        private ObservableCollection<Account> accounts;
        private InitialAccount initialAccount;

        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Access to the main (initial) account.
        /// </summary>
        public InitialAccount InitialAccount { get { return initialAccount; } }

        /// <summary>
        /// Balance of the main (initial) account.
        /// </summary>
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

        /// <summary>
        /// Accounts collection.
        /// </summary>
        public ObservableCollection<Account> Items
        {
            get { return accounts; }
            private set { accounts = value; }
        }

        /// <summary>
        /// Customer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bank client constructor with opening a main (initial) account.
        /// </summary>
        /// <param name="name"></param>
        public Customer(string name)
        {
            this.Name = name;
            accounts = new ObservableCollection<Account>();
            initialAccount = new InitialAccount();
            accounts.Add(initialAccount);
        }

        /// <summary>
        /// Fund your main (initial) account.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Customer FundInitialAccount(decimal amount)
        {
            if (amount > 0)
            {
                this.InitialBalance = amount;
            }
            return this;
        }

        /// <summary>
        /// Withdraw money from the main (initial) account.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Customer WithDrawInitialAccount(decimal amount)
        {
            if (amount > 0 && InitialBalance >= amount)
            {
                this.InitialBalance = -amount;
            }
            return this;
        }

        /// <summary>
        /// Remove concrete account.
        /// </summary>
        /// <param name="concreteAccount"></param>
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

        /// <summary>
        /// Open credit account and get money on initial account.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Repay concrete loan.
        /// </summary>
        /// <param name="concreteCredit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Customer RepayLoan(object concreteCredit, decimal amount)
        {
            if (InitialBalance >= amount && amount > 0)
            {
                (concreteCredit as Credit).OnBalanceChanged(amount);
                this.InitialBalance = -amount;
            }
            return this;
        }

        /// <summary>
        /// Withdraw concrete deposit.
        /// </summary>
        /// <param name="concreteDeposit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Customer WithDrawDeposit(object concreteDeposit, decimal amount)
        {
            if (amount > 0 && (concreteDeposit as Deposit).Balance >= amount)
            {
                (concreteDeposit as Deposit).OnBalanceChanged(-amount);
                this.InitialBalance = amount;
            }
            return this;
        }

        /// <summary>
        /// Open new deposit account.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Top up an existing deposit.
        /// </summary>
        /// <param name="concreteDeposit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
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
