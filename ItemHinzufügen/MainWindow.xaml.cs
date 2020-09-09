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
using MuskelKlicker;

namespace ItemHinzufügen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SpielstandDTB spielstandDTB = new SpielstandDTB();
            List<ShopItem> listItems = new List<ShopItem>();

            listItems = spielstandDTB.GetShopItems();

            foreach (ShopItem items in listItems)
            {
                lsBx_Items.Items.Add(items);
            }
        }
    }
}
