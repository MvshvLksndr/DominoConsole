using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domino;

namespace Domino
{
    //больше не работает !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!! !!!
    internal class OLDEngine
    {
        Random rnd = new Random();

        List<DiceModel> Dices = new List<DiceModel> 
        { 
            new DiceModel(0, 0),
            new DiceModel(0, 1),
            new DiceModel(0, 2),
            new DiceModel(0, 3),
            new DiceModel(0, 4),
            new DiceModel(0, 5),
            new DiceModel(0, 6),
            new DiceModel(1, 1),
            new DiceModel(1, 2),
            new DiceModel(1, 3),
            new DiceModel(1, 4),
            new DiceModel(1, 5),
            new DiceModel(1, 6),
            new DiceModel(2, 2),
            new DiceModel(2, 3),
            new DiceModel(2, 4),
            new DiceModel(2, 5),
            new DiceModel(2, 6),
            new DiceModel(3, 3),
            new DiceModel(3, 4),
            new DiceModel(3, 5),
            new DiceModel(3, 6),
            new DiceModel(4, 4),
            new DiceModel(4, 5),
            new DiceModel(4, 6),
            new DiceModel(5, 5),
            new DiceModel(5, 6),
            new DiceModel(6, 6),
        }; //их (4 по 7)

        public List<DiceModel> Bazar;
        public List<DiceModel>[] PlayerDices;

        #region old
        //string[] PlayerName;

        //public void Register(int Players)
        //{
        //    if (Players == 1)
        //    {
        //        PlayerName = new string[Players + 1];
        //        Console.WriteLine($"Введите имя игрока 1");
        //        PlayerName[0] = Console.ReadLine();
        //        PlayerName[1] = "Бот";
        //        Console.WriteLine("Против вас играет бот \n");
        //        GiveDices(Players);
        //    }
        //    else
        //    {
        //        PlayerName = new string[Players];
        //        for (int i = 0; i < Players; i++)
        //        {
        //            Console.WriteLine($"Введите имя игрока {i + 1}");
        //            PlayerName[i] = Console.ReadLine();
        //        }
        //        for (int i = 0; i < Players; i++)
        //        {
        //            Console.WriteLine(PlayerName[i]);
        //        }
        //        GiveDices(Players);
        //    }
        //}
        #endregion

        public void GiveDices(int Players)
        {
            PlayerDices = new List<DiceModel>[Players];
            int DiceIndex = rnd.Next(0, Dices.Count);

            for (int playersCount = 0; playersCount < Players; playersCount++)
            {
                PlayerDices[playersCount] = new List<DiceModel>();

                for (int dices = 0; dices < 7; dices++)
                {
                    if(Dices.Count == 1)
                    {
                        PlayerDices[playersCount].Add(Dices[0]);
                        Dices =  new List<DiceModel>();
                        //Console.WriteLine($"Игрок {playersCount + 1} получает |{Dices[0].Num1}|{Dices[0].Num2}| с индексом {0}. Осталось 0");
                    }
                    else
                    {
                        PlayerDices[playersCount].Add(Dices[DiceIndex]);
                        Dices.RemoveAt(DiceIndex);
                        DiceIndex = rnd.Next(0, Dices.Count);
                        //Console.WriteLine($"Игрок {playersCount + 1} получает |{Dices[DiceIndex].Num1}|{Dices[DiceIndex].Num2}| с индексом {DiceIndex}. Осталось {Dices.Count}");
                    }
                }
            }
            Bazar = Dices;
        }  
    }
}
