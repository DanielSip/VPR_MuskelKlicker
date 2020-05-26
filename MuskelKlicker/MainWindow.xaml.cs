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

namespace MuskelKlicker
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

        Clicker clicker = new Clicker(0, 1);
        int points = 100;
        int multiplikator = 1;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Items | Clicker
            List<ShopItem> ListItems = new List<ShopItem>();

            ShopItem hanteln = new ShopItem(10, "Hanteln", "Erhöht dein Klick um 1", 0, 1);
            ShopItem gHanteln = new ShopItem(20, "Goldene-Hanteln", "Erhöht dein Klick um 2", 0, 2);
            ShopItem protein = new ShopItem(200, "Protein", "Erhöht den Passive und Aktiven Klick um 10", 10, 10);
            ListItems.Add(hanteln);
            ListItems.Add(gHanteln);
            ListItems.Add(protein);


            //Shop mit Items erstellen
            foreach (ShopItem items in ListItems)
            {
                lstbx_shopitems.Items.Add(items);
            }

        }
        private void lstbx_shopitems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstbx_shopitems.SelectedItem != null)
            {
                ShopItem item = (ShopItem)lstbx_shopitems.SelectedItem;
                MessageBox.Show(item.EnoughPoints(points, item).ToString());
                if (item.EnoughPoints(points, item))
                {
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                    MessageBox.Show(points.ToString());
                }


            }
        }

        private void bt_KlickbarerBereich_Click(object sender, RoutedEventArgs e)
        {
            points += clicker.ActiveClick;
        }
    }
}
