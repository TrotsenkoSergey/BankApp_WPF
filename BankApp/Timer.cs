using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp
{
    /// <summary>
    /// Represents a time scrolling prototype.
    /// </summary>
    public class Timer : INotifyPropertyChanged
    {
        /// <summary>
        /// Time change event.
        /// </summary>
        public static event Action<int> AddMonths;

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private int currentYears;
        private int currentMonths;

        private const int MAX_MONTHS = 12;

        /// <summary>
        /// Information about the value of the previous month.
        /// </summary>
        public int OldMonths { get; private set; }

        /// <summary>
        /// Information about the value of the previous year.
        /// </summary>
        public int OldYears { get; private set; }

        /// <summary>
        /// Information about the value of the current month.
        /// </summary>
        public int CurrentMonths
        {
            get => currentMonths;
            private set
            {
                currentMonths = value;
                OnPropertyChanged(nameof(CurrentMonths));
            }
        }

        /// <summary>
        /// Information about the value of the current year.
        /// </summary>
        public int CurrentYears
        {
            get => currentYears;
            private set
            {
                currentYears = value;
                OnPropertyChanged(nameof(CurrentYears));
            }
        }

        /// <summary>
        /// Scroll the time by a certain value.
        /// </summary>
        /// <param name="months"></param>
        /// <param name="years"></param>
        public void NextTime(int months, int years = default)
        {
            int addmonths = years * 12 + months;
            OldMonths = CurrentMonths;
            OldYears = CurrentYears;

            if (CurrentMonths + months >= MAX_MONTHS)
            {
                CurrentYears += (CurrentMonths + months) / MAX_MONTHS + years;
                CurrentMonths = (CurrentMonths + months) % MAX_MONTHS;
            }
            else
            {
                CurrentYears += years;
                CurrentMonths += months;
            }
            AddMonths?.Invoke(addmonths);
        }

        private protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
