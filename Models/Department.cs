using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_WPF.Models
{
    public class Department : IConstruct<Customer>
    {
        private AttributeDepartment attribute;
        private ObservableCollection<Customer> customers;

        public AttributeDepartment Attribute
        {
            get { return attribute; }

        }

        public ObservableCollection<Customer> Items
        {
            get { return customers; }
            private set { customers = value; }
        }

        public string Name { get; set; }

        public Department(AttributeDepartment attribute) 
        {
            customers = new ObservableCollection<Customer>();
            this.attribute = attribute;
        }

        public void AddNewCustomer(string name)
        {
            Items.Add(new Customer(name));
        }

        public void Remove(Customer concreteCustomer)
        {
            Items.Remove(concreteCustomer);
        }
    }
}
