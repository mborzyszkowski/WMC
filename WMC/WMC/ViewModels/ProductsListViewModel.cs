using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WMC.Models;
using WMC.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WMC.ViewModels
{
    class ProductsListViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; }

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                OnProductTapped(value);
            }
        }

        public ProductsListViewModel()
        {
            Title = "Warehouse products";
            Products = new ObservableCollection<Product>();

            LoadProductsCommand = new Command(async () => await OnLoadProduct());
            AddProductCommand = new Command(OnAddProduct);
            ProductTapped = new Command<Product>(OnProductTapped);
        }

        public Command LoadProductsCommand { get; }
        public Command AddProductCommand { get; }
        public Command<Product> ProductTapped { get; }

        async Task OnLoadProduct()
        {
            IsBusy = true;

            try
            {
                Products.Clear();
                var products = await Warehouse.GetProductsList();
                products.ForEach(p => Products.Add(p));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void OnAddProduct()
        {
            await Shell.Current.GoToAsync(nameof(NewProductPage));
        }

        async void OnProductTapped(Product product)
        {
            if (product == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ProductDetailsPage)}?{nameof(ProductDetailsViewModel.ProductId)}={product.Id}");
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedProduct = null;
        }
    }
}
