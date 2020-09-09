using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.SqlServer.Server;

namespace ItemHinzufügen
{
    class ItemsDTB
    {
        private OleDbConnection verbindung;
        private ole

        public ItemsDTB()
        {
            verbindung = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;" +
                                             @"Data Source=../../../Datenbanken.accdb");
            verbindung.Open();
        }
        public List<ShopItem> GetShopItems()
        {
            List<ShopItem> itemListe = new List<ShopItem>();

            OleDbCommand kommando = new OleDbCommand("SELECT * FROM Itemdatenbank", verbindung);
            OleDbDataReader reader = kommando.ExecuteReader();

            while (reader.Read())
            {
                itemListe.Add(new ShopItem(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), 0));
            }

            return itemListe;
        }
    }
}
