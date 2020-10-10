using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewProductPage : ContentPage
    {
        public NewProductPage()
        {
            InitializeComponent();
            BindingContext = new NewProductViewModel();
        }
    }
}