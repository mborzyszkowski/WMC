using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WmcLoginPage : ContentPage
    {
        public WmcLoginPage()
        {
            InitializeComponent();
            BindingContext = new WmcLoginViewModel();
        }
    }
}