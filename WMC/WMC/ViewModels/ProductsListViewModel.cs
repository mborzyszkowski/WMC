using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using WMC.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WMC.ViewModels
{
    class ProductsListViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; }
        public Command LoadProductsCommand { get; }
        //public Command AddProductCommand { get; }
        //public Command<Product> ProductSelected { get; }

        public ProductsListViewModel()
        {
            Title = "Warehouse Products";
            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(ExecuteLoadProduct);
        }

        void ExecuteLoadProduct()
        {
            IsBusy = true;

            try
            {
                Products.Clear();
                var products = Warehouse.GetProductsList();
                products.ForEach(p => Products.Add(p));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
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
