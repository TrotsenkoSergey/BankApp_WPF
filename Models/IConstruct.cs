using System.Collections.ObjectModel;

namespace BankApp_WPF.Models
{

    /// <summary>
    /// Provides the ability to construct nested objects.
    /// </summary>
    /// <typeparam name="T">Nested class.</typeparam>
    public interface IConstruct<T>
    {
        /// <summary>
        /// Nested class collection property.
        /// </summary>
        ObservableCollection<T> Items { get; }

        /// <summary>
        /// Name of concrete constraction.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Remove nested item.
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);
    }
}
