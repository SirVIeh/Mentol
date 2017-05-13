using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Lotek
{
    internal class Program
    {
        private static LuckyNumbers luckyNumbers = new LuckyNumbers();
        private static Activation activation = new Activation();

        private static void Main(string[] args)
        {
            if (!File.Exists("activaction.xml"))
            {
                DownloadLicenseFile();
                ReadFromActivationFile();
            }
            else
            {
                if (activation.Activated == "egal")
                {
                    ReadFromActivationFile();
                }
                else
                {
                    DownloadLicenseFile();
                    ReadFromActivationFile();
                }
            }
            if (activation.Activated == "davon" || activation.Activated == "egal")
            {
                luckyNumbers.Numbers = new List<List<int>>();
                ReadFromFile();
                string[] enteredNumbersString;
                var choice = new ConsoleKeyInfo();
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
                    Console.WriteLine("[WYBIERZ OPCJĘ]");
                    Console.WriteLine("1 - Podaj nowy zestaw liczb");
                    Console.WriteLine("2 - Zobacz raport z dotychczasowych losowań");
                    Console.WriteLine("3 - Zakończ");

                    choice = Console.ReadKey();
                    Console.WriteLine("");
                    var numbersToSave = new List<int>();
                    switch (choice.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Clear();
                            Console.WriteLine("Podaj 6 liczb oddzielonych spacjami, aby dodać losowanie, a następnie kliknij \"Enter\" aby zatwierdzić:");
                            enteredNumbersString = Regex.Split(Console.ReadLine(), " ");
                            if (enteredNumbersString.Count() < 6 || enteredNumbersString.Count() > 6)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Podano złą ilość cyfr, bądź w złym formacie.");
                                Console.ResetColor();
                            }
                            else
                            {
                                int numberBeforeAdd = -1;
                                foreach (string stringNumber in enteredNumbersString)
                                {
                                    try
                                    {
                                        numberBeforeAdd = Convert.ToInt32(stringNumber);
                                    }
                                    catch (FormatException)
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Jedna z wartości nie jest liczbą. Spróbuj ponownie.");
                                        Console.ResetColor();
                                        numberBeforeAdd = -1;
                                        break;
                                    }
                                    if (numberBeforeAdd < 1 || numberBeforeAdd > 49)
                                    {
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Tej liczby nie ma w Lotku. Podaj losowanie jeszcze raz.");
                                            Console.ResetColor();
                                            numberBeforeAdd = -1;
                                            break;
                                    }
                                        numbersToSave.Add(numberBeforeAdd);
                                }
                                if (numberBeforeAdd != -1)
                                {
                                    if (numbersToSave.GroupBy(n => n).Any(c => c.Count() > 1))
                                    {
                                        exists = true;
                                    }
                                    if (exists)
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine(
                                            "Nie można podać dwóch takich samych liczb w jednym losowaniu");
                                        Console.ResetColor();
                                        exists = false;
                                        break;
                                    }
                                    if (!CheckIfExists(numbersToSave))
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Zapisano losowanie");
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        luckyNumbers.Numbers.Add(numbersToSave);
                                        Console.WriteLine("Ilość losowań: " + luckyNumbers.Numbers.Count);
                                        foreach (var currentNumbers in luckyNumbers.Numbers)
                                        {
                                            foreach (int i in currentNumbers)
                                            {
                                                Console.Write(i + "/");
                                            }
                                            Console.WriteLine("");
                                        }
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Podane losowanie już istnieje.");
                                        Console.ResetColor();
                                    }
                                    SaveToFile(luckyNumbers);
                                }
                            }
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            DisplayReport();
                            break;
                        case ConsoleKey.D3:
                            Console.Clear();
                            break;
                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Nieprawidłowy numer. Proszę wybrać jedną z dostępnych opcji.");
                            Console.ResetColor();
                            break;
                    }
                }


                Console.WriteLine("Wciśnij dowolny klawisz, aby zakończyć.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine(activation.Activated);
                Console.WriteLine("Wciśnij dowolny klawisz, aby zakończyć.");
                Console.ReadKey();
            }
        }

        private static void DisplayReport()
        {
            int threes = 0, fours = 0, fives = 0, sixes = 0;
            var recentNumbers = new List<int>();
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
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("To nie jest liczba. Spróbuj ponownie.");
                    Console.ResetColor();
                    readedLocalNumber = -1;
                }
                if (readedLocalNumber < 1 || readedLocalNumber > 49)
                {
                    if (readedLocalNumber != -1)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Tej liczby nie ma w Lotku. Wpisz jeszcze raz.");
                        Console.ResetColor();
                    }
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
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nie można podać dwóch takich samych liczb w jednym losowaniu");
                        Console.ResetColor();
                        i--;
                        exists = false;
                    }
                    else
                    {
                        recentNumbers.Add(readedLocalNumber);
                    }
                }
            }
            foreach (var currentNumbers in luckyNumbers.Numbers)
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
                        //default:
                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //    Console.WriteLine("Zła ilość znaleziona = " + countedNumbers);
                        //    Console.ResetColor();
                        //    break;
                }
                Console.WriteLine("");
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*******************RAPORT*********************");
            Console.WriteLine("Ilość trójek: " + threes);
            Console.WriteLine("Ilość czwórek: " + fours);
            Console.WriteLine("Ilość piątek: " + fives);
            Console.WriteLine("Ilość szóstek: " + sixes);
            Console.WriteLine("*******************GRATULACJE*********************");
            Console.ResetColor();
        }

        public static bool CheckIfExists(List<int> list)
        {
            int found = 0;
            foreach (var currentNumbers in luckyNumbers.Numbers)
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
                if (found == 6)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SaveToFile(LuckyNumbers lNumbers)
        {
            var xml = new XmlSerializer(typeof (LuckyNumbers));
            using (Stream fStream = new FileStream("data.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xml.Serialize(fStream, lNumbers);
            }
        }

        public static void SaveToFileAct()
        {
            var xml = new XmlSerializer(typeof (Activation));
            using (Stream fStream = new FileStream("ACT.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xml.Serialize(fStream, activation);
            }
        }

        public static void ReadFromFile()
        {
            var xml = new XmlSerializer(typeof (LuckyNumbers));
            try
            {
                using (Stream fStream = new FileStream("data.xml", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    luckyNumbers = (LuckyNumbers) xml.Deserialize(fStream);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Brak pliku z losowaniami. Tworzę nowy.");
            }
        }

        public static void ReadFromActivationFile()
        {
            var xml = new XmlSerializer(typeof (Activation));
            try
            {
                using (
                    Stream fStream = new FileStream("activaction.xml", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    activation = (Activation) xml.Deserialize(fStream);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Brak pliku z losowaniami. Tworzę nowy.");
            }
        }

        public static void DownloadLicenseFile()
        {
            File.Delete("activaction.xml");
            Console.WriteLine("Proszę czekać...");
            using (var client = new WebClient())
            {
                client.DownloadFile("https://docs.google.com/uc?export=download&id=0B-axT0J2c_GhR0pRWDhhLWdFcjQ",
                    "activaction.xml");
            }
            Console.WriteLine("Gotowe");
        }
    }
}