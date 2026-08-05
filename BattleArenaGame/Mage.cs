using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleArenaGame
{
    public class Mage : Character, ISpecialAbility
    {
        private int specialCooldown;
        private int cooldownLength;

        public Mage(string name, bool hardMode)
            : base(name, 90, 28, 3)
        {
            specialCooldown = 0;

            if (hardMode)
            {
                cooldownLength = 2;
            }
            else
            {
                cooldownLength = 1;
            }
        }

        public override int Attack()
        {
            int damage = BattleService.RandomGenerator.Next(
                AttackPower - 3,
                AttackPower + 4);

            Console.WriteLine(Name + " casts a magic attack!");

            if (specialCooldown > 0)
            {
                specialCooldown--;
            }

            return damage;
        }

        public int UseSpecialAbility()
        {
            if (specialCooldown > 0)
            {
                throw new InvalidGameActionException(
                    "Magic Blast is on cooldown for " +
                    specialCooldown + " more turn(s).");
            }

            Console.WriteLine(Name + " used Magic Blast!");

            specialCooldown = cooldownLength;

            return AttackPower + 20;
        }

        public override void DisplayInformation()
        {
            base.DisplayInformation();
            Console.WriteLine("Class: Mage");

            if (specialCooldown > 0)
            {
                Console.WriteLine(
                    "Magic Blast Cooldown: " +
                    specialCooldown + " turn(s)");
            }
            else
            {
                Console.WriteLine("Magic Blast: Ready");
            }
        }
    }
}