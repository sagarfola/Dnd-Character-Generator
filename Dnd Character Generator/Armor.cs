using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnd_Character_Generator
{
    class Armor
    {
        public string name;
        public int armorClass;
        public int cost;
        public int weight;

        public Armor(string choice)
        { 
            if (choice == "Leather") {
                name = "Leather";
                armorClass = 7;
                cost = 20;
                weight = 200;
            } else if (choice == "Chain mail") {
                name = "Chain mail";
                armorClass = 5;
                cost = 40;
                weight = 400;
            } else if (choice == "Plate mail") {
                name = "Plate Mail";
                armorClass = 3;
                cost = 60;
                weight = 500;
            } else if (choice == "Shield") {
                name = "Shield";
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
