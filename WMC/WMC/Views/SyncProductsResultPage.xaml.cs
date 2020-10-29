using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncProductsResultPage : ContentPage
    {
        public SyncProductsResultPage()
        {
            InitializeComponent();
            BindingContext = new SyncProductsResultViewModel();
        }
    }
}