using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductListPage : ContentPage
    {
        private readonly ProductsListViewModel _viewModel;

        public ProductListPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ProductsListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}