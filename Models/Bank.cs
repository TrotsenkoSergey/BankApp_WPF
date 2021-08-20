using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Timer = new Timer();
            this.Items = new ObservableCollection<Department>();
            this.Name = name;
        }

        public void CreateDepartment(AttributeDepartment attribute)
        {
            Items.Add(new Department(attribute));
        }

        public void Remove(Department concreteDepartment)
        {
            Items.Remove(concreteDepartment);
        }
    }
}
