using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    public class SyncProductsResultViewModel : BaseViewModel
    {
        private List<string> SyncProductsResult => Warehouse.GetSyncResultErrors();

        public ObservableCollection<string> SyncProductsResultList { get; set; }

        public SyncProductsResultViewModel()
        {
            Title = "Sync products result";
            OkCommand = new Command(() => Application.Current.MainPage = new AppShell());
            SyncProductsResultList = new ObservableCollection<string>(SyncProductsResult);
        }

        public Command OkCommand { get; }
    }
}
