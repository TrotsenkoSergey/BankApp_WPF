﻿using System;
using System.Collections.ObjectModel;

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
            Name = Enum.GetName(typeof(AttributeDepartment), attribute);
        }

        public Customer AddNewCustomer(string name)
        {
            Customer customer = new Customer(name);
            Items.Add(customer);
            return customer;
        }

        public void Remove(Customer concreteCustomer)
        {
            Items.Remove(concreteCustomer);
        }
    }
}
