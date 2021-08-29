using System.Collections.ObjectModel;

namespace BankApp_WPF.Models
{
    /// <summary>
    /// Implement the essence of a bank.
    /// </summary>
    public class Bank : IConstruct<Department>
    {

        private Timer timer;
        private ObservableCollection<Department> departments;

        /// <summary>
        /// Timer.
        /// </summary>
        public Timer Timer
        {
            get { return timer; }
            private set { timer = value; }
        }

        /// <summary>
        /// Departments collection.
        /// </summary>
        public ObservableCollection<Department> Items
        {
            get { return departments; }
            private set { departments = value; }
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public Bank(string name)
        {
            Timer = new Timer();
            Items = new ObservableCollection<Department>();
            Name = name;
        }

        public Department CreateDepartment(AttributeDepartment attribute)
        {
            Department department = new Department(attribute);
            Items.Add(department);
            return department;
        }

        public void Remove(Department concreteDepartment)
        {
            Items.Remove(concreteDepartment);
        }
    }
}
