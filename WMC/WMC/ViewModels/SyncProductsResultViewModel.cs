using System.Collections.ObjectModel;
using System.Linq;
using WMC.Models;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    public class SyncProductsResultViewModel : BaseViewModel
    {
        public ObservableCollection<Error> SyncProductsResultList { get; set; }

        public SyncProductsResultViewModel()
        {
            Title = "Sync products result";
            OkCommand = new Command(() => Application.Current.MainPage = new AppShell());
            LoadCommand = new Command(OnLoadSyncProductResult);
            SyncProductsResultList = new ObservableCollection<Error>(
                Warehouse.GetSyncResultErrors()
                    .Select(er => new Error { Content = er })
                    .ToList());
        }

        public Command OkCommand { get; }
        public Command LoadCommand { get; }

        void OnLoadSyncProductResult()
        {
            IsBusy = true;

            try
            {
                SyncProductsResultList.Clear();
                var syncProductsResult = Warehouse.GetSyncResultErrors();
                syncProductsResult.ForEach(p => SyncProductsResultList.Add(new Error { Content = p }));
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
