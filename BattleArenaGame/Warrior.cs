using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleArenaGame
{
    public class Warrior : Character, ISpecialAbility
    {
        private int specialUses;
        private int specialDamage;

        public int SpecialUsesLeft
        {
            get { return specialUses; }
        }

        public Warrior(string name, bool hardMode)
            : base(name, 120, hardMode ? 22 : 20, 8)
        {
            if (hardMode)
            {
                specialUses = 2;
                specialDamage = 25;
            }
            else
            {
                specialUses = 3;
                specialDamage = 15;
            }
        }

        public override int Attack()
        {
            int damage = BattleService.RandomGenerator.Next(
                AttackPower - 3,
                AttackPower + 4);

            Console.WriteLine(Name + " attacks with a sword!");

            return damage;
        }

        public int UseSpecialAbility()
        {
            if (specialUses <= 0)
            {
                throw new InvalidGameActionException(
                    "You have no Powerful Strikes left.");
            }

            specialUses--;

            Console.WriteLine(Name + " used Powerful Strike!");
            Console.WriteLine("Powerful Strikes left: " + specialUses);

            return AttackPower + specialDamage;
        }

        public override void DisplayInformation()
        {
            base.DisplayInformation();
            Console.WriteLine("Class: Warrior");
            Console.WriteLine("Powerful Strikes Left: " + specialUses);
        }
    }
}