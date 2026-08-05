using System;

namespace BattleArenaGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.Start();

            Console.WriteLine();
            Console.WriteLine("Press any key to close the game...");
            Console.ReadKey();
        }
    }
}