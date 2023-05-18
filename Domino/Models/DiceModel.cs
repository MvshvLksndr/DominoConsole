using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    internal class DiceModel
    {
        public int Num1 { get; set; }
        public int Num2 { get; set;}
        public string DiceStr { get; set; }
        public string DiceStrHidden { get; set; }
        public DiceModel(int num1, int num2) 
        {
            Num1= num1;
            Num2= num2;
            DiceStr = $"[{num1}|{num2}]";
            DiceStrHidden = "[*|*]";
        }
        public void Flip()
        {
            int buffer = Num1;
            Num1 = Num2;
            Num2 = buffer;
            DiceStr = $"[{Num1}|{Num2}]";
           
        }
    }
}
