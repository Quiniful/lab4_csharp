using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab4
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                new Program().Run();
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Ввод завершён.");
            }
        }

        private void Run()
        {
            int command = -1;
            while (command != 0)
            {
                Console.WriteLine("Лабораторная 4. Вариант 6");
                Console.WriteLine(
                    "1 - Одинаковые элементы List");
                Console.WriteLine("2 - Удаление из LinkedList");
                Console.WriteLine(
                    "3 - Музыкальные предпочтения");
                Console.WriteLine("4 - Гласные буквы в файле");
                Console.WriteLine("5 - Результаты олимпиады");
                Console.WriteLine("0 - Выход");
                command = ConsoleInput.ReadRange(
                    "Команда: ", 0, 5);
                try
                {
                    Execute(command);
                }
                catch (Exception error) when (
                    error is IOException
                        and not EndOfStreamException
                    or UnauthorizedAccessException
                    or FormatException)
                {
                    Console.WriteLine(error.Message);
                }
            }
        }

        private void Execute(int command)
        {
            switch (command)
            {
                case 1:
                    ShowDuplicates();
                    break;
                case 2:
                    ShowRemoval();
                    break;
                case 3:
                    ShowMusic();
                    break;
                case 4:
                    ShowVowels();
                    break;
                case 5:
                    ShowOlympiad();
                    break;
            }
        }

        private List<int> ReadList()
        {
            int count = ConsoleInput.ReadRange(
                "Число элементов (0-10000): ", 0, 10000);
            List<int> values = new List<int>(count);
            for (int index = 0; index < count; index++)
            {
                values.Add(ConsoleInput.ReadInt(
                    $"Элемент {index + 1}: "));
            }
            return values;
        }

        private void ShowDuplicates()
        {
            Console.WriteLine(
                CollectionTasks.HasDuplicates(ReadList())
                ? "Одинаковые элементы есть."
                : "Одинаковых элементов нет.");
        }

        private void ShowRemoval()
        {
            LinkedList<int> values = new LinkedList<int>(
                ReadList());
            int value = ConsoleInput.ReadInt(
                "Удалить элемент: ");
            Console.WriteLine(CollectionTasks.RemoveFirst(
                values, value) ? "Элемент удалён."
                : "Элемент не найден.");
            Console.WriteLine(
                $"Список: {string.Join(" ", values)}");
        }

        private void ShowMusic()
        {
            int count = ConsoleInput.ReadRange(
                "Число произведений (0-10000): ", 0, 10000);
            HashSet<string> catalog = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);
            string name;
            for (int index = 0; index < count; index++)
            {
                name = ConsoleInput.ReadText("Название: ");
                while (!catalog.Add(name))
                {
                    name = ConsoleInput.ReadText(
                        "Название уже есть. Другое: ");
                }
            }
            int listeners = ConsoleInput.ReadRange(
                "Число меломанов (1-10000): ", 1, 10000);
            List<HashSet<string>> preferences =
                new List<HashSet<string>>(listeners);
            HashSet<string> favorite;
            int favorites;
            for (int index = 0; index < listeners; index++)
            {
                Console.WriteLine($"Меломан {index + 1}");
                favorites = ConsoleInput.ReadRange(
                    "Сколько нравится: ", 0, catalog.Count);
                favorite = new HashSet<string>(catalog.Comparer);
                for (int item = 0; item < favorites; item++)
                {
                    name = ConsoleInput.ReadText("Название: ");
                    while (!catalog.Contains(name)
                        || !favorite.Add(name))
                    {
                        name = ConsoleInput.ReadText(
                            "Нужно новое название из перечня: ");
                    }
                }
                preferences.Add(favorite);
            }
            HashSet<string> all;
            HashSet<string> some;
            HashSet<string> none;
            CollectionTasks.ClassifyMusic(catalog, preferences,
                out all, out some, out none);
            PrintMusic("Нравятся всем", all);
            PrintMusic("Нравятся некоторым, но не всем", some);
            PrintMusic("Не нравятся никому", none);
        }

        private void PrintMusic(
            string title, HashSet<string> values)
        {
            Console.WriteLine($"{title}:");
            Console.WriteLine(values.Count == 0
                ? "Нет произведений."
                : string.Join("; ", values));
        }

        private void ShowVowels()
        {
            Directory.CreateDirectory("data");
            int count = ConsoleInput.ReadRange(
                "Число строк (0-10000): ", 0, 10000);
            CollectionTasks.FillTextFile("data/text.txt", count);
            string result = CollectionTasks.FindVowels(
                "data/text.txt");
            Console.WriteLine(
                "Гласные, встречающиеся "
                + "не более чем в одном слове:");
            Console.WriteLine(result.Length == 0
                ? "Таких букв нет." : result);
        }

        private void ShowOlympiad()
        {
            Directory.CreateDirectory("data");
            CollectionTasks.FillOlympiadFile(
                "data/olympiad.txt");
            Console.WriteLine("Лучший участник без победы "
                + "или количество таких участников:");
            Console.WriteLine(CollectionTasks.FindBestNonWinner(
                "data/olympiad.txt"));
        }
    }
}
