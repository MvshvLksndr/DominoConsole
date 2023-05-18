using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    internal class Player
    {
        public List<DiceModel> PlayerDices { get; set; }
        public string Name { get; set; }
        public Player() 
        { 
            PlayerDices = new List<DiceModel>();
        }
    }
}
