using System;
using System.Collections.Generic;
using System.Linq;
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
            int[] abilityScore = new int[6];    // Ability scores in index 0 to 5 that correspond to STR, INT, WIS, DEX, CON, CHA respectively
            int[] abilityMod = new int[6];      // Ability score modifiers based on the +3 to -3 scale corresponding to the appropriate stat
            string charClass;                   // Stores a string referring to a character classes and demihumans    

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

            Console.WriteLine("Str: " + abilityScore[0] + " | " + abilityMod[0]);
            Console.WriteLine("Int: " + abilityScore[1] + " | " + abilityMod[1]);
            Console.WriteLine("Wis: " + abilityScore[2] + " | " + abilityMod[2]);
            Console.WriteLine("Dex: " + abilityScore[3] + " | " + abilityMod[3]);
            Console.WriteLine("Con: " + abilityScore[4] + " | " + abilityMod[4]);
            Console.WriteLine("Cha: " + abilityScore[5] + " | " + abilityMod[5]);

            Console.ReadLine();

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

            void GetClass(int[] stat)
            {
                // This method randomly chooses a character class accounting for class restrictions
                //  currently does not optimize class choice

                int classChoice;    // stores the random choice value to select the class based on a switch case

                if (abilityScore[1] > 9 && abilityScore[3] > 9 && abilityScore[4] > 9)
                {
                    classChoice = rnd.Next(1, 8);   // .Next function has upper bound as exclusive

                    
                }
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


    }

}
