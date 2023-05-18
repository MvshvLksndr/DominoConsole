using Domino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Domino
{
    //новый движок
    
    internal class Engine
    {
        public bool firstTime = true;


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

        public int leftEnd;
        public int rightEnd;
        public void Init(int PlayerCount)
        {
            if (PlayerCount < 2 || PlayerCount > 4) throw new Exception($"Недопустимое количество игроков: {PlayerCount}!");
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
            

            if (firstTime)
            {
                if (Left)
                {
                    gameBoard = player[PlayerIndex].PlayerDices[index].DiceStr + gameBoard;
                    leftEnd = player[PlayerIndex].PlayerDices[index].Num1;
                    rightEnd = player[PlayerIndex].PlayerDices[index].Num2;
                    player[PlayerIndex].PlayerDices.RemoveAt(index);
                }
                else
                {
                    gameBoard = gameBoard + player[PlayerIndex].PlayerDices[index].DiceStr;
                    leftEnd = player[PlayerIndex].PlayerDices[index].Num1;
                    rightEnd = player[PlayerIndex].PlayerDices[index].Num2;
                    player[PlayerIndex].PlayerDices.RemoveAt(index);
                }
                firstTime = false;
                return;
            } //первый ход в игре

            if (!CheckMove(Left, player[PlayerIndex].PlayerDices[index])) throw new Exception("Вы не можете так сходить!");


            if(Left)
            {
                
                gameBoard = player[PlayerIndex].PlayerDices[index].DiceStr + gameBoard;
                leftEnd = player[PlayerIndex].PlayerDices[index].Num1;
                player[PlayerIndex].PlayerDices.RemoveAt(index);
            }
            else
            {
                gameBoard = gameBoard + player[PlayerIndex].PlayerDices[index].DiceStr;
                rightEnd = player[PlayerIndex].PlayerDices[index].Num2;
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

        /// <summary>
        /// Проверяет рыбу. Вроде работает
        /// </summary>
        /// <returns>true если рыба, false если нет</returns>
        public bool CheckFish()
        {
            if (firstTime) return false;
            bool NoDiceOnPlayers = false;
            bool NoDiceAtBazar = false;
            foreach (Player player in player)
            {
                //оно сравнивает все числа со всех костяшек ИГРОКОВ с правым и левым концом поля и возвращает NoDiceOnPlayers = true; если нет совпадений (совсем)
                if (player.PlayerDices.Any(x => x.Num1 == rightEnd || x.Num1 == leftEnd) || player.PlayerDices.Any(x => x.Num2 == rightEnd || x.Num2 == leftEnd)) 
                {
                    NoDiceOnPlayers = false;
                }
                else
                {
                    NoDiceOnPlayers = true;
                }
            }
            Console.WriteLine($"NoDiceOnPlayers {NoDiceOnPlayers}");

            //оно сравнивает все числа со всех костяшек В БАЗАРЕ с правым и левым концом поля и возвращает NoDiceAtBazar = true; если нет совпадений (совсем)
            foreach (DiceModel bazarDice in Bazar)
            {
                if ((bazarDice.Num1 == leftEnd || bazarDice.Num2 == leftEnd) || (bazarDice.Num1 == rightEnd || bazarDice.Num2 == rightEnd))
                {
                    NoDiceAtBazar = false;
                }
                else
                {
                    NoDiceAtBazar = true;
                }
            }
            Console.WriteLine($"NoDiceAtBazar {NoDiceAtBazar}");

            //если в обоих случаях нет совпадений - рыба
            if(NoDiceAtBazar && NoDiceOnPlayers) 
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        /// <summary>
        /// проверка хода.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="dice"></param>
        /// <returns>false если вы не можете так сходить, true если можете</returns>
        public bool CheckMove(bool left, DiceModel dice)
        {
            if (left)
            {
                Console.WriteLine($"{dice.Num2},{leftEnd}.");
                Console.ReadLine();
                if (dice.Num2 != leftEnd) return false;
            }
            else
            {
                Console.WriteLine($"{dice.Num1},{rightEnd}.");
                Console.ReadLine();
                if (dice.Num1 != rightEnd) return false;
            }
            return true;
        }
        /// <summary>
        /// возвращает игрока если нашло победителя
        /// </summary>
        /// <returns>Player</returns>
        public Player ChеckWinner()
        {
            foreach (Player winner in player)
            {
                if (winner.PlayerDices.Count == 0) return winner;
            }
            return null;
        }
    }
}
