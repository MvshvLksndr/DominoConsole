using Domino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    //новый движок
    internal class Engine
    {
        Random random = new Random();
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
        }; // все доминошки 4 по 7

        public Player[] player; //экземпляры игроков

        public List<DiceModel> Bazar; 
        int playersCount; //количество игроков (для этого класса)
        public string gameBoard = string.Empty; //поле игры
        
        public void Init(int PlayerCount)
        {
            if (PlayerCount < 2 || PlayerCount > 4)
            {
                throw new Exception($"Недопустимое количество игроков: {PlayerCount}!");
            }
            playersCount = PlayerCount;
            player= new Player[playersCount];
            for (int i = 0; i < PlayerCount; i++)
            {
                player[i] = new Player();
            }
            
        }

        public void GiveDices()
        {
            for (int p = 0; p < playersCount; p++)
            {
                for (int i = 0; i < 7; i++)
                {
                    DiceModel dice = GetRandomDice();
                    player[p].PlayerDices.Add(dice);
                    Dices.Remove(dice);
                }
            }
            Bazar = Dices;
        }

        public DiceModel GetRandomDice()
        {
            int diceIndex = random.Next(0, Dices.Count);
            DiceModel dice;

            if (Dices.Count == 1)
            {
                dice = Dices[0];
            }
            else
            {
                dice = Dices[random.Next(0, Dices.Count)];
                diceIndex = random.Next(0, Dices.Count);
            }
            return dice;
        }

        public void Flip(int index, int PlayerIndex)
        {
            player[PlayerIndex].PlayerDices[index].Flip();
        }

        public void Take(int PlayerIndex) 
        {
            try
            {
                int rndDice = random.Next(0, Bazar.Count);
                player[PlayerIndex].PlayerDices.Add(Bazar[rndDice]);
                Bazar.RemoveAt(rndDice);
            }
            catch (Exception)
            {
                throw new Exception("Базар пуст");
            }
        }

        public void Pass(int index, int PlayerIndex, bool Left)
        {
            if(Left)
            {
                gameBoard = player[PlayerIndex].PlayerDices[index].DiceStr + gameBoard;
                player[PlayerIndex].PlayerDices.RemoveAt(index);
            }
            else
            {
                gameBoard = gameBoard + player[PlayerIndex].PlayerDices[index].DiceStr;
                player[PlayerIndex].PlayerDices.RemoveAt(index);
            }
           
        }

        public int SummScore(int PlayerIndex)
        {
            int sum = 0;
            for (int i = 0; i < player[PlayerIndex].PlayerDices.Count; i++)
            {
                if (player[PlayerIndex].PlayerDices[i].Num1 + player[PlayerIndex].PlayerDices[i].Num2 == 0) sum += 10;
                sum += player[PlayerIndex].PlayerDices[i].Num1 + player[PlayerIndex].PlayerDices[i].Num2;
            }
            return sum; 
        }

    }
}
