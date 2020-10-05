using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WMC.Models;

namespace WMC.ViewModels
{
    class ProductsListViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; }

        public void OnAppearing()
        {
        }
    }
}
