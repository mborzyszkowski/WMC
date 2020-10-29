using System;
using System.Diagnostics;
using WMC.Models;
using WMC.Views;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    [QueryProperty(nameof(ProductId), nameof(ProductId))]
    public class ProductDetailsViewModel : BaseViewModel
    {
        public Product Product { get; private set; }
        private long _productId;
        private long _increaseQuantityNumber;
        private long _decreaseQuantityNumber;

        public ProductDetailsViewModel()
        {
            Title = "Product details";

            EditProductCommand = new Command(OnEditProduct);
            DeleteProductCommand = new Command(OnDeleteProduct, () => AuthenticationService.IsManager());
            IncreaseQuantityCommand = new Command(OnIncreaseQuantity, ValidateIncreaseQuantity);
            DecreaseQuantityCommand = new Command(OnDecreaseQuantity, ValidateDecreaseQuantity);
            PropertyChanged += (_, __) =>
            {
                IncreaseQuantityCommand.ChangeCanExecute();
                DecreaseQuantityCommand.ChangeCanExecute();
            };
        }
        public Command EditProductCommand { get; }
        public Command DeleteProductCommand { get; }
        public Command IncreaseQuantityCommand { get; }
        public Command DecreaseQuantityCommand { get; }

        async void OnEditProduct()
        {
            await Shell.Current.GoToAsync($"{nameof(EditProductPage)}?{nameof(EditProductViewModel.ProductId)}={Product.Id}");
        }

        async void OnDeleteProduct()
        {
            var deletionConfirmed = await Application.Current.MainPage.DisplayAlert("Delete product", "Are you sure?", "Yes", "No");

            if (deletionConfirmed)
            {
                try
                {
                    await Warehouse.RemoveProduct(_productId);
                    await Shell.Current.GoToAsync($"..");
                }
                catch (SyncRedirectException)
                {
                    Application.Current.MainPage = new SyncProductsResultPage();
                }
            }
        }

        async void OnIncreaseQuantity()
        {
            await Warehouse.ChangeProductQuantity(_productId, _increaseQuantityNumber);
            LoadProduct(_productId);
        }

        async void OnDecreaseQuantity()
        {
            await Warehouse.ChangeProductQuantity(_productId, - _decreaseQuantityNumber);
            LoadProduct(_productId);
        }

        public bool ValidateIncreaseQuantity()
        {
            return IncreaseQuantityNumber > 0;
        }

        public bool ValidateDecreaseQuantity()
        {
            return DecreaseQuantityNumber > 0 && Product.Quantity >= _decreaseQuantityNumber;
        }

        public long IncreaseQuantityNumber
        {
            get => _increaseQuantityNumber;
            set => SetProperty(ref _increaseQuantityNumber, value);
        }

        public long DecreaseQuantityNumber
        {
            get => _decreaseQuantityNumber;
            set => SetProperty(ref _decreaseQuantityNumber, value);
        }

        public string ProductId
        {
            get => _productId.ToString();
            set
            {
                if (Int64.TryParse(value, out long newProductId))
                {
                    _productId = newProductId;
                    LoadProduct(_productId);
                }
            }
        }

        private async void LoadProduct(long productId)
        {
            try
            {
                Product = await Warehouse.GetProduct(productId);
                OnPropertyChanged("Product");
            }
            catch (SyncRedirectException)
            {
                Application.Current.MainPage = new SyncProductsResultPage();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public void OnAppearing()
        {
            if (_productId != 0)
            {
                LoadProduct(_productId);
            }
        }
    }
}
