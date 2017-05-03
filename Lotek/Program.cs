using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lotek
{
    class Program
    {
        static LuckyNumbers luckyNumbers = new LuckyNumbers();
        static void Main(string[] args)
        {
            luckyNumbers.Numbers = new List<List<int>>();
            ReadFromFile();
            int readedLocalNumber = 0;
            ConsoleKeyInfo choice = new ConsoleKeyInfo();
            bool exists = false;
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            while (choice.Key != ConsoleKey.D3)
            {
                Console.WriteLine("*******************LOTTO*********************");
                Console.WriteLine("1 - Podaj nowy zestaw liczb");
                Console.WriteLine("2 - Zobacz raport z dotychczasowych losowań");
                Console.WriteLine("3 - Zakończ");

                choice = Console.ReadKey();
                Console.WriteLine("");
                List<int> numbersToSave = new List<int>();
                switch (choice.Key)
                {
                    case ConsoleKey.D1:
                        for (int i = 0; i < 6; i++)
                        {
                            int num = i + 1;
                            Console.WriteLine("Podaj liczbę [" + num + "] :");
                            try
                            {
                                readedLocalNumber = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("To nie jest liczba. Spróbuj ponownie.");
                                readedLocalNumber = -1;
                            }
                            if (readedLocalNumber < 1 || readedLocalNumber > 49)
                            {
                                if (readedLocalNumber != -1)
                                    Console.WriteLine("Tej liczby nie ma w Lotku. Wpisz jeszcze raz.");
                                i--;
                            }
                            else
                            {
                                foreach (int nToSave in numbersToSave)
                                {
                                    if (nToSave == readedLocalNumber)
                                    {
                                        exists = true;
                                    }
                                }
                                if (exists)
                                {
                                    Console.WriteLine("Nie można podać dwóch takich samych liczb w jednym losowaniu");
                                    i--;
                                    exists = false;
                                }
                                else
                                {
                                    numbersToSave.Add(readedLocalNumber);
                                }
                            }
                        }
                        if (!CheckIfExists(numbersToSave))
                        {
                            Console.WriteLine("Zapisuję podane liczby...");
                            luckyNumbers.Numbers.Add(numbersToSave);
                            Console.WriteLine("Ilość losowań: " + luckyNumbers.Numbers.Count);
                            foreach (List<int> currentNumbers in luckyNumbers.Numbers)
                            {
                                foreach (int i in currentNumbers)
                                {
                                    Console.Write(i + "/");
                                }
                                Console.WriteLine("");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Podane losowanie już istnieje.");
                        }
                        break;
                    case ConsoleKey.D2:
                        DisplayReport();
                        break;
                    case ConsoleKey.D3:
                        SaveToFile(luckyNumbers);
                        break;
                }
            }


            Console.WriteLine("Wciśnij dowolny klawisz, aby zakończyć.");
            Console.ReadKey();
        }

        private static void DisplayReport()
        {
            int threes = 0, fours = 0, fives = 0, sixes = 0;
            List<int> recentNumbers = new List<int>();
            int readedLocalNumber;
            bool exists = false;
            Console.WriteLine("Podaj kolejno liczby z ostatniego losowania");
            for (int i = 0; i < 6; i++)
            {
                int num = i + 1;
                Console.WriteLine("Podaj liczbę [" + num + "] :");
                try
                {
                    readedLocalNumber = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("To nie jest liczba. Spróbuj ponownie.");
                    readedLocalNumber = -1;
                }
                if (readedLocalNumber < 1 || readedLocalNumber > 49)
                {
                    if (readedLocalNumber != -1)
                        Console.WriteLine("Tej liczby nie ma w Lotku. Wpisz jeszcze raz.");
                    i--;
                }
                else
                {
                    foreach (int nToSave in recentNumbers)
                    {
                        if (nToSave == readedLocalNumber)
                        {
                            exists = true;
                        }
                    }
                    if (exists)
                    {
                        Console.WriteLine("Nie można podać dwóch takich samych liczb w jednym losowaniu");
                        i--;
                        exists = false;
                    }
                    else
                    {
                        recentNumbers.Add(readedLocalNumber);
                    }
                }
            }
            foreach (List<int> currentNumbers in luckyNumbers.Numbers)
            {
                int countedNumbers = 0;
                bool isFound;
                foreach (int number in recentNumbers)
                {
                    isFound = currentNumbers.Contains(number);
                    if (isFound)
                    {
                        countedNumbers++;
                        isFound = false;
                    }
                }
                switch (countedNumbers)
                {
                    case 3:
                        threes++;
                        break;
                    case 4:
                        fours++;
                        break;
                    case 5:
                        fives++;
                        break;
                    case 6:
                        sixes++;
                        break;
                    default:
                        Console.WriteLine("Zła ilość znaleziona = " + countedNumbers);
                        break;
                }
                Console.WriteLine("");
            }
            Console.WriteLine("*******************RAPORT*********************");
            Console.WriteLine("Ilość trójek: " + threes);
            Console.WriteLine("Ilość czwórek: " + fours);
            Console.WriteLine("Ilość piątek: " + fives);
            Console.WriteLine("Ilość szóstek: " + sixes);
            Console.WriteLine("*******************GRATULACJE*********************");
        }

        public static bool CheckIfExists(List<int> list)
        {
            int found = 0;
            foreach (List<int> currentNumbers in luckyNumbers.Numbers)
            {
                found = 0;
                foreach (int i in currentNumbers)
                {
                    foreach (int lToSave in list)
                    {
                        if (i == lToSave)
                        {
                            found ++;
                        }
                    }
                }
            }
            if (found == 6)
            {
                return true;
            }
            return false;
        }

        public static void SaveToFile(LuckyNumbers lNumbers)
        {
            XmlSerializer xml = new XmlSerializer(typeof(LuckyNumbers));
            using (Stream fStream = new FileStream("data.xml", FileMode.Create, FileAccess.Write , FileShare.None))
            {
                xml.Serialize(fStream, lNumbers);
            }
        }
        public static void ReadFromFile()
        {
            XmlSerializer xml = new XmlSerializer(typeof(LuckyNumbers));
            try
            {
                using (Stream fStream = new FileStream("data.xml", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    luckyNumbers = (LuckyNumbers)xml.Deserialize(fStream);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Brak pliku z losowaniami. Tworzę nowy.");
            }
        }
    }
}
