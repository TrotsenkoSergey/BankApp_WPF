using System;
using System.Collections.ObjectModel;

namespace BankApp_WPF.Models
{
    /// <summary>
    /// Entity of bank department.
    /// </summary>
    public class Department : IConstruct<Customer>
    {
        private AttributeDepartment attribute;
        private ObservableCollection<Customer> customers;

        /// <summary>
        /// Department attribute.
        /// </summary>
        public AttributeDepartment Attribute
        {
            get { return attribute; }

        }

        /// <summary>
        /// Collection of customers.
        /// </summary>
        public ObservableCollection<Customer> Items
        {
            get { return customers; }
            private set { customers = value; }
        }

        /// <summary>
        /// Department name.
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// Department constructor.
        /// </summary>
        /// <param name="attribute"></param>
        public Department(AttributeDepartment attribute) 
        {
            customers = new ObservableCollection<Customer>();
            this.attribute = attribute;
            Name = Enum.GetName(typeof(AttributeDepartment), attribute);
        }

        /// <summary>
        /// Add new customer.
        /// </summary>
        /// <param name="name">customer name</param>
        /// <returns></returns>
        public Customer AddNewCustomer(string name)
        {
            Customer customer = new Customer(name);
            Items.Add(customer);
            return customer;
        }

        /// <summary>
        /// Remove concrete customer.
        /// </summary>
        /// <param name="concreteCustomer"></param>
        public void Remove(Customer concreteCustomer)
        {
            Items.Remove(concreteCustomer);
        }
    }
}
