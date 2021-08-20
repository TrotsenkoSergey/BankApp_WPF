using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_WPF.Models
{
    public interface IConstruct<T>
    {
        ObservableCollection<T> Items { get; }
        string Name { get; set; }
        void Remove(T item);
    }
}
