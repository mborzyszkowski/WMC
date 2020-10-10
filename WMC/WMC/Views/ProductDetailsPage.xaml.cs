using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailsPage : ContentPage
    {
        private readonly ProductDetailsViewModel _viewModel;
        public ProductDetailsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ProductDetailsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}