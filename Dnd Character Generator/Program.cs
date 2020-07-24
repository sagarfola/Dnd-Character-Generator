using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dnd_Character_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            /* DND CHARACTER GENERATOR
             * 
             * The intial purpose of this program is to randomly generate a random level one character
             * using the 1981 Moldvay and Cook Basic and Expert versions of Dungeons and Dragons
             * 
             * It will first calculate ability scores and modifiers, and then use those values to select a class
             * at random based on what the scores are eligible for.
             * 
             * Goals for this program is to be able to assign starting gold, weapons and armour, and spells
             * Additional follow on goals are to be able to create a higher level character with the option
             * to generate possible magic items.
             * 
             * Finally, the ideal culmination is to be able to export this information onto a pdf or similar 
             * file for use by the player or GM
             *
             */

            // initialize variables needed for the program
            int[] abilityScore = new int[6];                            // Ability scores in index 0 to 5 that correspond to STR, INT, WIS, DEX, CON, CHA respectively
            int[] abilityMod = new int[6];                              // Ability score modifiers based on the +3 to -3 scale corresponding to the appropriate stat
            string charClass;                                           // Stores a string referring to a character classes and demihumans
            int armorClass, thaco;                                      // AC and Thac0 values
            int death, wands, paralysis, breath, spells;                // Saving throw values
            int opendoors, language, loyalty, max_retain, min_retain;   // Values to hold open door check value, # of languages known, loyalty, and min/max retainers 
            int melatk, rngatk;                                         // Attack modifiers
            int xp, xpNext, xpMod;                                      // Current Xp, XP to next level, Xp modifier based on prime requistite
            int hp;                                                     // Maximum HP
            int listenDoors, findTrap, findSecretDoor;                  // Standard listen at doors, find room traps, and finding secret doors


            // Define the random seed to use for all die rolls
            Random rnd = new Random();

            // ** GENERATE ABILITY SCORES **
            for (int i = 0; i < 6; i++)
            {
                abilityScore[i] = RollDice(3, 6);
            }

            // ** GENERATE ABILITY MODIFIERS **
            for (int i = 0; i < 6; i++)
            {
                abilityMod[i] = GetModifier(abilityScore[i]);
            }

            // ** CHOOSE CLASS **
            charClass = GetClass(abilityScore[1], abilityScore[3], abilityScore[4]);

            // ** USE MODIFIERS TO GENERATE VARIOUS SCORES
            opendoors = 2 + abilityMod[0];      // Open Doors Check

            melatk = abilityMod[0];             // Melee attack bonus

            language = abilityMod[1];           // Additional Languages known

            if (charClass == "Halfling") {      // Range attack bonus (Halflings get an additional +1)
                rngatk = abilityMod[3] + 1;
            } else {
                rngatk = abilityMod[3];
            }

            if (charClass == "Elf") {           // Listen at doors, find traps, and find secret door checks
                listenDoors = 2;
                findSecretDoor = 2;
                findTrap = 1;
            } else if (charClass == "Dwarf") {
                listenDoors = 2;
                findTrap = 2;
                findSecretDoor = 1;
            } else if (charClass == "Halfling") {
                listenDoors = 2;
                findTrap = 1;
                findSecretDoor = 1;
            } else {
                listenDoors = 1; 
                findTrap = 1; 
                findSecretDoor = 1;
            }

            // ** GENERATE HIT POINTS
            hp = GetHP(charClass, abilityMod[4]);

            Console.WriteLine("Class: " + charClass);
            Console.WriteLine("Max HP: " + hp);
            Console.WriteLine("Str: " + abilityScore[0] + " | " + abilityMod[0]);
            Console.WriteLine("Int: " + abilityScore[1] + " | " + abilityMod[1]);
            Console.WriteLine("Wis: " + abilityScore[2] + " | " + abilityMod[2]);
            Console.WriteLine("Dex: " + abilityScore[3] + " | " + abilityMod[3]);
            Console.WriteLine("Con: " + abilityScore[4] + " | " + abilityMod[4]);
            Console.WriteLine("Cha: " + abilityScore[5] + " | " + abilityMod[5]);
            Console.WriteLine("Melee Attack Mod: " + melatk);
            Console.WriteLine("Ranged Attack Mod: " + rngatk);
            Console.WriteLine("Additional Languages Known: " + language);
            Console.WriteLine("Open Doors: " + opendoors + "-in-6");
            Console.WriteLine("Listen at Doors: " + listenDoors + "-in-6");
            Console.WriteLine("Find Room Traps: " + findTrap + "-in-6");
            Console.WriteLine("Find Secret Doors: " + findSecretDoor + "-in-6");


            ReadCsv("C:/Users/sagar/source/repos/Dnd Character Generator/Dnd Character Generator/SavingThrows_THAC0.csv");


            Console.ReadLine();

            // *** FUNCTIONS UTILIZING RANDOM VALUES
            int RollDice(int dieNum, int dieSize)
                // This function rolls a [dieNum] number of dice of
                //   [dieSize] size and adds the results which is passed
                //   in the result
            {
                int[] rolls = new int[dieSize];

                for (int i = 1; i <= dieNum; i++)
                {
                    rolls[i] = rnd.Next(1, dieSize + 1);
                }

                int result = rolls.Sum();

                return result;
            }

            string GetClass(int intelligence, int dexterity, int constitution)
            {
                // This method randomly chooses a character class accounting for class restrictions
                //  currently does not optimize class choice

                int classChoice = 0;    // stores the random choice value to select the class based on a switch case
                string result;

                if (intelligence < 9 && constitution < 9)  // Only Human Classes
                {
                    classChoice = rnd.Next(1, 5);
                } else if (intelligence > 8 && constitution < 9)  // Elf, No Dwarf, No Halfling
                {
                    classChoice = rnd.Next(1, 6);
                } else if (intelligence < 9 && dexterity > 8 && constitution > 8)    // No Elf
                {
                    classChoice = rnd.Next(1, 8);
                    if (classChoice == 5)
                    {
                        classChoice = 0;
                    }
                } else if (intelligence < 9 && dexterity < 9 && constitution > 8)    // No Elf, No Halfling
                {
                    classChoice = rnd.Next(1, 8);
                    if (classChoice == 5 || classChoice == 7)
                    {
                        classChoice = 0;
                    }
                } else if (intelligence > 8 && dexterity < 9 && constitution > 8)    // No Halfling
                {
                    classChoice = rnd.Next(1, 7);
                } else if (intelligence > 8 && dexterity > 8 && constitution > 8)    // Allows all classes
                {
                    classChoice = rnd.Next(1, 8);
                }

                switch (classChoice)
                {
                    case 1:
                        result = "Cleric";
                        break;
                    case 2:
                        result = "Fighter";
                        break;
                    case 3:
                        result = "Magic User";
                        break;
                    case 4:
                        result = "Thief";
                        break;
                    case 5:
                        result = "Elf";
                        break;
                    case 6:
                        result = "Dwarf";
                        break;
                    case 7:
                        result = "Halfling";
                        break;
                    default:
                        result = "Fighter";
                        break;
                }

                return result;
            }

            int GetHP(string job, int stat)
            {
                // This function calculates the starting hit points of the character based on
                //  the class' hit die modified by constitution

                int result;

                if (job == "Cleric" || job == "Elf" || job == "Halfling") {
                    result = RollDice(1, 6) + stat;
                } else if(job == "Fighter" || job == "Dwarf") {
                    result = RollDice(1, 8) + stat;
                } else {
                    result = RollDice(1, 4) + stat;
                }

                if (result <= 0) {      // Prevents negative starting HP
                    result = 1;
                }

                return result;
            }
        }

        static int GetModifier(int stat)
        {   
            // This method generates ability scores for each of the 6 stats generated
            //   Values range from +3 to -3 evaluated using if statements and the ability score passed.
            int result;

            if (stat > 17) {
                result = 3;
            } else if (18 > stat && stat > 15) {
                result = 2;
            } else if (16 > stat && stat > 12) {
                result = 1;
            } else if (13 > stat && stat > 8) {
                result = 0;
            } else if (9 > stat && stat > 5) {
                result = -1;
            } else if (6 > stat && stat > 3) {
                result = -2;
            } else { 
                result = -3;
            }
            return result;
        }

        static void ReadCsv(string filePath)
        {
            StreamReader readFile = new StreamReader(filePath);
            string line;
            string[] row;
            readFile.ReadLine();
            while ((line = readFile.ReadLine()) != null)
            {
                row = line.Split(',');
                string Class = row[0];
                int level = Convert.ToInt32(row[1]);
                int death = Convert.ToInt32(row[2]);
                int wands = Convert.ToInt32(row[3]);
                int paralysis = Convert.ToInt32(row[4]);
                int breath = Convert.ToInt32(row[5]);
                int spells = Convert.ToInt32(row[6]);
                int thaco = Convert.ToInt32(row[7]);
                int xp = Convert.ToInt32(row[8]);

                Console.WriteLine(death);
                Console.WriteLine(wands);
                Console.WriteLine(paralysis);
                Console.WriteLine(breath);
                Console.WriteLine(spells);
                Console.WriteLine(thaco);
                Console.WriteLine(xp);
            }

        }
    }

}
