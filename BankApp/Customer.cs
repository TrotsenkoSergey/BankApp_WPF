using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp
{
    /// <summary>
    /// Bank client entity.
    /// </summary>
    public class Customer : IConstruct<Account>, ICredit, IDeposit, INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Access to the main (initial) account.
        /// </summary>
        public InitialAccount InitialAccount { get; private set; }

        /// <summary>
        /// Balance of the main (initial) account.
        /// </summary>
        public decimal InitialBalance
        {
            get => InitialAccount.Balance;
            private set
            {
                InitialAccount.OnBalanceChanged(value, InitialAccount);
                OnPropertyChanged(nameof(InitialBalance));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Accounts collection.
        /// </summary>
        public ObservableCollection<Account> Items { get; private set; }

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
            Name = name;
            Items = new ObservableCollection<Account>();
            InitialAccount = new InitialAccount();
            Items.Add(InitialAccount);
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
                InitialBalance = amount;
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
                InitialBalance = -amount;
            }
            return this;
        }

        /// <summary>
        /// Remove concrete account.
        /// </summary>
        /// <param name="concreteAccount"></param>
        public void Remove(Account concreteAccount)
        {
            if (concreteAccount is Credit accountCredit && InitialBalance >= -accountCredit.Balance)
            {
                InitialBalance = accountCredit.Balance;
                accountCredit.BalanceChanged -= InitialAccount.OnBalanceChanged;
                this.Items.Remove(accountCredit);
            }
            else if (concreteAccount is Deposit accountDeposit)
            {
                InitialBalance = accountDeposit.Balance;
                accountDeposit.BalanceChanged -= InitialAccount.OnBalanceChanged;
                this.Items.Remove(accountDeposit);
            }
            else // concreteAccount is InitialAccount
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
                credit.BalanceChanged += InitialAccount.OnBalanceChanged;
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
            var credit = concreteCredit as Credit;
            if (InitialBalance >= amount && amount > 0)
            {
                credit.OnBalanceChanged(amount, credit);
                InitialBalance = -amount;
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
            var deposit = concreteDeposit as Deposit;
            if (amount > 0 && deposit.Balance >= amount)
            {
                deposit.OnBalanceChanged(-amount, deposit);
                InitialBalance = amount;
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
                deposit.BalanceChanged += InitialAccount.OnBalanceChanged;
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
            var deposit = concreteDeposit as Deposit;
            if (amount > 0 && InitialBalance >= amount)
            {
                deposit.OnBalanceChanged(amount, deposit);
                InitialBalance = -amount;
            }
            return this;
        }
    }
}
