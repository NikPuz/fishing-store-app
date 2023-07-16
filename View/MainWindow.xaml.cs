using Caliburn.Micro;
using fishing_store_app.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace fishing_store_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel();
        }

        private void clearBasketButton_Click(object sender, RoutedEventArgs e)
        {
            tBBarcode.Focus();
        }

        private void deleteBasketButton_Click(object sender, RoutedEventArgs e)
        {
            tBBarcode.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tBBarcode.Focus();
        }

        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            tBBarcode.Focus();
        }

        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            refreshProductsButton.Command.Execute(null);
        }

        private void TabItem_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            refreshProductsButton.Command.Execute(null);
        }

        private void TabItem_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            refreshSalesButton.Command.Execute(null);
        }

        private void TabItem_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            refreshSuppiesButton.Command.Execute(null);
            refreshProductsButton.Command.Execute(null);
        }

        private void TabItem_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            refreshManufacturersButton.Command.Execute(null);
        }

        private void TabItem_MouseLeftButtonDown_5(object sender, MouseButtonEventArgs e)
        {
            refreshCategoriesButton.Command.Execute(null);
        }
    }
}
