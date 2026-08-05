using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace BattleArenaGame
{
    public class Game
    {
        private const string HealthPotionKey = "Health Potion";

        private readonly BattleService battleService = new BattleService();
        private readonly Dictionary<string, int> inventory = new Dictionary<string, int>();
        private readonly List<Enemy> defeatedEnemies = new List<Enemy>();

        public Game(){}

        public void Start()
        {
            bool hardMode = false;
            bool playAgain = true;

            while (playAgain)
            {
                defeatedEnemies.Clear();
                inventory.Clear();

                if (hardMode)
                {
                    inventory.Add(HealthPotionKey, 4);
                }
                else
                {
                    inventory.Add(HealthPotionKey, 3);
                }

                DisplayTitle();

                if (hardMode)
                {
                    Console.WriteLine("HARD MODE");
                    Console.WriteLine("Enemies are stronger.");
                    Console.WriteLine();
                }

                string playerName = ReadNonEmptyText("Enter your name: ");

                Character player = CreatePlayer(playerName, hardMode);
                List<Enemy> enemies = CreateEnemies(hardMode);

                Console.WriteLine();
                Console.WriteLine("Your selected character:");
                player.DisplayInformation();

                DisplayEnemyPreview(enemies);

                foreach (Enemy enemy in enemies)
                {
                    if (!player.IsAlive)
                    {
                        break;
                    }

                    bool enemyDefeated =
                        battleService.RunBattle(player, enemy, inventory);

                    if (enemyDefeated)
                    {
                        defeatedEnemies.Add(enemy);
                    }
                }

                bool playerWon =
                    player.IsAlive &&
                    !enemies.Any(enemy => enemy.IsAlive);

                DisplayGameSummary(player, enemies, playerWon);
                battleService.DisplayRecentBattleActions(5);

                if (playerWon && !hardMode)
                {
                    Console.WriteLine();
                    Console.WriteLine("You unlocked Hard Mode!");
                    Console.WriteLine("Would you like to play Hard Mode?");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");

                    int replayChoice = BattleService.ReadMenuChoice(1, 2);

                    if (replayChoice == 1)
                    {
                        hardMode = true;
                        Console.Clear();
                    }
                    else
                    {
                        playAgain = false;
                    }
                }
                else
                {
                    playAgain = false;
                }
            }
        }

        private static void DisplayTitle()
        {
            Console.WriteLine("////////////////////////////////");
            Console.WriteLine("       BATTLE ARENA GAME");
            Console.WriteLine("////////////////////////////////");
        }

        private static Character CreatePlayer(string playerName, bool hardMode)
        {
            while (true)
            {
                Console.WriteLine("Choose your character:");
                Console.WriteLine("1. Warrior");
                Console.WriteLine("2. Mage");

                int choice = BattleService.ReadMenuChoice(1, 2);

                switch (choice)
                {
                    case 1:
                        return new Warrior(playerName, hardMode);

                    case 2:
                        return new Mage(playerName, hardMode);

                    default:
                        Console.WriteLine("Invalid character selection.");
                        break;
                }
            }
        }
        private static List<Enemy> CreateEnemies(bool hardMode)
        {
            List<Enemy> enemies = new List<Enemy>();

            if (hardMode)
            {
                enemies.Add(new Enemy("Goblin", 44, 12, 2, 75));
                enemies.Add(new Enemy("Skeleton", 55, 14, 3, 100));
                enemies.Add(new Enemy("Bandit", 70, 17, 4, 140));
                enemies.Add(new Enemy("Orc", 90, 21, 6, 200));
                enemies.Add(new Enemy("Dragon", 115, 26, 8, 350));
            }
            else
            {
                enemies.Add(new Enemy("Goblin", 40, 11, 2, 50));
                enemies.Add(new Enemy("Skeleton", 50, 13, 3, 75));
                enemies.Add(new Enemy("Bandit", 65, 16, 4, 100));
                enemies.Add(new Enemy("Orc", 80, 19, 5, 150));
                enemies.Add(new Enemy("Dragon", 110, 23, 7, 250));
            }

            return enemies;
        }

        private static void DisplayEnemyPreview(List<Enemy> enemies)
        {
            Console.WriteLine("Enemies in the arena (lowest to highest reward):");

            IEnumerable<Enemy> orderedEnemies = enemies.OrderBy(enemy => enemy.RewardScore);

            foreach (Enemy enemy in orderedEnemies)
            {
                Console.WriteLine(
                    "- " + enemy.Name + ": " + enemy.MaximumHealth + " HP, " +
                    enemy.AttackPower + " ATK, " + enemy.RewardScore + " points");
            }
        }

        private void DisplayGameSummary(Character player, List<Enemy> allEnemies, bool playerWon)
        {
            List<Enemy> confirmedDefeatedEnemies = defeatedEnemies
                .Where(enemy => !enemy.IsAlive)
                .ToList();

            int defeatedCount = confirmedDefeatedEnemies.Count();
            int totalScore = confirmedDefeatedEnemies.Sum(enemy => enemy.RewardScore);

            Enemy strongestDefeatedEnemy = confirmedDefeatedEnemies
                .OrderByDescending(enemy => enemy.AttackPower)
                .FirstOrDefault();

            bool enemiesRemain = allEnemies.Any(enemy => enemy.IsAlive);
            int remainingPotions = inventory.ContainsKey(HealthPotionKey)
                ? inventory[HealthPotionKey]
                : 0;

            Console.WriteLine();
            Console.WriteLine("////////////////////////////////");
            Console.WriteLine("          GAME SUMMARY");
            Console.WriteLine("////////////////////////////////");
            Console.WriteLine("Player: " + player.Name);
            Console.WriteLine("Character: " + player.GetType().Name);
            Console.WriteLine("Result: " + (playerWon ? "Victory" : "Defeat"));
            Console.WriteLine("Final Health: " + player.Health + "/" + player.MaximumHealth);
            Console.WriteLine("Enemies Defeated: " + defeatedCount);
            Console.WriteLine("Defeated Enemy Names: " + FormatEnemyNames(confirmedDefeatedEnemies));
            Console.WriteLine("Remaining Potions: " + remainingPotions);
            Console.WriteLine("Total Score: " + totalScore);
            Console.WriteLine(
                "Strongest Defeated Enemy: " +
                (strongestDefeatedEnemy == null ? "None" : strongestDefeatedEnemy.Name));
            Console.WriteLine("Enemies Still Alive: " + (enemiesRemain ? "Yes" : "No"));
        }

        private static string FormatEnemyNames(List<Enemy> defeatedEnemyList)
        {
            if (defeatedEnemyList.Count == 0)
            {

                return "None";
            }

            return string.Join(", ", defeatedEnemyList.Select(enemy => enemy.Name).ToArray());
        }

        private static string ReadNonEmptyText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }

                Console.WriteLine("The value cannot be empty. Please try again.");
            }
        }
    }
}
