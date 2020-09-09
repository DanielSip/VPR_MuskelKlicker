using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuskelKlicker
{
    class PrestigeItem
    {
        //Andrew John Lariat
        private int cost;
        private string name;
        private string description;
        private int advantage;
        private int amount;

        public int Cost { get => cost; set => cost = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int Advantage { get => advantage; set => advantage = value; }
        public int Amount { get => amount; set => amount = value; }

        public PrestigeItem(int cost, string name, string description, int advantage, int amount)
        {
            Cost = cost;
            Name = name;
            Description = description;
            Advantage = advantage;
            Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("Cost: {0} \n Name: {1} \n Description: {2}", Cost, Name, Description);
        }
    }
}
