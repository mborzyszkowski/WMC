﻿using System;
using System.Diagnostics;
using WMC.Exceptions;
using WMC.Models;
using WMC.Views;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    [QueryProperty(nameof(ProductId), nameof(ProductId))]
    class EditProductViewModel : BaseViewModel
    {
        private long _productId;
        private string _manufacturerName;
        private string _modelName;
        private double _price;
        private double _priceUsd;

        public EditProductViewModel()
        {
            Title = "Edit product";

            UpdateCommand = new Command(OnUpdate, ValidateProduct);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => UpdateCommand.ChangeCanExecute();
        }

        public Command UpdateCommand { get; }
        public Command CancelCommand { get; }

        public bool ValidateProduct()
        {
            return !string.IsNullOrWhiteSpace(_manufacturerName)
                   && !string.IsNullOrWhiteSpace(_modelName)
                   && _price > 0;
        }
        public string ProductId
        {
            get => _productId.ToString();
            set
            {
                _productId = Int64.Parse(value);
                LoadProduct(_productId);
            }
        }
        public string ManufacturerName
        {
            get => _manufacturerName;
            set => SetProperty(ref _manufacturerName, value);
        }

        public string ModelName
        {
            get => _modelName;
            set => SetProperty(ref _modelName, value);
        }

        public double Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public double PriceUsd
        {
            get => _priceUsd;
            set => SetProperty(ref _priceUsd, value);
        }

        private async void LoadProduct(long productId)
        {
            try
            {
                var product = await Warehouse.GetProduct(productId);
                ManufacturerName = product.ManufacturerName;
                ModelName = product.ModelName;
                Price = product.Price;
                PriceUsd = product.PriceUsd ?? 0.0;
                OnPropertyChanged("Product");
            }
            catch (SyncRedirectException)
            {
                Application.Current.MainPage = new SyncProductsResultPage();
            }
            catch (ProductNotFoundException)
            {
                await Application.Current.MainPage.DisplayAlert("Not found", "Selected product no longer exists", "Ok");
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private async void OnUpdate()
        {
            var newProduct = new Product()
            {
                Id = _productId,
                ManufacturerName = _manufacturerName,
                ModelName = _modelName,
                Price = _price,
                PriceUsd = _priceUsd,
            };

            try
            {
                await Warehouse.UpdateProduct(newProduct);
                await Shell.Current.GoToAsync($"..");
            }
            catch (ProductNotFoundException)
            {
                await Application.Current.MainPage.DisplayAlert("Not found", "Selected product no longer exists", "Ok");
                Application.Current.MainPage = new AppShell();
            }
            catch (SyncRedirectException)
            {
                Application.Current.MainPage = new SyncProductsResultPage();
            }
        }
    }
}
