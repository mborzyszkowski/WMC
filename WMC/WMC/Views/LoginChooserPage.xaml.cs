using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WMC.Models;
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
            BindingContext = new LoginChooserViewModel(WebView);
        }
    }
}