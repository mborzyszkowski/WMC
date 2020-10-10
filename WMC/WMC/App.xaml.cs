using System;
using WMC.Models;
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

            DependencyService.Register<WarehouseProducts>();

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
