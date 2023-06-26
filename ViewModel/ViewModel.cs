using fishing_store_app.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace fishing_store_app
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        public ObservableCollection<Manufacturer> Manufacturers { get; set; }

        private Dictionary<string, int> CategoriesId { get; set; }

        private Dictionary<string, int> ManufacturersId { get; set; }

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("http://localhost:8080/"),
        };

        public ViewModel()
        {
            fillProducts();
            fillCategories();
            fillManufacturers();
        }

        private void fillProducts()
        {
            Products = new ObservableCollection<Product>(sharedClient.GetFromJsonAsync<List<Product>>("products", default).Result);
        }

        private void fillCategories()
        {
            Categories = new ObservableCollection<Category>(sharedClient.GetFromJsonAsync<List<Category>>("categories", default).Result);
            List<string> categories = new List<string>(Categories.Count);
            CategoriesId = new Dictionary<string, int>();
            for (int i = 0; i < Categories.Count; i++)
            {
                categories.Add(Categories[i].Name);
                CategoriesId[Categories[i].Name] = Categories[i].Id;
            }
            CBProductCategory = categories;
        }

        private void fillManufacturers()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(sharedClient.GetFromJsonAsync<List<Manufacturer>>("manufacturers", default).Result);
            List<string> manufacturers = new List<string>(Manufacturers.Count);
            ManufacturersId = new Dictionary<string, int>();
            for (int i = 0; i < Manufacturers.Count; i++)
            {
                manufacturers.Add(Manufacturers[i].Name);
                ManufacturersId[Manufacturers[i].Name] = Manufacturers[i].Id;
            }
            CBProductManufacturer = manufacturers;
        }

        private RelayCommand _refreshProducts;
        public RelayCommand RefreshProducts
        {
            get
            {
                return _refreshProducts ??
                (_refreshProducts = new RelayCommand(obj =>
                {
                    Products = new ObservableCollection<Product>(sharedClient.GetFromJsonAsync<List<Product>>("products", default).Result);
                    NotifyPropertyChanged("Products");
                }));
            }
        }

        private RelayCommand _createProduct;
        public RelayCommand CreateProduct
        {
            get
            {
                return _createProduct ??
                (_createProduct = new RelayCommand(obj =>
                {
                    var requestProduct = new RequestProduct
                    {
                        Name = TBProductName,
                        Price = TBProductPrice,
                        Stock = TBProductStock,
                        CategoryId = CategoriesId[SelectedCBProductCategory],
                        ManufacturerId = ManufacturersId[SelectedCBProductManufacturer]
                    };
                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestProduct), Encoding.UTF8);
                    var response = sharedClient.PostAsync("products", httpContent).Result;
                    Thread.Sleep(5);
                    Products = new ObservableCollection<Product>(sharedClient.GetFromJsonAsync<List<Product>>("products", default).Result);
                    NotifyPropertyChanged("Products");
                }));
            }
        }

        private RelayCommand _updateProduct;
        public RelayCommand UpdateProduct
        {
            get
            {
                return _updateProduct ??
                (_updateProduct = new RelayCommand(obj =>
                {
                    var requestProduct = new RequestProduct
                    {
                        Id = SelectedProduct.Id,
                        Name = TBProductName,
                        Price = TBProductPrice,
                        Stock = TBProductStock,
                        CategoryId = CategoriesId[SelectedCBProductCategory],
                        ManufacturerId = ManufacturersId[SelectedCBProductManufacturer]
                    };
                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestProduct), Encoding.UTF8);
                    var response = sharedClient.PutAsync("products", httpContent).Result;
                    Thread.Sleep(5);
                    Products = new ObservableCollection<Product>(sharedClient.GetFromJsonAsync<List<Product>>("products", default).Result);
                    NotifyPropertyChanged("Products");
                }));
            }
        }

        private RelayCommand _deleteProduct;
        public RelayCommand DeleteProduct
        {
            get
            {
                return _deleteProduct ??
                (_deleteProduct = new RelayCommand(obj =>
                {
                    sharedClient.DeleteAsync("products/" + SelectedProduct.Id);
                    Thread.Sleep(5);
                    Products = new ObservableCollection<Product>(sharedClient.GetFromJsonAsync<List<Product>>("products", default).Result);
                    NotifyPropertyChanged("Products");
                }));
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (value != null) {
                    TBProductName = value.Name;
                    TBProductPrice = value.Price;
                    TBProductStock = value.Stock;
                    SelectedCBProductCategory = value.Category;
                    SelectedCBProductManufacturer = value.Manufacturer;

                    _selectedProduct = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _tBProductName;
        public string TBProductName
        {
            get { return _tBProductName; }
            set
            {
                _tBProductName = value;
                NotifyPropertyChanged();
            }
        }

        private int _tBProductPrice;
        public int TBProductPrice
        {
            get { return _tBProductPrice; }
            set
            {
                _tBProductPrice = value;
                NotifyPropertyChanged();
            }
        }

        private int _tBProductStock;
        public int TBProductStock
        {
            get { return _tBProductStock; }
            set
            {
                _tBProductStock = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedCBProductCategory;
        public string SelectedCBProductCategory
        {
            get { return _selectedCBProductCategory; }
            set
            {
                _selectedCBProductCategory = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedCBProductManufacturer;
        public string SelectedCBProductManufacturer
        {
            get { return _selectedCBProductManufacturer; }
            set
            {
                _selectedCBProductManufacturer = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> _cBProductCategory;
        public List<string> CBProductCategory
        {
            get { return _cBProductCategory; }
            set
            {
                _cBProductCategory = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> _cBProductManufacturer;
        public List<string> CBProductManufacturer
        {
            get { return _cBProductManufacturer; }
            set
            {
                _cBProductManufacturer = value;
                NotifyPropertyChanged();
            }
        }

        public class RelayCommand : ICommand
        {
            private Action<object> execute;
            private Func<object, bool> canExecute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                this.execute = execute;
                this.canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return this.canExecute == null || this.canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                this.execute(parameter);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
