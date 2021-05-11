using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drofsnar
{
    public class Drofsnar
    {
        
        public string[] Seed()
        {
            // Grabbing my txt information. Since this needs to change depending on location I left this at the top
            string text = File.ReadAllText(@"C:\ElevenFiftyProjects\86SD\Drofsnar\GameSequence.txt");
            return text.Split(',');
        }

        // Setting up my enemy field. I created a Dictionary of Key type 'string' that will be seen in the .txt file and holds a value of Enemy class. This lets me pull up a full enemy type with the simplified string. I hardcoded this dictionary since it won't be changing run to run. Enemies are the same every time.
        private readonly Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>
        {
            // Key = "Bird", Value is my Enemy class type with the properties matching Bird
            {"Bird",new Enemy("a Bird",10) },
            {"CrestedIbis",new Enemy("a Crested Ibis",100) },
            {"GreatKiskudee",new Enemy("a Great Kiskudee",300) },
            {"RedCrossbill",new Enemy("a Red Crossbill",500) },
            {"Red-neckedPhalarope",new Enemy("a Red-necked Phalarope",700) },
            {"EveningGrosbeak",new Enemy("an Evening Grosbeak",1000) },
            {"GreaterPrairieChicken",new Enemy("a Greater Prairie Chicken",2000) },
            {"IcelandGull",new Enemy("an Iceland Gull",3000) },
            {"Orange-belliedParrot",new Enemy("an Orange-bellied Parrot",5000) },
            {"InvincibleBirdHunter", new Enemy("an Invincible Bird Hunter", 0) },
            {"VulnerableBirdHunter", new Enemy("a Vulnerable Bird Hunter", 200) }
            // Side note, I could've made a ternairy to check if the first letter is a vowel and make a/an changes for me but just affixing it myself to the name seemed easiest since those properties are only used in the console writelines. Just as a quick and easy work around I did.
        };

        public void Run()
        {
            // Get my text sequnce
            var sequence = Seed();

            // Set up my default values
            int score = 5000;
            int lives = 3;

            // Setting up my logic variables
            // this is so I can keep track of how many vulnerable hunters I hit in a row to calculate my multiplier
            int consecutiveBirdHunters = 0;
            // This is so I can keep track of how many lives I had so it can set new thresholds for getting a one up
            int livesGained = 1;
            // Instantiating a string variable for use later
            string whatHappened;

            foreach (string encounter in sequence)
            {
                // Resetting my what happened variable just in case
                whatHappened = "new event";
                Console.ForegroundColor = ConsoleColor.White;

                // Finding my vulnerable strings
                if (encounter == "VulnerableBirdHunter")
                {
                    // Adding 200 times by my score multiplier which uses the consecutiveBirdHunters variable to keep track. 2^0 is 1, so that covers my 200 on the first encounter. Then 2^2 is 4, 4x200 is 800. So I get my triple kill multiplier.
                    score += 200 * (int)(Math.Pow(2, consecutiveBirdHunters));
                    // Adding one to our count just in case it happens again
                    consecutiveBirdHunters++;
                    // Loading my string I'll mess with later
                    whatHappened = "You killed a Bird Hunter!";
                    // Setting it to green since it's a good event
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                // Finding my Invincible strings
                else if (encounter == "InvincibleBirdHunter")
                {
                    // Subtracting a life
                    lives--;
                    // Loading my string for later
                    whatHappened = $"Attacked by a Bird Hunter! You lost a life. {lives} lives remaing.";
                    // Resetting my consecutive count, since I hit this obviously I didn't encounter another vulnerable bird hunter
                    consecutiveBirdHunters = 0;
                    // Making the text red
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                // My else that will trigger for anything other than Bird Hunters
                else
                {
                    // Using my dictionary to match the current string with a Key string, then pulling up that score property for that class. If the current string is "CrestedIbis" it will match to the key of the same string that was setup up above. Which allows us access to the value of Enemy that has the Name of "Crested Ibis" and a score of 100.
                    // Using that functionality the string matching is automatic and allows us access to the properties we need. Adding the score proprety value of the matching property to score 
                    score += _enemies[encounter].Score;
                    // Loading up our Console write line
                    whatHappened = $"Oh!! It's {_enemies[encounter].Name} +{_enemies[encounter].Score}";
                }
                // Since I want an out put after each encounter regardless, I loaded the whatHappened string and print after all my logic related to the encounter itself has ran. If I wanted to I could use this setup to do more advanced print lines
                Console.WriteLine(whatHappened);

                // Checking if they earned a new life after each encounter. It's a 10k threshold for each life and keeping track of lives given lets me set the threshold at the next 10k. (3 lives gained, times 10k, means 30k until a new life.)
                if(score >= 10000 * livesGained)
                {
                    //Increase the life counter
                    ++lives;
                    // Increment our livesGained score multiplier
                    ++livesGained;
                    // Adding a writeline here since this is a special event that doesn't happen everytime
                    Console.WriteLine($"{score}pts! You gained a life");
                }

                // Checking if after each encounter if the player died
                if (lives == 0)
                {
                    // Writing out this special event. Since this can only happen when running into an InvincibleBirdHunter, which sets our text to red within this run of the loop, this should be red.
                    Console.WriteLine("You died");
                    // Using a break in this foreach loop ends the loops right here and now
                    break;
                }
            }
            // If either the foreach loops ends naturally or early from death, we want to present a game over and show the score
            Console.WriteLine("Game over\n" +
                $"Final score: {score}\n" +
                $"Lives: {lives}");
            Console.ReadKey();
        }

        // Setting up an enemy class to hold some simple values I'll use in the dictionary above
        public class Enemy
        {
            // Setting up the constructor I want to use to easily create Enemy class types, which I can use in line with my Dictionary entry creation
            public Enemy(string name, int score)
            {
                Name = name;
                Score = score;
            }
            public string Name { get; set; }
            public int Score { get; set; }
        }
    }
}
