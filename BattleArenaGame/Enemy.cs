using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleArenaGame
{
    public class Enemy : Character
    {
        public int RewardScore { get; private set; }

        public Enemy(string name, int health, int attackPower, int defense, int rewardScore)
            : base(name, health, attackPower, defense)
        {
            RewardScore = rewardScore;
        }

        public override int Attack()
        {
            Random random = BattleService.RandomGenerator;

            int damage = random.Next(AttackPower - 3, AttackPower + 4);

            Console.WriteLine(Name + " attacks!");

            return damage;
        }

        public override void DisplayInformation()
        {
            base.DisplayInformation();
            Console.WriteLine("Reward Score: " + RewardScore);
        }
    }
}