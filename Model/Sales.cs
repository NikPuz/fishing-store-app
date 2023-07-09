using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fishing_store_app.Model
{
    class Sale : INotifyPropertyChanged
    {
        private int id;
        private int sum;
        private int cashierId;
        private DateTime date;
        private List<SaleItem> saleItems;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
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
        public int CashierId
        {
            get { return cashierId; }
            set
            {
                cashierId = value;
                OnPropertyChanged("CashierId");
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
        public List<SaleItem> SaleItems
        {
            get { return saleItems; }
            set
            {
                saleItems = value;
                OnPropertyChanged("SaleItems");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class SaleItem : INotifyPropertyChanged
    {
        private int id;
        private int saleId;
        private string? productName;
        private int unitPrice;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int SaleId
        {
            get { return saleId; }
            set
            {
                saleId = value;
                OnPropertyChanged("SaleId");
            }
        }
        public string? ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class RequestSale
    {
        public int CashierId;
        public List<SaleItem> SaleItems;
    }
}
