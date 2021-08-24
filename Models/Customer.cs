using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp_WPF.Models
{
    public class Customer : IConstruct<Account>, ICredit, IDeposit, INotifyPropertyChanged
    {
        private ObservableCollection<Account> accounts;
        private InitialAccount initialAccount;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal InitialBalance
        {
            get { return initialAccount.Balance; }
            private set
            {
                initialAccount.Balance = value;
                OnPropertyChanged();
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
                (concreteAccount as Credit).BalanceChanged -= initialAccount.OnBalanceChanged;
                this.Items.Remove(concreteAccount);
            }
            else if (concreteAccount is Deposit)
            {
                InitialBalance += (concreteAccount as Deposit).Balance;
                (concreteAccount as Deposit).BalanceChanged -= initialAccount.OnBalanceChanged;
                this.Items.Remove(concreteAccount);
            }
        }

        public void GetCredit(decimal amount)
        {
            Credit credit = new Credit(amount);
            Items.Add(credit);
            InitialBalance += amount;
            credit.BalanceChanged += initialAccount.OnBalanceChanged;
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
                Deposit deposit = new Deposit(amount);
                Items.Add(deposit);
                InitialBalance -= amount;
                deposit.BalanceChanged += initialAccount.OnBalanceChanged;
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
