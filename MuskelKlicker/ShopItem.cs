using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuskelKlicker
{
    class ShopItem
    {
        private int cost;
        private string name;
        private string description;
        private int upgradeP;
        private int upgradeA;

        public int Cost { get => cost; set => cost = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int UpgradeP { get => upgradeP; set => upgradeP = value; }
        public int UpgradeA { get => upgradeA; set => upgradeA = value; }

        public ShopItem(int cost, string name, string description, int upgradeP, int upgradeA)
        {
            Cost = cost;
            Name = name;
            Description = description;
            UpgradeA = upgradeA;
            UpgradeP = upgradeP;
        }


        public bool EnoughPoints(int points, ShopItem currentItem)
        {
            if (currentItem.cost < points)
            {
                points -= currentItem.Cost;
                currentItem.Cost *= 2;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("Cost:{0} \n Name:{1} \n Description:{2}", Cost, Name, Description);
        }
    }
}
