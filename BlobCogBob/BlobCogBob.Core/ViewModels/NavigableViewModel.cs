using System;
using CodeMill.VMFirstNav;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace BlobCogBob.Core
{
    public abstract class NavigableViewModel : IViewModel, INotifyPropertyChanged
    {
        string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        bool _isBusy;
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyname = "",
                                      Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
