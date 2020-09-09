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

namespace ItemsHinzufuegen
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    //Daniel Sippel
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ItemlistRefresh();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            if (txB_Name.Text != "" && txB_Description.Text != "" && txB_Cost.Text != "" && txB_Active.Text != "" && txB_Passive.Text != "")
            {
                int cost;
                int passive;
                int active;
                if (int.TryParse(txB_Cost.Text, out cost) && int.TryParse(txB_Active.Text, out active) && int.TryParse(txB_Passive.Text, out passive))
                {
                    SpielstandDTB spielstandDTB = new SpielstandDTB();
                    bool mssg = spielstandDTB.AddItem(cost, txB_Name.Text, txB_Description.Text, passive, active);

                    if (mssg)
                    {
                        MessageBox.Show(txB_Name.Text + " wurde hinzugefügt");
                        txB_Name.Text = "";
                        txB_Description.Text = "";
                        txB_Cost.Text = "";
                        txB_Active.Text = "";
                        txB_Passive.Text = "";
                        ItemlistRefresh();

                    }
                    else
                    {
                        MessageBox.Show(txB_Name.Text + " existiert schon");
                        txB_Name.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("bitte Rechte Seite nur mit Zahlen füllen");
                }
            }
        }

        private void bt_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (lsBx_Items.SelectedItem != null)
            {
                SpielstandDTB spielstandDTB = new SpielstandDTB();
                List<ShopItem> listItems = new List<ShopItem>();

                spielstandDTB.DeleteItem((lsBx_Items.SelectedItem as ShopItem).Name);
                lsBx_Items.ReleaseMouseCapture();
                ItemlistRefresh();

                MessageBox.Show("Wurde gelöscht");
            }
        }

        private void ItemlistRefresh()
        {
            lsBx_Items.Items.Clear();
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
