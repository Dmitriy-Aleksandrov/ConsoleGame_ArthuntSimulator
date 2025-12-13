using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CNSigra_arthunt
{
    class ArtInventory
    {
        public int Id { get; set; }
        public int Rarity { get; set; }
    }
    class ArtSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Moder { get; set; }
    }
    class Program
    {
        static List<ArtInventory> inventory;
        static List<ArtSet> Gartlist;
        
        static Random rnd = new Random();
        static int charge = 150, scanIndex = 700, scan = 0, score = 0; //si=1000
        static bool buryat = false, elbrus = false;

        static string[] SRSartstorage = new string[] { "0 Вехотка 1,18","1 Осколок 1,93","2 Стальной_ежик 1,77","3 Кисель 1.2",
            "4 Призма 2.23","5 Флегма 1.14","6 Репях 1.45","7 Трещотка 1.51","8 Комета 1.54","9 Кислотный_кристалл 1.25",
            "10 Призрачный_кристалл 1.25","11 Красный_кристалл 1.25","12 Пиявка 1.47","13 Ветка_калины 1.89","14 Цибуля 1.36",
            "15 Браслет 1.87","16 Ягодка 1.11" };

        /*
         
         
         */
        
        static void FillArtSet()
        {
            try
            {                
                Gartlist = new List<ArtSet>();
                //string filePath = "C:/Users/user/source/repos/CNSigra_arthunt/istochnik.txt";
                //string[] lines = File.ReadAllLines(filePath);
                foreach (string line in SRSartstorage)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(' ');
                    int id = int.Parse(parts[0]);
                    string name = parts[1].Replace('_', ' ');
                    double coefficient = double.Parse(parts[2].Replace('.', ','));

                    var newart = new ArtSet
                    {
                        Id = id,
                        Name = name,
                        Moder = coefficient
                    };
                    Gartlist.Add(newart);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message.ToString());
                
            }
        }

        static void GoArthant()
        {
            try
            {
                int detectorChoise, locationChoise;
                inventory = new List<ArtInventory>();

                do{
                    Console.WriteLine("Какой детектор выбрать?\n 1. Глазками, \n 2. Бурят, \n 3. Эльбрус, \n 4. Идём обратно.");
                    int.TryParse(Console.ReadLine(), out detectorChoise);
                    if(detectorChoise == 2 && buryat == false)
                    {
                        Console.Clear();
                        Console.WriteLine("У вас нет Бурята. Купите его в магазине.");
                        detectorChoise = 0;
                    }
                    else if(detectorChoise == 3 && elbrus == false)
                    {
                        Console.Clear();
                        Console.WriteLine("У вас нет Эльбруса. Купите его в магазине.");
                        locationChoise = 0;
                    }
                } while (detectorChoise < 1 || detectorChoise > 4);
                
                if(detectorChoise == 4)
                {
                    return;
                }

                Console.Clear();

                do{
                    Console.WriteLine("Какую локацию выбрать? \n 1. Кордон \n 2. Армейские склады \n 3. Затон");
                    int.TryParse(Console.ReadLine(), out locationChoise);
                } while (locationChoise < 1 || locationChoise > 3);
                Console.Clear();
                if (detectorChoise < 4)
                {
                    Console.WriteLine("Провести скан? \n 1. Да \n 2. Нет, вынос.");
                    int goChoise = 0;

                    do
                    {
                        int.TryParse(Console.ReadLine(), out goChoise);
                        Console.Clear();

                        int pkChanse = 0;
                        if (goChoise >= 2)
                        {
                            break;
                        }

                        switch (locationChoise)
                        {
                            case 1:
                                pkChanse = rnd.Next(1000);
                                break;
                            case 2:
                                pkChanse = rnd.Next(700);
                                scanIndex = 850; //850
                                break;
                            case 3:
                                pkChanse = rnd.Next(300);
                                scanIndex = 700; //700
                                break;
                        }
                        switch (detectorChoise)
                        {
                            case 1:
                                scan = rnd.Next(scanIndex + 300);
                                break;
                            case 2:
                                scan = rnd.Next(scanIndex);
                                charge -= 10;
                                break;
                            case 3:
                                scan = rnd.Next(scanIndex - 300);
                                charge -= 5;
                                break;

                        }
                        
                        if (pkChanse == 152)
                        {
                            Console.WriteLine("На вас напали ПКшеры, вы всё потеряли.");
                            inventory.Clear();
                            break;
                        }

                        if (charge < 0)
                        {
                            Console.WriteLine("Детектор разряжен, идём на вынос.");
                            break;
                        }
                        _ = ArthantAnimation();

                    } while (goChoise != 2);
                    
                }
            }
            catch
            {

            }
            finally
            {
                Vynos();
                _ = VynosAnimation();
            }
        }
        static void Arthant(int scan)
        {
            Console.Clear();
            int found = 0;
            ArtSet lootedart;
            if (scan > 0 && scan < 45)
            {
                found = 1;
            }
            else if (scan > 45 && scan < 70)
            {
                found = 2;
            }
            else if (scan > 70 && scan < 85)
            {
                found = 3;
            }
            else if (scan > 85 && scan < 95)
            {
                found = 4;
            }
            else if (scan > 95 && scan < 99)
            {
                found = 5;
            }
            else if (scan > 99 && scan < 100)
            {
                found = 6;
            }
            else
            {
                Console.WriteLine("Ненаход. Провести ещё скан? \n 1. Да \n 2. Нет, вынос \n");
                found = 0;
            }
            if (found != 0)
            {
                lootedart = Gartlist[rnd.Next(Gartlist.Count())];
                Console.Write("Вы залутали ");
                switch (found)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case 5:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 6:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                }
                Console.WriteLine(lootedart.Name + " ");
                ArtInventory newArt = new ArtInventory
                {
                    Rarity = found,
                    Id = lootedart.Id
                };
                inventory.Add(newArt);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Провести ещё скан? \n 1. Да \n 2. Нет, вынос.");
            }
        }

        private static async Task ArthantAnimation()
        {
            int timer = 20;
            
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("  %%  \n    %%  ");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
                Console.WriteLine("\n    %%\n  %%  ");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
                Console.WriteLine("\n%%    \n  %%  ");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
                Console.WriteLine("  %%  \n%%    \n");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
            }
            Arthant(scan);

        }
        private static async Task VynosAnimation()
        {
            int timer = 20;

            Console.Clear();
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Идём на вынос.");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
                Console.WriteLine("Идём на вынос..");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
                Console.WriteLine("Идём на вынос...");
                await Task.Delay(TimeSpan.FromMilliseconds(timer));
                Console.Clear();
            }
        }

        static void Vynos()
        {
            int vynossum = 0;
            int[] priseset = new int[inventory.Count];
            Console.Write("Конец.\n Вынос: ");
            if (inventory.Count == 0)
            {
                Console.WriteLine("Ненаход. \n Смерть в нищите.");
            }
            else
            {
                for (int i = 0; i < inventory.Count(); i++)
                {
                    switch (inventory[i].Rarity)
                    {
                        case 1:
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 2:
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            }
                        case 3:
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            }
                        case 4:
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            }
                        case 5:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            }
                        case 6:
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            }
                    }
                    ArtSet lootedart = Gartlist[inventory[i].Id];
                    Console.Write(lootedart.Name);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (i == inventory.Count - 1)
                    {
                        Console.Write(".\n");
                    }
                    else
                    {
                        Console.Write(", ");
                    }
                }
                for(int i = 0; i < inventory.Count; i++)
                {
                    int artprise = 0;
                    ArtInventory selectedart = inventory[i];
                    ArtSet concArt = Gartlist[selectedart.Id];
                    artprise = Convert.ToInt32(Math.Pow((selectedart.Rarity * 0.35) * concArt.Moder + 5, concArt.Moder) * 298);
                    Console.WriteLine(artprise); //debug
                    vynossum += artprise;

                }

                score += vynossum;

                Console.Write($"Общая сумма выноса: {vynossum}");
                
                var wait = Console.ReadLine();

            }
        }

        static void GoShop()
        {
            bool leaveshop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в МАГАЗИН! \nАссортимент:");
                Console.Write("1. Бурят;" ,-8,"75000 руб"); if (buryat == false) { Console.WriteLine("-- ПРОДАНО!!!"); }
                Console.WriteLine("2. Эльбрус.;", -8,"250000 руб"); if (elbrus == false) { Console.WriteLine("-- ПРОДАНО!!!"); }
                Console.WriteLine(" 3. Выход. \nЧто брать?");
                var buychoise = Console.ReadKey();
                switch (buychoise.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        if (buryat == true && score >= 75000) { buryat = true; Console.WriteLine("Вы уже купили это."); }
                        else { Console.WriteLine("Поздравляем с приобретением!"); }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        if (elbrus == true && score >= 250000) { elbrus = true; }
                        else { Console.WriteLine("Поздравляем с приобретением!"); }
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        leaveshop = true;
                        break;
                }

            } while (leaveshop == false);
            Console.Clear();
        }

        static void Main(string[] args) 
        {
            try
            {
                FillArtSet();
                Console.WriteLine("Прошёл выброс. Пора лутать арты.");
                bool leavegame = false;
                do
                {
                    
                    Console.WriteLine("Куда отправимся: \n1. Лутать арты. \n2. В магазин.\n 3. Завершить игру. ");
                    var gochoise = Console.ReadKey();
                    switch (gochoise.Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            Console.Clear();
                            GoArthant();
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            Console.Clear();
                            GoShop();
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            leavegame = true;
                            Console.Clear();
                            break;
                    }
                    
                } while(leavegame == false);
            }
            catch
            {
                return;
            }
            finally
            {
                Console.WriteLine($"Общий счёт: {score};");
            }
            var anticlose = Console.ReadLine();
        }
    }
}
