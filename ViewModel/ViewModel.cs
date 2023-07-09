﻿using fishing_store_app.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Image = iTextSharp.text.Image;
using Barcoded;
using System.Diagnostics;
using System.Drawing;
using Rectangle = iTextSharp.text.Rectangle;
using Font = System.Drawing.Font;
using Document = iTextSharp.text.Document;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace fishing_store_app
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        
        public ObservableCollection<Supply> Supplies { get; set; }
        
        public ObservableCollection<Sale> Sales { get; set; }

        public ObservableCollection<SaleItem> SaleItems { get; set; }
        
        public ObservableCollection<Printing> Printings { get; set; }

        public ObservableCollection<BasketItem> Basket { get; set; }

        private Dictionary<string, int> CategoriesId { get; set; }

        private Dictionary<string, int> ManufacturersId { get; set; }

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("http://62.109.22.0:8088/"),
        };

        public ViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            fillProducts();
            fillCategories();
            fillManufacturers();
            fillSales();
            fillSupplies();
            TBBarcodeCount = 1;
            TBPrintColums = 3;
        }

        private void fillProducts()
        {
            Products = new ObservableCollection<Product>(sharedClient.GetFromJsonAsync<List<Product>>("products", default).Result);
        }

        private void fillCategories()
        {
            Categories = new ObservableCollection<Category>(sharedClient.GetFromJsonAsync<List<Category>>("categories", default).Result);
            ObservableCollection<string> categories = new ObservableCollection<string>();
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
            ObservableCollection<string> manufacturers = new ObservableCollection<string>();
            ManufacturersId = new Dictionary<string, int>();
            for (int i = 0; i < Manufacturers.Count; i++)
            {
                manufacturers.Add(Manufacturers[i].Name);
                ManufacturersId[Manufacturers[i].Name] = Manufacturers[i].Id;
            }
            CBProductManufacturer = manufacturers;
        }

        private void fillSupplies()
        {
            Supplies = new ObservableCollection<Supply>(sharedClient.GetFromJsonAsync<List<Supply>>("supplies", default).Result);
        }

        private void fillSales()
        {
            Sales = new ObservableCollection<Sale>(sharedClient.GetFromJsonAsync<List<Sale>>("sales", default).Result);
        }

        private RelayCommand _refreshProducts;
        public RelayCommand RefreshProducts
        {
            get
            {
                return _refreshProducts ??
                (_refreshProducts = new RelayCommand(obj =>
                {
                    fillProducts();
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
                        Description = TBProductDescription,
                        Stock = TBProductStock,
                        CategoryId = CategoriesId[SelectedCBProductCategory],
                        ManufacturerId = ManufacturersId[SelectedCBProductManufacturer]
                    };

                    sharedClient.PostAsync("products", new StringContent(JsonConvert.SerializeObject(requestProduct), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillProducts();
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
                        Description = TBProductDescription,
                        Stock = TBProductStock,
                        CategoryId = CategoriesId[SelectedCBProductCategory],
                        ManufacturerId = ManufacturersId[SelectedCBProductManufacturer]
                    };

                    sharedClient.PutAsync("products", new StringContent(JsonConvert.SerializeObject(requestProduct), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillProducts();
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
                    fillProducts();
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
                    TBProductDescription = value.Description;
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
        
        private string _tBProductDescription;
        public string TBProductDescription
        {
            get { return _tBProductDescription; }
            set
            {
                _tBProductDescription = value;
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
                if (value != null)
                {
                    _selectedCBProductCategory = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _selectedCBProductManufacturer;
        public string SelectedCBProductManufacturer
        {
            get { return _selectedCBProductManufacturer; }
            set
            {
                if (value != null)
                {
                    _selectedCBProductManufacturer = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _cBProductCategory;
        public ObservableCollection<string> CBProductCategory
        {
            get { return _cBProductCategory; }
            set
            {
                _cBProductCategory = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<string> _cBProductManufacturer;
        public ObservableCollection<string> CBProductManufacturer
        {
            get { return _cBProductManufacturer; }
            set
            {
                _cBProductManufacturer = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _refreshCategories;
        public RelayCommand RefreshCategories
        {
            get
            {
                return _refreshCategories ??
                (_refreshCategories = new RelayCommand(obj =>
                {
                    fillCategories();
                    NotifyPropertyChanged("Categories");
                }));
            }
        }

        private RelayCommand _createCategory;
        public RelayCommand CreateCategory
        {
            get
            {
                return _createCategory ??
                (_createCategory = new RelayCommand(obj =>
                {
                    var category = new Category
                    {
                        Name = TBCategoryName
                    };

                    sharedClient.PostAsync("categories", new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillCategories();
                    NotifyPropertyChanged("Categories");
                }));
            }
        }

        private RelayCommand _updateCategory;
        public RelayCommand UpdateCategory
        {
            get
            {
                return _updateCategory ??
                (_updateCategory = new RelayCommand(obj =>
                {
                    var category = new Category
                    {
                        Id = SelectedCategory.Id,
                        Name = TBCategoryName
                    };

                    sharedClient.PutAsync("categories", new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillCategories();
                    NotifyPropertyChanged("Categories");
                }));
            }
        }

        private RelayCommand _deleteCategory;
        public RelayCommand DeleteCategory
        {
            get
            {
                return _deleteCategory ??
                (_deleteCategory = new RelayCommand(obj =>
                {
                    if (SelectedCategory.Id != 0)
                    {
                        sharedClient.DeleteAsync("categories/" + SelectedCategory.Id);
                        Thread.Sleep(5);
                        fillCategories();
                        fillProducts();
                        NotifyPropertyChanged("Categories");
                        NotifyPropertyChanged("Products");
                    }
                }));
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (value != null)
                {
                    TBCategoryName = value.Name;

                    _selectedCategory = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _tBCategoryName;
        public string TBCategoryName
        {
            get { return _tBCategoryName; }
            set
            {
                _tBCategoryName = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _refreshManufacturers;
        public RelayCommand RefreshManufacturers
        {
            get
            {
                return _refreshManufacturers ??
                (_refreshManufacturers = new RelayCommand(obj =>
                {
                    fillManufacturers();
                    NotifyPropertyChanged("Manufacturers");
                }));
            }
        }

        private RelayCommand _createManufacturer;
        public RelayCommand CreateManufacturer
        {
            get
            {
                return _createManufacturer ??
                (_createManufacturer = new RelayCommand(obj =>
                {
                    var manufacturer = new Manufacturer
                    {
                        Name = TBManufacturerName
                    };

                    sharedClient.PostAsync("manufacturers", new StringContent(JsonConvert.SerializeObject(manufacturer), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillManufacturers();
                    NotifyPropertyChanged("Manufacturers");
                }));
            }
        }

        private RelayCommand _updateManufacturer;
        public RelayCommand UpdateManufacturer
        {
            get
            {
                return _updateManufacturer ??
                (_updateManufacturer = new RelayCommand(obj =>
                {
                    var manufacturer = new Manufacturer
                    {
                        Id = SelectedManufacturer.Id,
                        Name = TBManufacturerName
                    };

                    sharedClient.PutAsync("manufacturers", new StringContent(JsonConvert.SerializeObject(manufacturer), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillManufacturers();
                    NotifyPropertyChanged("Manufacturers");
                }));
            }
        }

        private RelayCommand _deleteManufacturer;
        public RelayCommand DeleteManufacturer
        {
            get
            {
                return _deleteManufacturer ??
                (_deleteManufacturer = new RelayCommand(obj =>
                {
                    if (SelectedManufacturer.Id != 0)
                    {
                        sharedClient.DeleteAsync("manufacturers/" + SelectedManufacturer.Id);
                        Thread.Sleep(5);
                        fillManufacturers();
                        fillProducts();
                        NotifyPropertyChanged("Manufacturers");
                        NotifyPropertyChanged("Products");
                    }
                }));
            }
        }

        private Manufacturer _selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set
            {
                if (value != null)
                {
                    TBManufacturerName = value.Name;

                    _selectedManufacturer = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _tBManufacturerName;
        public string TBManufacturerName
        {
            get { return _tBManufacturerName; }
            set
            {
                _tBManufacturerName = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _refreshSuppies;
        public RelayCommand RefreshSuppies
        {
            get
            {
                return _refreshSuppies ??
                (_refreshSuppies = new RelayCommand(obj =>
                {
                    fillSupplies();
                    NotifyPropertyChanged("Supplies");
                }));
            }
        }


        private Product _selectedSupplyProduct;
        public Product SelectedSupplyProduct
        {
            get { return _selectedSupplyProduct; }
            set
            {
                if (value != null)
                {
                    TBSupplyProductName = value.Name;
                    TBSupplyProductCategory = value.Category;
                    TBSupplyProductManufacturer = value.Manufacturer;

                    _selectedSupplyProduct = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private RelayCommand _createSupply;
        public RelayCommand CreateSupply
        {
            get
            {
                return _createSupply ??
                (_createSupply = new RelayCommand(obj =>
                {
                    var supply = new RequestSupply
                    {
                        ProductId = SelectedSupplyProduct.Id,
                        UnitPrice = TBSupplyProductUnitPrice,
                        Count = TBSupplyProductCount,
                        Date = null
                    };

                    sharedClient.PostAsync("supplies", new StringContent(JsonConvert.SerializeObject(supply), Encoding.UTF8));
                    Thread.Sleep(5);
                    fillSupplies();
                    NotifyPropertyChanged("Supplies");
                }));
            }
        }

        private string _tBSupplyProductName;
        public string TBSupplyProductName
        {
            get { return _tBSupplyProductName; }
            set
            {
                _tBSupplyProductName = value;
                NotifyPropertyChanged();
            }
        }

        private string _tBSupplyProductCategory;
        public string TBSupplyProductCategory
        {
            get { return _tBSupplyProductCategory; }
            set
            {
                _tBSupplyProductCategory = value;
                NotifyPropertyChanged();
            }
        }

        private string _tBSupplyProductManufacturer;
        public string TBSupplyProductManufacturer
        {
            get { return _tBSupplyProductManufacturer; }
            set
            {
                _tBSupplyProductManufacturer = value;
                NotifyPropertyChanged();
            }
        }

        private int _tBSupplyProductCount;
        public int TBSupplyProductCount
        {
            get { return _tBSupplyProductCount; }
            set
            {
                _tBSupplyProductCount = value;
                NotifyPropertyChanged();
            }
        }

        private int _tBSupplyProductUnitPrice;
        public int TBSupplyProductUnitPrice
        {
            get { return _tBSupplyProductUnitPrice; }
            set
            {
                _tBSupplyProductUnitPrice = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _refreshSales;
        public RelayCommand RefreshSales
        {
            get
            {
                return _refreshSales ??
                (_refreshSales = new RelayCommand(obj =>
                {
                    fillSales();
                    NotifyPropertyChanged("Sales");
                }));
            }
        }

        private Sale _selectedSale;
        public Sale SelectedSale
        {
            get { return _selectedSale; }
            set
            {
                if (value != null)
                {
                    if (value != null)
                    {
                        _selectedSale = value;
                        SaleItems = new ObservableCollection<SaleItem>(value.SaleItems);
                        NotifyPropertyChanged("SaleItems");
                        NotifyPropertyChanged();
                    }
                }
            }
        }

        private Product _selectedBarcodeProduct;
        public Product SelectedBarcodeProduct
        {
            get { return _selectedBarcodeProduct; }
            set
            {
                if (value != null)
                {
                    
                    _selectedBarcodeProduct = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _tBBarcodeCount;
        public int TBBarcodeCount
        {
            get { return _tBBarcodeCount; }
            set
            {
                _tBBarcodeCount = value;
                NotifyPropertyChanged();
            }
        }

        private Printing _selectedPrinting;
        public Printing SelectedPrinting
        {
            get { return _selectedPrinting; }
            set
            {
                if (value != null)
                {

                    _selectedPrinting = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private RelayCommand _addToPrintings;
        public RelayCommand AddToPrintings
        {
            get
            {
                return _addToPrintings ??
                (_addToPrintings = new RelayCommand(obj =>
                {

                    var printing = new Printing
                    {
                        Product = SelectedBarcodeProduct,
                        Count = TBBarcodeCount
                    };

                    if (Printings != null)
                    {
                        Printings.Add(printing);
                    } else
                    {
                        Printings = new ObservableCollection<Printing>() { printing };
                    }
                    
                    NotifyPropertyChanged("Printings");
                }));
            }
        }

        private int _tBPrintColums;
        public int TBPrintColums
        {
            get { return _tBPrintColums; }
            set
            {
                _tBPrintColums = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _deletePrinting;
        public RelayCommand DeletePrinting
        {
            get
            {
                return _deletePrinting ??
                (_deletePrinting = new RelayCommand(obj =>
                {

                    Printings.Remove(SelectedPrinting);
                    NotifyPropertyChanged("Printings");
                }));
            }
        }

        private RelayCommand _clearPrintings;
        public RelayCommand ClearPrintings
        {
            get
            {
                return _clearPrintings ??
                (_clearPrintings = new RelayCommand(obj =>
                {

                    Printings = new ObservableCollection<Printing>();
                    NotifyPropertyChanged("Printings");
                }));
            }
        }

        private RelayCommand _printing;
        public RelayCommand Printing
        {
            get
            {
                return _printing ??
                (_printing = new RelayCommand(obj =>
                {
                    if (Printings == null || Printings.Count == 0) {
                        return;
                    }

                    var document = new Document();

                    var fileStream = new FileStream("Barcodes.pdf", FileMode.Create, FileAccess.Write, FileShare.None);

                    PdfWriter.GetInstance(document, fileStream);

                    var images = new List<Image>();

                    document.Open();

                    foreach (var item in Printings)
                    {
                        var cuntryCode = "460";
                        var manufacturerCode = ManufacturersId[item.Product.Manufacturer].ToString();
                        var productCode = item.Product.Id.ToString();
                        for (var i = 1; manufacturerCode.Length < 4; i++)
                        {
                            manufacturerCode = "0" + manufacturerCode;
                        }

                        for (var i = 1; productCode.Length < 5; i++)
                        {
                            productCode = "0" + productCode;
                        }
                        var itemBarcode = cuntryCode + manufacturerCode + productCode;

                        LinearBarcode newBarcode = new LinearBarcode(itemBarcode, Symbology.Ean13)
                        {
                            Encoder =
                            {
                                Dpi = 300,
                                BarcodeHeight = 100
                            }
                        };

                        var bmp = new Bitmap(300, 300);
                        bmp.SetResolution(250, 250);
                        Graphics graph = Graphics.FromImage(bmp);
                        graph.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

                        var barcodeImage = Image.GetInstance(newBarcode.Image, ImageFormat.Jpeg);

                        graph.DrawImage(System.Drawing.Image.FromStream(new MemoryStream(barcodeImage.RawData)), 0, bmp.Height * 2 / 3);

                        var name = item.Product.Name + " " + item.Product.Manufacturer;
                        if (item.Product.Manufacturer == "Без производителя")
                        {
                            name = item.Product.Name;
                        }

                        var textBounds = graph.VisibleClipBounds;
                        textBounds.Inflate(-5, -5);
                        Font font = new Font("Arial", 10);
                        graph.DrawString(
                            name,
                            font,
                            Brushes.Black,
                            textBounds
                        );

                        textBounds.Inflate(-5, -80);
                        font = new Font("Arial", 4);
                        graph.DrawString(
                            item.Product.Description,
                            font,
                            Brushes.Black,
                            textBounds
                        );

                        font = new Font("Arial", 24);
                        textBounds.Inflate(-25, -20);
                        if (item.Product.Price > 999)
                        {
                            font = new Font("Arial", 18);
                            textBounds.Inflate(-10, -15);
                        }
                        graph.DrawString(
                            item.Product.Price.ToString() + "₽",
                            font,
                            Brushes.Black,
                            textBounds
                        );

                        var image = Image.GetInstance(bmp, ImageFormat.Bmp);
                        image.Border = Rectangle.BOX;
                        image.BorderColor = BaseColor.GRAY;
                        image.BorderWidth = 6f;
                        image.SpacingBefore = 10f;

                        for (var i = 0; i < item.Count; i++)
                        {
                            images.Add(image);
                        }
                    }

                    var table = new PdfPTable(TBPrintColums);

                    for (var i = 0; i < images.Count; i++)
                    {
                        table.AddCell(new PdfPCell(images[i], true));
                    }
                    table.CompleteRow();
                    document.Add(table);

                    document.Close();

                    Process.Start(new ProcessStartInfo("Barcodes.pdf") { UseShellExecute = true });
                }));
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
