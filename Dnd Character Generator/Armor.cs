using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnd_Character_Generator
{
    class Armor
    {
        private string name;
        private int armorClass;
        private int cost;
        private int weight;

        public Armor(string name)
        { 
            if (name == "Leather") {
                armorClass = 7;
                cost = 20;
                weight = 200;
            } else if (name == "Chain mail") {
                armorClass = 5;
                cost = 40;
                weight = 400;
            } else if (name == "Plate mail") {
                armorClass = 3;
                cost = 60;
                weight = 500;
            } else if (name == "Shield") {
                armorClass = -1;
                cost = 10;
                weight = 100;
            } else {
                name = "Unarmored";
                armorClass = 9;
                cost = 0;
                weight = 0;
            }
        }
    }
}
