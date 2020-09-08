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

        private void bt_oldGame_Click(object sender, RoutedEventArgs e)
        {
            if (user != "")
            {
                MainWindow game = new MainWindow(user);
                Close();
                game.Show();
            }
            
        }

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

        private void bt_DeleteSave_Click(object sender, RoutedEventArgs e)
        {
            SpielstandDTB spielstand = new SpielstandDTB();
            spielstand.DeleteSpielstand(user);

            spieler.RemoveAt(pos);

            MessageBox.Show("User " + user + " wurde gelöscht");

            bt_ChangeUser_Click(sender, e);
            user = "";



        }

        private void bt_Credits_Click(object sender, RoutedEventArgs e)
        {
            Credits credits = new Credits();
            Close();
            credits.Show();
        }
    }
}
