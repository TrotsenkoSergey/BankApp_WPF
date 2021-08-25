using System.Collections.ObjectModel;

namespace BankApp_WPF.Models
{

    public interface IConstruct<T>
    {

        ObservableCollection<T> Items { get; }

        string Name { get; set; }

        void Remove(T item);
    }
}
