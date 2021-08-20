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
        
        protected string Name { get; private set; }

        public virtual decimal Balance { get; set; }
       

    }
}
