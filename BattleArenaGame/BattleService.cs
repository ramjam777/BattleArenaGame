using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleArenaGame
{
    public class BattleService
    {
        public static Random RandomGenerator = new Random();

        private Stack<string> battleActions = new Stack<string>();

        public bool RunBattle(Character player, Enemy enemy, Dictionary<string, int> inventory)
        {
            Console.WriteLine();
            Console.WriteLine("A wild " + enemy.Name + " appeared!");

            while (player.IsAlive && enemy.IsAlive)
            {
                Console.WriteLine();
                DisplayBattleStatus(player, enemy);

                Console.WriteLine("1. Normal Attack");
                Console.WriteLine("2. Use Special Ability");
                Console.WriteLine("3. Use Health Potion");
                Console.WriteLine("4. View Player Information");
                Console.WriteLine("5. View Enemy Information");

                int choice = ReadMenuChoice(1, 5);

                bool playerUsedTurn = false;

                try
                {
                    switch (choice)
                    {
                        case 1:
                            enemy.TakeDamage(player.Attack());
                            battleActions.Push(player.Name + " attacked " + enemy.Name);
                            playerUsedTurn = true;
                            break;

                        case 2:
                            UseSpecialAttack(player, enemy);
                            playerUsedTurn = true;
                            if (player is Warrior warrior)
                            {
                                Console.WriteLine("Powerful Strikes Left: " + warrior.SpecialUsesLeft);
                            }
                            break;

                        case 3:
                            UseHealthPotion(player, inventory);
                            playerUsedTurn = true;
                            break;

                        case 4:
                            player.DisplayInformation();
                            break;

                        case 5:
                            enemy.DisplayInformation();
                            break;
                    }
                }
                catch (InvalidGameActionException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine();
                }

                if (enemy.IsAlive && playerUsedTurn)
                {
                    player.TakeDamage(enemy.Attack());
                }
            }

            if (player.IsAlive)
            {
                Console.WriteLine(enemy.Name + " was defeated!");
                return true;
            }

            Console.WriteLine("You were defeated.");
            return false;
        }

        private void UseSpecialAttack(Character player, Enemy enemy)
        {
            if (player is ISpecialAbility special)
            {
                int damage = special.UseSpecialAbility();
                enemy.TakeDamage(damage);

                battleActions.Push(player.Name + " used a special ability.");
            }
        }

        public void UseHealthPotion(Character player, Dictionary<string, int> inventory)
        {
            if (!inventory.ContainsKey("Health Potion") || inventory["Health Potion"] <= 0)
            {
                throw new InvalidGameActionException("No health potions left.");
            }

            if (player.Health == player.MaximumHealth)
            {
                throw new InvalidGameActionException("Your health is already full.");
            }

            int healthBefore = player.Health;

            player.RestoreHealth(45);

            int restoredHealth = player.Health - healthBefore;

            inventory["Health Potion"]--;

            Console.WriteLine("You restored " + restoredHealth + " health.");
            Console.WriteLine("Potions left: " + inventory["Health Potion"]);
        }

        public void DisplayBattleStatus(Character player, Enemy enemy)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine(player.Name + ": " + player.Health + "/" + player.MaximumHealth);
            Console.WriteLine(enemy.Name + ": " + enemy.Health + "/" + enemy.MaximumHealth);
            Console.WriteLine("----------------------");
        }

        public static int ReadMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.Write("Choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice >= min && choice <= max)
                    {
                        return choice;
                    }
                }

                Console.WriteLine("Invalid choice. Try again.");
            }
        }

        public void DisplayRecentBattleActions(int amount)
        {
            Console.WriteLine();
            Console.WriteLine("Recent Battle Actions:");

            foreach (string action in battleActions)
            {
                Console.WriteLine("- " + action);

                amount--;

                if (amount == 0)
                {
                    break;
                }
            }
        }
    }
}