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
using System.Windows.Threading;
using System.Timers;

namespace MuskelKlicker
{
    /// <s0000000ummary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string user;

        public MainWindow(string user)
        {
            InitializeComponent();
            this.user = user;
        }

        Random rnd = new Random();

        Clicker clicker = new Clicker(0, 1);
        List<ShopItem> ListItems = new List<ShopItem>();
        List<PrestigeItem> prestigeItems = new List<PrestigeItem>();

        int points = 100;
        int multiplyer = 1;
        int clicksPerSecond = 0;
        
        List<int> lastClicks = new List<int>(); // Liste mit den 10 letzten werten der cps

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ToDo: -Klicks per sec             Done: Andrew John / Dennis
            //      -Reset von klicks per sec   Done: Andrew John / Dennis
            //      -Klickbonus                 Done: Andrew John
            //      -Grafiken einfügen          Done: Andrew John
            //      -Shopitems aus der DB       
            //      -Fortschritt speichern      fehlt Prestige
            //      -Fortschritt aufrufen       fehlt Prestige

            #region Shop und Item Update
            //Items | Clicker
            lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
            lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);

            //Test
            ///*//Aktive Clicks
            //ShopItem hanteln = new ShopItem(10, "Hanteln", "Erhöht dein Klick um 1", 0, 1, 0);
            //ShopItem gHanteln = new ShopItem(20, "Goldene-Hanteln", "Erhöht dein Klick um 2", 0, 2, 0);
            //ShopItem protein = new ShopItem(200, "Protein", "Erhöht den Passiven und Aktiven Klick um 10", 10, 10, 0);
            ////Passive Clicks
            //ShopItem schlaf = new ShopItem(100, "Schlafen", "Erhöht den Passive um 10", 10, 0, 0);
            
            ////Add Items
            ////ListItems.Add(hanteln);
            ////ListItems.Add(gHanteln);
            ////ListItems.Add(protein);
            ////ListItems.Add(schlaf);
            //*/

            //Prestige
            PrestigeItem gutesBett = new PrestigeItem(100000, "Gutes Bett", "Verringert die Kosten durch 2 von normalen Shop items", 2,0);
            PrestigeItem guteHanteln = new PrestigeItem(400000, "Gute Hanteln", "Verringert die Kosten durch 3 von normalen Shop items", 3,0);
            PrestigeItem perfekteHanteln = new PrestigeItem(800000, "Perfekte Hanteln", "Verringert die Kosten durch 4 von normalen Shop items", 4,0);
                      

            prestigeItems.Add(gutesBett);
            prestigeItems.Add(guteHanteln);
            prestigeItems.Add(perfekteHanteln);

            #region Gespeicherten Fortschritt aufrufen
            SpielstandDTB spielstand = new SpielstandDTB();

            List<int> countList = new List<int>();
            countList = spielstand.GetSpielstand(user);

            ListItems = spielstand.GetShopItems();

            //Shop mit Items erstellen
            foreach (ShopItem items in ListItems)
            {
                lstbx_shopitems.Items.Add(items);
            }
            lbl_Points.Content = points.ToString();

            foreach (PrestigeItem item in prestigeItems)
            {
                lstbx_shopitemsPrestige.Items.Add(item);
            }
            #endregion

            if (countList.Count > 0)
            {
                //Übernimmt Punkte
                points = countList[0];

                ////Übernimmt Hantel
                //ShopItem item = (ShopItem)lstbx_shopitems.Items[0];
                //for (int i = 0; i < countList[1]; i++)
                //{
                //    item.Cost *= 2;
                //    clicker.ActiveClick += item.UpgradeA;
                //    clicker.PassiveClick += item.UpgradeP;
                //}
                //lbl_Points.Content = points.ToString();

                ////Übernimmt Goldene Hanteln
                //item = (ShopItem)lstbx_shopitems.Items[1];
                //for (int i = 0; i < countList[2]; i++)
                //{
                //    item.Cost *= 2;
                //    clicker.ActiveClick += item.UpgradeA;
                //    clicker.PassiveClick += item.UpgradeP;
                //}
                //lbl_Points.Content = points.ToString();

                ////Übernimmt Protein
                //item = (ShopItem)lstbx_shopitems.Items[2];
                //for (int i = 0; i < countList[3]; i++)
                //{
                //    item.Cost *= 2;
                //    clicker.ActiveClick += item.UpgradeA;
                //    clicker.PassiveClick += item.UpgradeP;
                //}
                //lbl_Points.Content = points.ToString();

                ////Übernimmt Schlaf
                //item = (ShopItem)lstbx_shopitems.Items[3];
                //for (int i = 0; i < countList[4]; i++)
                //{
                //    item.Cost *= 2;
                //    clicker.ActiveClick += item.UpgradeA;
                //    clicker.PassiveClick += item.UpgradeP;
                //}
                //lbl_Points.Content = points.ToString();

                //zeigt alles nochmal richtig an
                lstbx_shopitems.Items.Refresh();

                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
                lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);
            }

            #endregion


            #region Timer (1 Sec)

            ////Timer für jede Sekunde
            //DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            //{
            //    //this.dateText.Content = DateTime.Now.ToString("ss");
            //    points += clicker.PassiveClick;
            //    lbl_Points.Content = points.ToString();

            //    //clicksPerSecond = 0;
            //    lbl_Clicks.Content = clicksPerSecond.ToString();


            //    // Lässt den PowerUp-Button mit einer 1%-Wahrscheinlichkeit spawnen
            //    if(rnd.Next(0,5) == 1)
            //    {
            //        bt_powerUP_spawn();
            //    }
            //    else
            //    {
            //        //bt_powerUP.IsEnabled = false;
            //        //bt_powerUP.Visibility = Visibility.Hidden;
            //    }

                
                
            //}, this.Dispatcher);
            #endregion
        }
        private void lstbx_shopitems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstbx_shopitems.SelectedItem != null)
            {
                ShopItem item = (ShopItem)lstbx_shopitems.SelectedItem;

                //Check, ob points vorhanden sind | Kosten erhöhen und A/P klick verbessern
                if (item.Cost <= points)
                {                        
                    points -= item.Cost;
                    item.Cost *= 2;
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                    lbl_Points.Content = points.ToString();
                    item.Amount++;
                }
                else
                {
                    MessageBox.Show("Nicht genügend Geld");
                }
                lstbx_shopitems.Items.Refresh();

                //Aktiven Clicker und Passiven Clicker darstellen
                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
                lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WriteToLabel();

            //GetBonusPoints(20);
        }

        private void WriteToLabel()
        {
            //Button Aktive Click
            points += clicker.ActiveClick;
            lbl_Points.Content = points.ToString();


            // Clicks per Second
            clicksPerSecond++;
            lastClicks.Add(clicksPerSecond);
            //ClicksPerSecond();
        }


        private void ClicksPerSecond()
        {
            if (lastClicks.Count >= 10)
            {
                lastClicks.RemoveAt(0);
            }

            float avgCPS = 0;
            int cpsSum = 0;

            foreach (var number in lastClicks)
            {
                cpsSum += number;
            }

            avgCPS = cpsSum / lastClicks.Count;

            lbl_Clicks.Content = avgCPS;
        }

        private void GetBonusPoints(int bonusPoints)
        {
            if (Convert.ToInt32(lbl_Clicks.Content) >= 10)
            {
                points *= bonusPoints;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {   
            SpielstandDTB spielstand = new SpielstandDTB();
            List<int> countList = new List<int>();

            //Zähler für Hanteln
            //ShopItem item = (ShopItem)lstbx_shopitems.Items[0];
            //countList.Add(item.Amount);

            foreach (var item in lstbx_shopitems.Items)
            {
                countList.Add((item as ShopItem).Amount);
            }



            spielstand.SaveSpielstand(points, countList, user);
            
            Close();
        }

        private void bt_ten_Click(object sender, RoutedEventArgs e)
        {
            multiplyer = 10;
            MultiplyItemCost();

            bt_ten.IsEnabled = false;
            bt_five.IsEnabled = false;
            bt_one.IsEnabled = true;
        }

        private void bt_five_Click(object sender, RoutedEventArgs e)
        {
            multiplyer = 5;
            MultiplyItemCost();

            bt_ten.IsEnabled = false;
            bt_five.IsEnabled = false;
            bt_one.IsEnabled = true;
        }

        private void bt_one_Click(object sender, RoutedEventArgs e)
        {

            ReduceItemCost();
            multiplyer = 1;

            bt_ten.IsEnabled = true;
            bt_five.IsEnabled = true;
            bt_one.IsEnabled = false;
        }

        private void MultiplyItemCost()
        {
            foreach (ShopItem item in ListItems)
            {
                for (int i = 0; i < multiplyer; i++)
                {
                    item.Cost *= 2;
                    
                }

                item.UpgradeA *= multiplyer;
                item.UpgradeP *= multiplyer;
            }
            
            lstbx_shopitems.Items.Refresh();
        }

        private void ReduceItemCost()
        {
            foreach (ShopItem item in ListItems)
            {
                for (int i = 0; i < multiplyer; i++)
                {
                    item.Cost /= 2;
                }

                item.UpgradeA /= multiplyer;
                item.UpgradeP /= multiplyer;
            }
            lstbx_shopitems.Items.Refresh();
        }

        //Dennis Martens
        private void bt_powerUP_spawn()
        {
            int duration = 5;
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (duration > 0)
                {
                    duration--;
                }
                else
                {
                    bt_powerUP.Visibility = Visibility.Hidden;
                    bt_powerUP.IsEnabled = false;
                }

            }, this.Dispatcher);

            bt_powerUP.Visibility = Visibility.Visible;
            bt_powerUP.IsEnabled = true;
            MainWindow m = new MainWindow("");

            //Höhe und Breite des MainWindows
            var width = Window.GetWindow(m).Width;
            var height = Window.GetWindow(m).Height;

            //Zufallskoordinaten für den PowerUP-Button
            double rndWidth = rnd.Next(50, (int)width - 50);
            double rndHeight = rnd.Next(50, (int)height - 50);

            bt_powerUP.Margin = new Thickness(rndWidth, rndHeight, 0, 0);
        }

        private void bt_powerUP_Click(object sender, RoutedEventArgs e)
        {
            powerUPeffect(10);
            int duration = 30;
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, duration), DispatcherPriority.Normal, delegate
            {
                duration = 30;
                clicker.ActiveClick /= 10;
            }, this.Dispatcher);

            bt_powerUP.IsEnabled = false;
            bt_powerUP.Visibility = Visibility.Hidden;

            lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
            lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);

        }

        private void powerUPeffect(int multiplier)
        {
            clicker.ActiveClick *= multiplier;
        }

        private void lstbx_shopitemsPrestige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstbx_shopitemsPrestige.SelectedItem != null)
            {
                PrestigeItem item = (PrestigeItem)lstbx_shopitemsPrestige.SelectedItem;

                //Check, ob points vorhanden sind | Kosten erhöhen und A/P klick verbessern
                if (item.Cost <= points)
                {
                    points -= item.Cost;
                    foreach (ShopItem shopitem in lstbx_shopitems.Items)
                    {
                        if (shopitem.Cost / item.Advantage == 0)
                        {
                            shopitem.Cost = 1;
                        }
                        else
                        {
                            shopitem.Cost /= item.Advantage;
                        }
                        
                    }
                    item.Cost *= 2;
                    lbl_Points.Content = points.ToString();
                }
                else
                {
                    MessageBox.Show("Nicht genügend Geld");
                }
                

            }
            lstbx_shopitems.Items.Refresh();
            lstbx_shopitemsPrestige.Items.Refresh();
        }
    }
}
