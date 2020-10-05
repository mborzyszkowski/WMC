using System;
using WMC.Services;
using WMC.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WMC
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<WarehouseProductsMock>();
            MainPage = new ProductListPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
