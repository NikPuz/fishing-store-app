using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fishing_store_app.Model
{
    internal class Printing : INotifyPropertyChanged
    {
        private Product product;
        private int count;

        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged("Product");
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
}
