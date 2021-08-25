using System.Collections.ObjectModel;

namespace BankApp_WPF.Models
{
    public class Bank : IConstruct<Department>
    {

        private Timer timer;
        private ObservableCollection<Department> departments;

        public Timer Timer
        {
            get { return timer; }
            private set { timer = value; }
        }

        public ObservableCollection<Department> Items
        {
            get { return departments; }
            private set { departments = value; }
        }

        public string Name { get; set; }

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
