using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankApp_WPF.Models
{
    public class Timer : INotifyPropertyChanged
    {

        public static event Action<int> AddMonths;
        public event PropertyChangedEventHandler PropertyChanged;
        private int currentYears;
        private int currentMonths;

        private const int MAX_MONTHS = 12;

        public int OldMonths { get; private set; }

        public int OldYears { get; private set; }

        public int CurrentMonths
        {
            get { return currentMonths; }
            private set
            {
                currentMonths = value;
                OnPropertyChanged(nameof(CurrentMonths));
            }
        }

        public int CurrentYears
        {
            get { return currentYears; }
            private set
            {
                currentYears = value;
                OnPropertyChanged(nameof(CurrentYears));
            }
        }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
