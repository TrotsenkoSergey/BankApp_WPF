using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_WPF.Models
{
    public abstract class Account 
       
    {
        protected decimal balance;
        protected List<decimal> historyOfBalance = new List<decimal>();
        
        protected string Name { get; private set; }

        public virtual decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                historyOfBalance.Add(value);
            }
        }
       
        public virtual List<decimal> HistoryOfBalance { get { return historyOfBalance; } }

    }
}
