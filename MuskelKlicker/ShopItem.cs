using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuskelKlicker
{
    public class ShopItem
    {
        private int cost;
        private string name;
        private string description;
        private int upgradeP;
        private int upgradeA;
        private int amount;

        public int Cost { get => cost; set => cost = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int UpgradeP { get => upgradeP; set => upgradeP = value; }
        public int UpgradeA { get => upgradeA; set => upgradeA = value; }
        public int Amount { get => amount; set => amount = value; }

        public ShopItem(int cost, string name, string description, int upgradeP, int upgradeA, int amount)
        {
            Cost = cost;
            Name = name;
            Description = description;
            UpgradeP = upgradeP;
            UpgradeA = upgradeA;
            Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("Cost: {0} \n Name: {1} \n Description: {2}", Cost, Name, Description);
        }
    }
}
