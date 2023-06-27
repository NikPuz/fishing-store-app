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
        private string? productName;
        private string? productCategory;
        private string? productManufacturer;
        private int unitPrice;
        private int count;
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
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
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

    public class RequestSupply
    {
        public int Id;
        public int ProductId;
        public int UnitPrice;
        public int Count;
        public DateTime? Date;
    }
}
