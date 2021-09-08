using System.Collections.ObjectModel;

namespace BankApp
{
    /// <summary>
    /// Implement the essence of a bank.
    /// </summary>
    public class Bank : IConstruct<Department>
    {
        /// <summary>
        /// Timer.
        /// </summary>
        public Timer Timer { get; private set; }

        /// <summary>
        /// Departments collection.
        /// </summary>
        public ObservableCollection<Department> Items { get; private set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a bank and a new entity of time.
        /// </summary>
        public Bank()
        {
            Timer = new Timer();
            Items = new ObservableCollection<Department>();
        }

        /// <summary>
        /// Creates a bank and a new entity of time.
        /// </summary>
        /// <param name="name"></param>
        public Bank(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Create concrete department.
        /// </summary>
        /// <param name="attribute">department attribute</param>
        /// <returns></returns>
        public Department CreateDepartment(AttributeDepartment attribute)
        {
            Department department = new Department(attribute);
            Items.Add(department);
            return department;
        }

        /// <summary>
        /// Remove concrete department.
        /// </summary>
        /// <param name="concreteDepartment"></param>
        public void Remove(Department concreteDepartment)
        {
            Items.Remove(concreteDepartment);
        }
    }
}
