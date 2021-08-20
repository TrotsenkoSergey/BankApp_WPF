using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_WPF.Models
{
    public class InitialAccount : Account
    {
        public override decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                historyOfBalance.Add(value);
            }
        }
        public InitialAccount() : base()
        {
            Timer.AddMonths += OnTimer_NewTime;
            Credit.BalanceChanged += OnBalanceChanged;
            Deposit.BalanceChanged += OnBalanceChanged;
        }

        private void OnBalanceChanged(decimal changedAmount)
        {
            this.Balance += changedAmount;
        }

        private void OnTimer_NewTime(int obj)
        {
            
        }
    }
}
