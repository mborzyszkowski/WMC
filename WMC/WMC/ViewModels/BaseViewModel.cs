﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WMC.Models;
using WMC.Services;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IWarehouseProductsService<Product> Warehouse => DependencyService.Get<IWarehouseProductsService<Product>>();
        public IAuthenticationService AuthenticationService => DependencyService.Get<IAuthenticationService>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string UserName => AuthenticationService.GetUserName();

        public string CurrencyStr => AuthenticationService.IsPlCurrency() ? "PLN" : "USD";

        public bool IsPlCurrency => AuthenticationService.IsPlCurrency();

        public bool IsUsCurrency => AuthenticationService.IsUsCurrency();

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
