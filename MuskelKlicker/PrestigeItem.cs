using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuskelKlicker
{
    class PrestigeItem
    {
        private int cost;
        private string name;
        private string description;
        private int advantage;

        public int Cost { get => cost; set => cost = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int Advantage { get => advantage; set => advantage = value; }

        public PrestigeItem(int cost, string name, string description, int advantage)
        {
            Cost = cost;
            Name = name;
            Description = description;
            Advantage = advantage;

        }

        public override string ToString()
        {
            return string.Format("Cost: {0} \n Name: {1} \n Description: {2}", Cost, Name, Description);
        }
    }
}
