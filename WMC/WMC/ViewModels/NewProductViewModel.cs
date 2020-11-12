using WMC.Exceptions;
using WMC.Models;
using WMC.Views;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    public class NewProductViewModel : BaseViewModel
    {
        private string _manufacturerName;
        private string _modelName;
        private double _price;
        private double _priceUsd;

        public NewProductViewModel()
        {
            Title = "Add new product";
            SaveCommand = new Command(OnSave, ValidateProduct);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public bool ValidateProduct()
        {
            return !string.IsNullOrWhiteSpace(_manufacturerName)
                   && !string.IsNullOrWhiteSpace(_modelName)
                   && _price > 0
                   && _priceUsd > 0;
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

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var newProduct = new Product()
            {
                ManufacturerName = _manufacturerName,
                ModelName = _modelName,
                Price = _price,
                PriceUsd = _priceUsd,
                Quantity = 0,
            };

            try
            {
                await Warehouse.AddProduct(newProduct);
                await Shell.Current.GoToAsync("..");
            }
            catch (SyncRedirectException)
            {
                Application.Current.MainPage = new SyncProductsResultPage();
            }
        }
    }
}
