using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginChooserPage : ContentPage
    {
        public LoginChooserPage()
        {
            InitializeComponent();
            BindingContext = new LoginChooserViewModel();
        }
    }
}