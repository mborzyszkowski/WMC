using System;
using WMC.Services;
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
            MainPage = new MainPage();
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
