using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuskelKlicker
{
    class Clicker
    {
        private int passiveClick;
        private int activeClick;

        public int PassiveClick { get => passiveClick; set => passiveClick = value; }
        public int ActiveClick { get => activeClick; set => activeClick = value; }

        public Clicker(int passiveClick, int activeClick)
        {
            PassiveClick = passiveClick;
            ActiveClick = activeClick;
        }
    }
}
