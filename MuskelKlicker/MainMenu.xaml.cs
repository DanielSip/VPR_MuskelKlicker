using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace MuskelKlicker
{
    /// <summary>
    /// Interaktionslogik für MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        string user = "";
        List<string> spieler = new List<string>();
        int pos = -1;

        public MainMenu()
        {
            InitializeComponent();
        }

        // Beenden-Button
        //By Lars Stuhlmacher
        private void bt_end_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Daniel Sippel
        /// Öffnet den Alten Spielstand und gibt ihn das Profil mit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_oldGame_Click(object sender, RoutedEventArgs e)
        {
            if (user != "")
            {
                MainWindow game = new MainWindow(user);
                Close();
                game.Show();
            }
            
        }

        /// <summary>
        /// Daniel Sippel
        /// Öffnet den Alten Spielstand und gibt ihn das Profil mit das in der Textbox ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_newGame_Click(object sender, RoutedEventArgs e)
        {
            SpielstandDTB spielstand = new SpielstandDTB();
            lb_Name.Visibility = Visibility.Visible;
            txt_newName.Visibility = Visibility.Visible;

            if (txt_newName.Text != "")
            {
                
                
                if (!spielstand.UserExists(txt_newName.Text))
                {
                    user = txt_newName.Text;
                    MainWindow game = new MainWindow(user);
                    Close();
                    game.Show();
                }
                else
                {
                    MessageBox.Show("Benutzer gibt es Bereits");
                }
            }          
                       
        }

        /// <summary>
        /// Daniel Sippel
        /// Nimmt den nächsten Spieler in der Liste wenn er auf max ist wir des wieder auf 0 gesetzt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if (spieler.Count > pos + 1)
            {
                pos++;
                lb_User.Content = spieler[pos];
                user = spieler[pos];
            }
            else
            {
                if (spieler.Count != 0)
                {
                    user = spieler[0];
                    lb_User.Content = spieler[0];
                    pos = 0;
                }
                else
                {
                    lb_User.Content = "";
                }
            }
        }

        /// <summary>
        /// Daniel Sippel
        /// Nimmt alle Users und fügt sie einer Liste hinzu dabei versteckt er den oberen Label und die Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu1_Loaded(object sender, RoutedEventArgs e)
        {
            SpielstandDTB spielstand = new SpielstandDTB();

            spieler = spielstand.GetUsers();

            if (spieler.Count != 0)
            {
                lb_User.Content = spieler[0];
                pos = 0;
                user = spieler[0];

                lb_Name.Visibility = Visibility.Hidden;
                txt_newName.Visibility = Visibility.Hidden;
            }
            
        }


        /// <summary>
        /// Daniel Sippel
        /// Löscht den User der gerade aktiv in der Liste ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_DeleteSave_Click(object sender, RoutedEventArgs e)
        {
            SpielstandDTB spielstand = new SpielstandDTB();
            spielstand.DeleteSpielstand(user);

            spieler.RemoveAt(pos);

            MessageBox.Show("User " + user + " wurde gelöscht");

            bt_ChangeUser_Click(sender, e);
            user = "";



        }

        /// <summary>
        /// Daniel Sippel
        /// öffnet das Credits Fenster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Credits_Click(object sender, RoutedEventArgs e)
        {
            Credits credits = new Credits();
            Close();
            credits.Show();
        }
    }
}
