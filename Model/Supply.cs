using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fishing_store_app.Model
{
    class Supply : INotifyPropertyChanged
    {
        private int id;
        private List<SupplyItem> items;
        private int sum;
        private DateTime date;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public List<SupplyItem> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }
        public int Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                OnPropertyChanged("Sum");
            }
        }
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value.AddHours(3);
                OnPropertyChanged("Date");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    class SupplyItem : INotifyPropertyChanged
    {
        private string? productName;
        private string? productCategory;
        private string? productManufacturer;
        private int unitPrice;
        private int count;

        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        public string ProductCategory
        {
            get { return productCategory; }
            set
            {
                productCategory = value;
                OnPropertyChanged("ProductCategory");
            }
        }
        public string ProductManufacturer
        {
            get { return productManufacturer; }
            set
            {
                productManufacturer = value;
                OnPropertyChanged("ProductManufacturer");
            }
        }
        public int UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class RequestSupply
    {
        public int productId;
        public int unitPrice;
        public int count;
    }
}
