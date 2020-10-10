using WMC.Models;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    class NewProductViewModel : BaseViewModel
    {
        private string _manufacturerName;
        private string _modelName;
        private double _price;

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
                   && _price > 0;
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
                Quantity = 0,
            };

            await Warehouse.AddProduct(newProduct);

            await Shell.Current.GoToAsync("..");
        }
    }
}
