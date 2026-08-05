using System;

namespace BattleArenaGame
{
    public abstract class Character
    {
        public string Name { get; set; }

        public int Health { get; protected set; }

        public int MaximumHealth { get; protected set; }

        public int AttackPower { get; protected set; }

        public int Defense { get; protected set; }


        public bool IsAlive
        {
            get
            {
                return Health > 0;
            }
        }


        protected Character(string name, int health, int attackPower, int defense)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Character name cannot be empty.");
            }

            if (health <= 0 || attackPower <= 0 || defense < 0)
            {
                throw new ArgumentException("Character statistics are not valid.");
            }

            Name = name;
            MaximumHealth = health;
            Health = health;
            AttackPower = attackPower;
            Defense = defense;
        }


        public abstract int Attack();


        public virtual void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentException("Damage cannot be negative.");
            }

            int finalDamage = damage - Defense;

            if (finalDamage < 1)
            {
                finalDamage = 1;
            }

            Health -= finalDamage;

            if (Health < 0)
            {
                Health = 0;
            }

            Console.WriteLine(Name + " received " + finalDamage + " damage.");
        }


        public void RestoreHealth(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Healing amount must be greater than zero.");
            }

            Health += amount;

            if (Health > MaximumHealth)
            {
                Health = MaximumHealth;
            }
        }


        public virtual void DisplayInformation()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Health: " + Health + "/" + MaximumHealth);
            Console.WriteLine("Attack Power: " + AttackPower);
            Console.WriteLine("Defense: " + Defense);
            Console.WriteLine("----------------------------");
        }
    }
}