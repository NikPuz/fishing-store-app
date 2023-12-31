﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fishing_store_app.Model
{
    public class Product : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private int price;
        private string description;
        private int stock;
        private string barcode;
        private string category;
        private string manufacturer;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public int Stock
        {
            get { return stock; }
            set
            {
                stock = value;
                OnPropertyChanged("Stock");
            }
        }
        public string Barcode
        {
            get { return barcode; }
            set
            {
                barcode = value;
                OnPropertyChanged("Barcode");
            }
        }
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        public string Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class RequestProduct
    {
        public int Id;
        public string Name;
        public int Price;
        public string Description;
        public int Stock;
        public string Barcode;
        public int CategoryId;
        public int ManufacturerId;
    }
}
