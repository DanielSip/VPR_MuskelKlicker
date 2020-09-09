using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.SqlServer.Server;

namespace MuskelKlicker
{
    class SpielstandDTB
    {
        private OleDbConnection verbindung;

        public SpielstandDTB()
        {
            verbindung = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;" +
                                             @"Data Source=../../../Datenbanken.accdb");
            verbindung.Open();
        }

        public List<int> GetSpielstand(string user)
        {
            string sql = string.Format("select * from Spielstand WHERE Spielername = '{0}'",user);
            OleDbCommand kommando = new OleDbCommand(sql, verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            List<int> SpielerDaten = new List<int>();

            while (reader.Read())
            {               
                for (int i = 1; i <= 5; i++)
                {
                    SpielerDaten.Add(reader.GetInt32(i));
                }
            }
            return SpielerDaten;
                
        }

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

        //public void SaveSpielstand(int scoreAnzahl, int hantelAnzahl, int goldeneHantelnAnzahl, int proteinAnzahl, int schlafAnzahl, string user)
        public void SaveSpielstand(int scoreAnzahl, List<int> anzahlenItems, string user)
        {
            List<ShopItem> shopItems = new List<ShopItem>();
            shopItems = GetShopItems();
            

            OleDbCommand kommando = new OleDbCommand("SELECT id FROM Spielstand Where Spielername = '"+ user +"'", verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            string cmd;

            if (!reader.HasRows)
            {
                cmd = "INSERT INTO Spielstand ( Score,";

                for (int i = 0; i < shopItems.Count; i++)
                {

                    cmd += shopItems[i].Name + ",";

                }

                cmd += "Spielername) Values (" + scoreAnzahl + ",";

                for (int i = 0; i < shopItems.Count; i++)
                {

                    cmd += anzahlenItems[i] + ", ";

                       
                }
                cmd += "'" + user + "')";

                //kommando = new OleDbCommand($"INSERT INTO Spielstand (Score, Hanteln, GoldeneHanteln, Protein, Schlafen, Spielername) " +
                //                                         $"VALUES ('{scoreAnzahl}', '{hantelAnzahl}', '{goldeneHantelnAnzahl}', '{proteinAnzahl}', '{schlafAnzahl}', '{user}')",
                //                                         verbindung);
            }
            else
            {
                //kommando = new OleDbCommand($"UPDATE Spielstand Set Score = '{scoreAnzahl}', Hanteln ={hantelAnzahl}, GoldeneHanteln ={goldeneHantelnAnzahl}, Protein={proteinAnzahl}, Schlafen={schlafAnzahl} Where Spielername = '" + user + "'",
                //                                                     verbindung);

                cmd = "UPDATE Spielstand Set Score = "+ scoreAnzahl +",";

                for (int i = 0; i < shopItems.Count; i++)
                {
                    if (i < shopItems.Count - 1)
                    {
                        cmd += shopItems[i].Name + "=" + anzahlenItems[i] + ",";
                    }
                    else
                    {
                        cmd += shopItems[i].Name + "=" + anzahlenItems[i] + " Where Spielername = '" + user +"'";
                    }
                }
            }
            kommando = new OleDbCommand(cmd,verbindung);
            kommando.ExecuteNonQuery();
            verbindung.Close();
        }

        public void DeleteSpielstand(string user)
        {

            //OleDbCommand kommando = new OleDbCommand($"INSERT INTO Spielstand (Score, Hanteln, GoldeneHanteln, Protein, Schlafen) " +
            //                                         $"VALUES ('{scoreAnzahl}', '{hantelAnzahl}', '{goldeneHantelnAnzahl}', '{proteinAnzahl}', '{schlafAnzahl}')",
            //                                         verbindung);

            OleDbCommand kommando = new OleDbCommand("DELETE from Spielstand WHERE Spielername = '"+ user +"'",
                                                     verbindung);
            kommando.ExecuteNonQuery();
        }

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
    }
}
