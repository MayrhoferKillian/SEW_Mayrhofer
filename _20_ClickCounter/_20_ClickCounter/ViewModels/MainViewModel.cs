using _20_ClickCounter.Helpers;
using _20_ClickCounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace _20_ClickCounter.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _currentId = 0;

        public ObservableCollection<LogEntry> LogEntries { get; set; }

        public ICommand LogClickCommand { get; }
        public ICommand ClearListCommand{ get; }

        public int TotalClickCount => LogEntries.Count;

        public MainViewModel()
        {
            LogEntries = new ObservableCollection<LogEntry>();
            LogEntries.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalClickCount));

            LogClickCommand = new RelayCommand(ExecuteLogClick);
            ClearListCommand = new RelayCommand(ExecuteClearList, CanExecuteClearList);
        }

        private void ExecuteLogClick(object parameter)
        {
            _currentId++;
            LogEntries.Add(new LogEntry
            {
                Id = _currentId,
                Timestamp = DateTime.Now
            });
        }

        private void ExecuteClearList(object parameter) 
        {
            LogEntries.Clear();
            _currentId = 0;
        }

        private bool CanExecuteClearList(object parameter)
        {
            return LogEntries.Count > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
