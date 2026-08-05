using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleArenaGame
{
    public class InvalidGameActionException : Exception
    {
        public InvalidGameActionException(string message) : base(message)
        {

        }
    }
}
