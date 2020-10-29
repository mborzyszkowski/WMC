using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncProductsResultPage : ContentPage
    {
        private readonly SyncProductsResultViewModel _viewModel;
        public SyncProductsResultPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SyncProductsResultViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}