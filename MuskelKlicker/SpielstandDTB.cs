using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.SqlServer.Server;

namespace MuskelKlicker
{
    public class SpielstandDTB
    {
        //Daniel Sippel

        private OleDbConnection verbindung;
        private int itemAddAmount = 0;

        /// <summary>
        /// Daniel Sippel
        /// Erstellt eine neue SpielstandDTB mit festen werten der Datenbank
        /// </summary>
        public SpielstandDTB()
        {
            verbindung = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;" +
                                             @"Data Source=../../../../Datenbanken.accdb");
            verbindung.Open();
        }

        /// <summary>
        /// Daniel Sippel
        /// Liest alle Daten aus dem Spielstand und fügt sie dann der Spielerdaten liste hinzu
        /// </summary>
        /// <param name="user">User ist der name des Profils</param>
        /// <returns>Liste von den Spielerdaten die aus ints bestehen</returns>
        public List<int> GetSpielstand(string user)
        {
            string sql = string.Format("select * from Spielstand WHERE Spielername = '{0}'",user);
            OleDbCommand kommando = new OleDbCommand(sql, verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            List<int> SpielerDaten = new List<int>();

            for (int i = reader.FieldCount; i >= 5; i--)
            {
                    itemAddAmount++;
            }
            

            while (reader.Read())
            {               
                for (int i = 1; i <= reader.FieldCount - itemAddAmount; i++)
                {
                    SpielerDaten.Add(reader.GetInt32(i));
                }
            }
            return SpielerDaten;
                
        }

        /// <summary>
        /// Daniel Sippel
        /// Liest die Shopitems aus der Itemdatenbank und erstellt neue items die er dann in die Liste packt
        /// </summary>
        /// <returns>Return alle items aus der Datenbank</returns>
        public List<ShopItem> GetShopItems()
        {
            List<ShopItem> itemListe = new List<ShopItem>();

            OleDbCommand kommando = new OleDbCommand("SELECT * FROM Itemdatenbank", verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            while (reader.Read())
            {
                itemListe.Add(new ShopItem(reader.GetInt32(1),reader.GetString(2),reader.GetString(3),reader.GetInt32(4),reader.GetInt32(5),0));
            }

            return itemListe;
        }

        /// <summary>
        /// Daniel Sippel
        /// Liest nur die Namen aus dem Spielstand und fügt sie der Liste hinzu
        /// </summary>
        /// <returns>returnt alle Spielernamen</returns>
        public List<string> GetUsers()
        {
            List<string> spielerListe = new List<string>();

            OleDbCommand kommando = new OleDbCommand("SELECT Spielername FROM Spielstand", verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            while (reader.Read())
            {
                spielerListe.Add(reader.GetString(0));
            }            

            return spielerListe;
        }

        /// <summary>
        /// Daniel Sippel
        /// Guckt ob der Spielstand existiert wenn ja dann updatet er die Stände wenn nein dann erstellt er einen Neuen Spielstand
        /// </summary>
        /// <param name="scoreAnzahl">Die Punkte die man im Moment hat</param>
        /// <param name="anzahlenItems">Die Anzahl der gekauften Items</param>
        /// <param name="user">Name des Profils</param>
        public void SaveSpielstand(int scoreAnzahl, List<int> anzahlenItems, string user)
        {
            List<ShopItem> shopItems = new List<ShopItem>();
            shopItems = GetShopItems();

            string cmd;

            if (!UserExists(user))
            {
                cmd = "INSERT INTO Spielstand ( Score,";

                for (int i = 0; i < shopItems.Count; i++)
                {
                    cmd += "[" + shopItems[i].Name + "],";
                }

                cmd += "[Spielername]) Values (" + scoreAnzahl + ",";

                for (int i = 0; i < shopItems.Count; i++)
                {

                    cmd += anzahlenItems[i] + ", ";

                       
                }
                cmd += "'" + user + "')";
            }
            else
            {
                cmd = "UPDATE Spielstand Set Score = "+ scoreAnzahl +",";

                for (int i = 0; i < shopItems.Count; i++)
                {
                    if (i < shopItems.Count - 1)
                    {
                        cmd += "["+shopItems[i].Name + "]=" + anzahlenItems[i] + ",";
                    }
                    else
                    {
                        cmd += "[" +shopItems[i].Name + "]=" + anzahlenItems[i] + " Where Spielername = '" + user +"'";
                    }
                }
            }
            OleDbCommand kommando = new OleDbCommand(cmd,verbindung);
            kommando.ExecuteNonQuery();
            verbindung.Close();
        }

        /// <summary>
        /// Daniel Sippel
        /// Löscht den Spielstand des Profils
        /// </summary>
        /// <param name="user">Name des Profils</param>
        public void DeleteSpielstand(string user)
        {
            OleDbCommand kommando = new OleDbCommand("DELETE from Spielstand WHERE Spielername = '"+ user +"'",
                                                     verbindung);
            kommando.ExecuteNonQuery();
        }

        /// <summary>
        /// Uberprüft ob das Profil existiert
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True wenn das Profil schon vorhanden ist</returns>
        public Boolean UserExists(string user)
        {
            OleDbCommand kommando = new OleDbCommand("SELECT id FROM Spielstand Where Spielername = '" + user + "'", verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            if (!reader.HasRows)
            {
                return false;
            } 

            return true;
        }

        /// <summary>
        /// Fügt das Item der itemdatenbank hinzu und added dann eine Spalte mit dem itemname der Spielstand Datenbank hinzu
        /// </summary>
        /// <param name="cost">Die Kosten des Items</param>
        /// <param name="name">Der Name des Items</param>
        /// <param name="description">Die Beschreibung des Items</param>
        /// <param name="passive">Die passiven Punkte durch das Kaufen des Items</param>
        /// <param name="active">Die aktiven Punkte durch das Kaufen des Items</param>
        /// <returns></returns>
        public Boolean AddItem(int cost, string name, string description, int passive, int active)
        {
            OleDbCommand kommando = new OleDbCommand("SELECT id FROM Itemdatenbank Where itemname = '" + name + "'", verbindung);

            OleDbDataReader reader = kommando.ExecuteReader();

            if (reader.HasRows)
            {
                return false;
            }
            reader.Close();

            kommando = new OleDbCommand($"INSERT INTO itemdatenbank (Kosten, Itemname, Beschreibung, UpgradeP, UpgradeA) " +
                                                     $"VALUES ('{cost}', '{name}', '{description}', '{passive}', '{active}')", verbindung);
            kommando.ExecuteNonQuery();

            kommando = new OleDbCommand("ALTER TABLE Spielstand ADD " + name + " int ",verbindung);

            kommando.ExecuteNonQuery();

            kommando = new OleDbCommand("UPDATE Spielstand Set " + name + " = 0",verbindung);

            kommando.ExecuteNonQuery();

            return true;
        }

        /// <summary>
        /// Entfernt das Item aus der itemdatenbank und entfern die extra Spalte aus dem Spielstand
        /// </summary>
        /// <param name="name">der Name des Items das Gelöscht werden soll</param>
        public void DeleteItem(string name)
        {
            OleDbCommand kommando = new OleDbCommand("DELETE from itemdatenbank WHERE itemname = '" + name + "'",
                                                     verbindung);
            kommando.ExecuteNonQuery();

            kommando = new OleDbCommand("ALTER TABLE Spielstand DROP COLUMN "+ name, verbindung);

            kommando.ExecuteNonQuery();
        }
    }
}
