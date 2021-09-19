using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BankApp
{
    /// <summary>
    /// Entity of bank department.
    /// </summary>
    public class Department : IConstruct<Customer>
    {
        /// <summary>
        /// Department name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Department attribute.
        /// </summary>
        public AttributeDepartment Attribute { get; private set; }

        [JsonPropertyName("customers")]
        /// <summary>
        /// Collection of customers.
        /// </summary>
        public ObservableCollection<Customer> Items { get; private set; }

        /// <summary>
        /// Department constructor.
        /// </summary>
        /// <param name="attribute"></param>
        public Department(AttributeDepartment attribute)
        {
            Items = new ObservableCollection<Customer>();
            Attribute = attribute;
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
