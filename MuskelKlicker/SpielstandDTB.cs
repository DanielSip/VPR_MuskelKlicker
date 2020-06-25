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
                                             @"Data Source=../../Datenbanken.accdb");
            verbindung.Open();
        }

        public List<int> GetSpielstand()
        {
            string sql = "select * from Spielstand";
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

        public void SaveSpielstand(int scoreAnzahl, int hantelAnzahl, int goldeneHantelnAnzahl, int proteinAnzahl, int schlafAnzahl)
        {

            //OleDbCommand kommando = new OleDbCommand($"INSERT INTO Spielstand (Score, Hanteln, GoldeneHanteln, Protein, Schlafen) " +
            //                                         $"VALUES ('{scoreAnzahl}', '{hantelAnzahl}', '{goldeneHantelnAnzahl}', '{proteinAnzahl}', '{schlafAnzahl}')",
            //                                         verbindung);

            OleDbCommand kommando = new OleDbCommand($"UPDATE Spielstand Set Score = '{scoreAnzahl}', Hanteln ={hantelAnzahl}, GoldeneHanteln ={goldeneHantelnAnzahl}, Protein={proteinAnzahl}, Schlafen={schlafAnzahl} Where Spielername = 'Test'",                                                
                                                     verbindung);
            kommando.ExecuteNonQuery();
        }

        public void DeleteSpielstand()
        {

            //OleDbCommand kommando = new OleDbCommand($"INSERT INTO Spielstand (Score, Hanteln, GoldeneHanteln, Protein, Schlafen) " +
            //                                         $"VALUES ('{scoreAnzahl}', '{hantelAnzahl}', '{goldeneHantelnAnzahl}', '{proteinAnzahl}', '{schlafAnzahl}')",
            //                                         verbindung);

            OleDbCommand kommando = new OleDbCommand($"UPDATE Spielstand Set Score = 0, Hanteln = 0, GoldeneHanteln = 0, Protein = 0, Schlafen = 0 Where Spielername = 'Test'",
                                                     verbindung);
            kommando.ExecuteNonQuery();
        }
    }
}
