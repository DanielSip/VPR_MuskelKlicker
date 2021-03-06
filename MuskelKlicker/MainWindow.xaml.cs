﻿using System;
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
    /// <summary>
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

        bool isPoweredUp = false;

        DateTime poweredTime = new DateTime();
        int clicksToLower = 0;

        int cpsLabel = 0;
        DateTime lastValue;



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Shop und Item Update | Andrew John Lariat
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

            #region Gespeicherten Fortschritt aufrufen | Daniel Sippel
            SpielstandDTB spielstand = new SpielstandDTB();

            List<int> countList = new List<int>();
            countList = spielstand.GetSpielstand(user);

            ListItems = spielstand.GetShopItems();

            //Shop mit Items erstellen
            foreach (ShopItem items in ListItems)
            {              
                lstbx_shopitems.Items.Add(items);
                lstbx_shopitems.Items.Refresh();
            }

            for (int i = 1; i < countList.Count; i++)
            {
                for (int j = 0; j < countList[i]; j++)
                {
                    ListItems[i-1].Cost *= 2;
                }
                ListItems[i - 1].Amount = countList[i];
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
                lbl_Points.Content = points.ToString();
                foreach (ShopItem sItem in ListItems)
                {
                    
                    clicker.ActiveClick += sItem.UpgradeA * sItem.Amount;
                    clicker.PassiveClick += sItem.UpgradeP * sItem.Amount;
                }

                

                //zeigt alles nochmal richtig an
                lstbx_shopitems.Items.Refresh();

                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
                lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);
            }

            #endregion

            #region Timer (1 Sec) | Andrew John Lariat / Dennis Martens

            //Timer für jede Sekunde
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //this.dateText.Content = DateTime.Now.ToString("ss");
                points += clicker.PassiveClick;
                lbl_Points.Content = points.ToString();

                //Klicks pro Sekunde sind zu beginn auf 0 gesetzt. Kann mit Upgrades erhöht werden.
                clicksPerSecond = 0;
                lbl_Clicks.Content = clicksPerSecond.ToString();


                // Lässt den PowerUp-Button mit einer 1%-Wahrscheinlichkeit spawnen
                if (rnd.Next(0, 2) == 1)
                {
                    bt_powerUP_spawn();
                }
                else
                {
                    //bt_powerUP.IsEnabled = false;
                    //bt_powerUP.Visibility = Visibility.Hidden;
                }

                powerDowneffect();
                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);

            }, this.Dispatcher);
            #endregion
        }

        #region ShopItem kauf Andrew John Lariat
        /// <summary>
        /// Kauft ShopItems und verringert die Punkte bei ein erfolgreichen Kauf
        /// </summary>
        private void lstbx_shopitems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstbx_shopitems.SelectedItem != null)
            {
                ShopItem item = (ShopItem)lstbx_shopitems.SelectedItem;

                //Check, ob points vorhanden sind | Kosten erhöhen und A/P klick verbessern
                if (item.Cost <= points)
                {                        
                    points -= item.Cost;
                    for (int i = 0; i < multiplyer; i++)
                    {
                        item.Cost *= 2;
                    }

                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                    lbl_Points.Content = points.ToString();
                    item.Amount += multiplyer;
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
        #endregion

        #region Mainbutton Points adden | Andrew John Lariat
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clicksPerSecond++;
            WriteToLabel();

            GetBonusPoints(20);
        }
        #endregion

        #region Write To Labels
        /// <summary>
        /// Methode die die Punkte ausgibt und die Klick/Sekunde angibt
        /// 
        /// ~ Lars Stuhlmacher und Andrew John Lariat
        /// </summary>
        private void WriteToLabel()
        {
            //Button Aktive Click
            points += clicker.ActiveClick;
            lbl_Points.Content = points.ToString();
            lbl_Clicks.Content = clicksPerSecond.ToString();



            
            int temp = clicksPerSecond;

            //Wenn die aktuellen clicks das neue Maximum sind, cpsLabel auf das Maximum setzen
            if (temp > cpsLabel)
            {
                cpsLabel = temp;
                lastValue = DateTime.Now;
            }
            else if (DateTime.Now > lastValue.AddSeconds(1.3))  //Nach 1,3 Sekunden unabhängig vom Maximum, cpsLabel auf die Clicks setzen
            {                                                       //Wird benötigt um die cps richtig auszugeben
                cpsLabel = temp;
                lastValue = DateTime.Now;
            }

            lbl_Clicks.Content = cpsLabel.ToString();

        }
#endregion

        #region Points bei genügend Klicks | Andrew John Lariat
        /// <summary>
        /// Fügt Punkte bonuspunkte bei genügend Klicks (7 Klicks pro Sekunde) hinzu
        /// </summary>
        /// <param name="bonusPoints">Die Bonuspunkte werden mal 10 genommen und auf den Normalen Punkten hinzugefügt</param>
        private void GetBonusPoints(int bonusPoints)
        {
            if (Convert.ToInt32(lbl_Clicks.Content) >= 7)
            {
                points += bonusPoints * 10;
            }
        }
        #endregion

        #region Speicherung des Profils | Daniel Sippel
        /// <summary>
        /// Daten von den Items in die Liste hinzufügen und dann Speichern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            Application.Current.Shutdown();
        }
        #endregion

        #region Multiplikator für Items | Andrew John Lariat
        /// <summary>
        /// Kauft das Shopitem 10 mal
        /// </summary>
        private void bt_ten_Click(object sender, RoutedEventArgs e)
        {
            multiplyer = 10;
            MultiplyItemCost();

            bt_ten.IsEnabled = false;
            bt_five.IsEnabled = false;
            bt_one.IsEnabled = true;
        }
        /// <summary>
        /// Kauft das Shopitem 5 mal
        /// </summary>
        private void bt_five_Click(object sender, RoutedEventArgs e)
        {
            multiplyer = 5;
            MultiplyItemCost();

            bt_ten.IsEnabled = false;
            bt_five.IsEnabled = false;
            bt_one.IsEnabled = true;
        }
        /// <summary>
        /// Kauft das Shopitem 1 mal
        /// </summary>
        private void bt_one_Click(object sender, RoutedEventArgs e)
        {

            ReduceItemCost();
            multiplyer = 1;

            bt_ten.IsEnabled = true;
            bt_five.IsEnabled = true;
            bt_one.IsEnabled = false;
        }

        /// <summary>
        /// Erhöht die Kosten, sodass man Shopitems kauft basierend auf den Multiplyer
        /// </summary>
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
        /// <summary>
        /// Verringert die Kosten, sodass man die standart Kosten bezahlen muss
        /// </summary>
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
        #endregion

        #region Power Up | Dennis Martens
        /// <summary>
        /// Steuert das erscheinen des PowerUp Buttons
        /// </summary>
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

        /// <summary>
        /// Nach dem Klicken des PowerUp Buttons werden die aktiven Klicks für 30 Sekunden verzehnfacht 
        /// 
        /// ~ Dennis Martens und Lars Stuhlmacher
        /// </summary>
        private void bt_powerUP_Click(object sender, RoutedEventArgs e)
        {
            isPoweredUp = true;
            powerUPeffect(10);

            if (isPoweredUp)
            {
                poweredTime = DateTime.Now;
            }
            

            bt_powerUP.IsEnabled = false;
            bt_powerUP.Visibility = Visibility.Hidden;

            lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
            lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);

        }

        /// <summary>
        /// Setzt die Werte des Powerups nach 10 Sekunden zurück.
        /// 
        /// ~Lars Stuhlmacher
        /// </summary>
        private void powerDowneffect()
        {
            if (isPoweredUp)
            {
                if (DateTime.Now >= poweredTime + new TimeSpan(0,0,10))
                {
                    MessageBox.Show("ist hier rein gegangen");
                    clicker.ActiveClick -= clicksToLower;
                    isPoweredUp = false;
                }
            }
        }

        /// <summary>
        /// Verzehnfacht die aktiven Klicks
        /// 
        /// ~Dennis Martens und Lars Stuhlmacher
        /// </summary>
        private void powerUPeffect(int multiplier)
        {
            int temp = clicker.ActiveClick;
            clicker.ActiveClick *= multiplier;
            clicksToLower = clicker.ActiveClick - temp;
        }
        #endregion

        #region PrestigeItems | Andrew John Lariat
        /// <summary>
        /// Kauft PrestigeItems und verringert die Punkte bei ein erfolgreichen Kauf
        /// </summary>
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
        #endregion
    }
}
