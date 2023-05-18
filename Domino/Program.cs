using Domino;
using System.Data;
using System.IO.Pipes;
using System.Security.Cryptography;

internal class Program
{
    public static Random rnd = new Random();
    public static Engine engine;
    public static int PlayersCount;
    public static bool HideBazar = false;

    public static void Main()
    {
        Console.WriteLine("Домино козел. \n Команды:\n  start - запустить игру \n  help - помощь по командам \n hidebazar - скрыть/показать базар");
        while (true)
        {
            string comand = Console.ReadLine();
            switch (comand.ToLower())
            {
                case "start":
                    engine = new Engine();
                    Console.Clear();
                    Console.WriteLine($">{comand}");
                    Init();
                    break;

                case "hidebazar":
                    HideBazar = !HideBazar;
                    Console.WriteLine($"базар спрятан ({HideBazar}) \n(нажмите для продолжения)");
                    Console.ReadLine();
                    Console.Clear();
                    Main();
                    break;

                case "stop":
                    Console.Clear();
                    Console.WriteLine($">{comand}");
                    Console.WriteLine("Игра еще не началась");
                    break;

                case "help":
                    Console.Clear();
                    Console.WriteLine($">{comand}");
                    Console.WriteLine("Когда вы начнете игру, вам будут доступны команды. Сначала пишется команда, затем номер костяшки (если требуется): \n\npass+/+pass - ход, где '+' указывает куда положить костяшку (pass+ - справа или +pass - слева) \nflip - перевернуть костяшку \ntake - взять с базара\nskip - пропустить ход если у вас нет подходящей костяшки\nend - завершить игру и посчитаь очки \nstop - завершить игру.");
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine($">{comand}");
                    Console.WriteLine($"Неизвестная комманда >{comand}<.");
                    Main();
                    break;
            }
        }
    }
    
    public static void Init()
    {
        bool Ready = false;
        int playersCount;

        while (!Ready)
        {
            Console.WriteLine("Введите количество игроков (от 2 до 4)");

            try // ввод количества игроков
            {
                playersCount = Convert.ToInt32(Console.ReadLine());
            }
            catch //ввели не число
            {
                Console.Clear();
                Console.WriteLine("Введите ЧИСЛО игроков");
                Init(); //запускаем заново
                return;
            }
            
            try
            {
                engine.Init(playersCount);
                PlayersCount = playersCount;
                for (int i = 0; i < playersCount; i++)
                {
                    Console.WriteLine($"Введите имя игрока {i+1}");
                    engine.player[i].Name = Console.ReadLine();
                }
                Ready = true;
                Console.WriteLine("Создание игроков...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        engine.GiveDices();
        int turn = 0;
        Game(turn);
    }

    public static void Game(int turn)
    {
        string command;
        while (true)
        {
            if (turn >= PlayersCount) turn = 0;

            DrawUI();
            PlayerTurn(turn);
            command = Console.ReadLine();
            CommandManager(command, turn);
        }
    }

    public static void PlayerTurn(int PlayerIndex)
    {
        Console.WriteLine($"\n\nХОД ИГРОКА {engine.player[PlayerIndex].Name} ({PlayerIndex + 1}). \nДоступные команды: pass+/+pass - ход (справа/слева), flip - перевернуть костяшку, take - взять с базара,end - остановить игру и подсчитать очки, stop - остановить игру\n Доступные костяшки:");
        for (int i = 0; i < engine.player[PlayerIndex].PlayerDices.Count; i++)
        {
            Console.WriteLine($"({i+1}) {engine.player[PlayerIndex].PlayerDices[i].DiceStr}");
        }

    }

    public static void DrawUI()
    {
        Console.Clear();
        Console.WriteLine($"<--------------->[Игровое поле]<-------------->");
        Console.WriteLine($"\n|{engine.gameBoard}|\n");
        Console.WriteLine("<------------------>[Базар]<------------------>\n");
        if(HideBazar = false)
        {
            for (int i = 0; i < engine.Bazar.Count; i++) { Console.Write($" {engine.Bazar[i].DiceStr} "); }   //доминошки в базаре
        }
        else
        {
            for (int i = 0; i < engine.Bazar.Count; i++) { Console.Write($" {engine.Bazar[i].DiceStrHidden} "); }   //доминошки в базаре
        }
        Console.WriteLine("\n<----------------->[игроки]<------------------>\n");

        for (int p = 0; p < PlayersCount; p++)
        {
            Console.WriteLine($"{engine.player[p].Name} (Игрок {p + 1}):");
            for (int i = 0; i < engine.player[p].PlayerDices.Count; i++) //engine.player[p].PlayerDices.Count -- количество доминошек у игрока с индексом [p]
            {
                Console.Write($" {engine.player[p].PlayerDices[i].DiceStr} ");
            }
            Console.Write("\n");
        }
    }

    public static void CommandManager(string cmnd, int PlayerIndex) //PlayerIndex - игрок который ходит в данный момент
    {
        int index; 

        switch (cmnd.ToLower())
        {
           

            case "stop":
                Console.WriteLine("Остановка игры. (enter для продолжения)");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Игра остановлена.");
                Main();
                break;

            case "flip":
                Console.WriteLine("Введите номер костяшки которую хотите перевернуть");
                index = Convert.ToInt32(Console.ReadLine()) - 1;
                engine.Flip(index, PlayerIndex);
                Game(PlayerIndex);
                break;

            case "take":
                Console.WriteLine("Вы берете костяшку из базара. (enter для продолжения)");
                try
                {
                    engine.Take(PlayerIndex);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.ReadLine();
                    Game(PlayerIndex);
                }
                break;

            case "pass+":
                Console.WriteLine("Выберите костяшку которой хотите пойти");
                index = Convert.ToInt32(Console.ReadLine()) - 1;
                engine.Pass(index, PlayerIndex, false);
                Game(PlayerIndex + 1);
                break;

            case "pass":
                Console.WriteLine("Выберите костяшку которой хотите пойти");
                index = Convert.ToInt32(Console.ReadLine()) - 1;
                engine.Pass(index, PlayerIndex, false);
                Game(PlayerIndex + 1);
                break;

            case "+pass":
                Console.WriteLine("Выберите костяшку которой хотите пойти");
                index = Convert.ToInt32(Console.ReadLine()) - 1;
                engine.Pass(index, PlayerIndex, true);
                Game(PlayerIndex + 1);
                break;

            case "skip":
                Console.WriteLine("Вы пропускаете ход. (enter для продолжения)");
                Console.ReadLine();
                Game(PlayerIndex + 1);
                break;

            case "end":
                Console.WriteLine("Завершение игры и подсчет очков. (enter для продолжения)");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Игра завершена");
                for (int i = 0; i < PlayersCount; i++)
                {
                    Console.WriteLine($"\nИгрок {engine.player[i].Name} ({i+1}). Сумма очков: {engine.SummScore(i)}");
                }
                Console.WriteLine("\n(enter для продолжения)");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Игра завершена");
                Main();
                break;

            default:
                Console.WriteLine($"Неизвестная комманда >{cmnd}<. (enter для продолжения)");
                Console.ReadLine();
                Game(PlayerIndex);
                break;
        }
    }
}