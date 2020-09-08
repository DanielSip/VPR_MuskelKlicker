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

        public void SaveSpielstand(int scoreAnzahl, int hantelAnzahl, int goldeneHantelnAnzahl, int proteinAnzahl, int schlafAnzahl, string user)
        {

            

            OleDbCommand kommando = new OleDbCommand("SELECT id FROM Spielstand Where Spielername = '"+ user +"'", verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            if (!reader.HasRows)
            {
                kommando = new OleDbCommand($"INSERT INTO Spielstand (Score, Hanteln, GoldeneHanteln, Protein, Schlafen, Spielername) " +
                                                         $"VALUES ('{scoreAnzahl}', '{hantelAnzahl}', '{goldeneHantelnAnzahl}', '{proteinAnzahl}', '{schlafAnzahl}', '{user}')",
                                                         verbindung);
            }
            else
            {
                kommando = new OleDbCommand($"UPDATE Spielstand Set Score = '{scoreAnzahl}', Hanteln ={hantelAnzahl}, GoldeneHanteln ={goldeneHantelnAnzahl}, Protein={proteinAnzahl}, Schlafen={schlafAnzahl} Where Spielername = '" + user + "'",
                                                                     verbindung);
            }
          
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
