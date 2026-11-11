using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lab4
{
    public static class CollectionTasks
    {
        public static bool HasDuplicates(List<int> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            HashSet<int> found = new HashSet<int>();
            foreach (int value in values)
            {
                if (!found.Add(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool RemoveFirst(
            LinkedList<int> values, int value)
        {
            ArgumentNullException.ThrowIfNull(values);
            return values.Remove(value);
        }

        public static void ClassifyMusic(
            HashSet<string> catalog,
            List<HashSet<string>> preferences,
            out HashSet<string> all,
            out HashSet<string> some,
            out HashSet<string> none)
        {
            ArgumentNullException.ThrowIfNull(catalog);
            ArgumentNullException.ThrowIfNull(preferences);
            if (preferences.Count == 0)
            {
                throw new ArgumentException(
                    "Нужен хотя бы один меломан.");
            }
            all = new HashSet<string>(
                catalog, catalog.Comparer);
            some = new HashSet<string>(catalog.Comparer);
            none = new HashSet<string>(
                catalog, catalog.Comparer);
            foreach (HashSet<string> favorite in preferences)
            {
                ArgumentNullException.ThrowIfNull(favorite);
                if (!catalog.IsSupersetOf(favorite))
                {
                    throw new ArgumentException(
                        "Произведение отсутствует в перечне.");
                }
                all.IntersectWith(favorite);
                some.UnionWith(favorite);
            }
            none.ExceptWith(some);
            some.ExceptWith(all);
        }

        public static void FillTextFile(
            string path, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            using StreamWriter writer = new StreamWriter(
                path, false, Encoding.UTF8);
            for (int index = 0; index < count; index++)
            {
                writer.WriteLine(ConsoleInput.ReadLine(
                    $"Строка {index + 1}: "));
            }
        }

        public static string FindVowels(string path)
        {
            const string vowels = "аеёиоуыэюя";
            int[] occurrences = new int[vowels.Length];
            HashSet<char> word = new HashSet<char>();
            bool inWord = false;
            int code;
            char symbol;
            using StreamReader reader = new StreamReader(
                path, Encoding.UTF8);
            while ((code = reader.Read()) != -1)
            {
                symbol = char.ToLowerInvariant((char)code);
                if (IsRussianLetter(symbol))
                {
                    inWord = true;
                    word.Add(symbol);
                }
                else if (inWord)
                {
                    CountOccurrences(word, occurrences, vowels);
                    word.Clear();
                    inWord = false;
                }
            }
            if (inWord)
            {
                CountOccurrences(word, occurrences, vowels);
            }
            StringBuilder result = new StringBuilder();
            for (int index = 0; index < vowels.Length; index++)
            {
                if (occurrences[index] <= 1)
                {
                    result.Append(vowels[index]);
                }
            }
            return result.ToString();
        }

        private static bool IsRussianLetter(char symbol)
        {
            return symbol >= 'а' && symbol <= 'я'
                || symbol == 'ё';
        }

        private static void CountOccurrences(HashSet<char> word,
            int[] occurrences, string vowels)
        {
            for (int index = 0; index < vowels.Length; index++)
            {
                if (word.Contains(vowels[index])
                    && occurrences[index] < 2)
                {
                    occurrences[index]++;
                }
            }
        }

        public static bool IsParticipant(string line,
            out int score, out string name)
        {
            score = 0;
            name = "";
            if (line == null)
            {
                return false;
            }
            string[] parts = line.Split(' ');
            int grade;
            if (parts.Length != 4
                || !IsName(parts[0], 20)
                || !IsName(parts[1], 15)
                || !int.TryParse(parts[2], NumberStyles.None,
                    CultureInfo.InvariantCulture, out grade)
                || grade < 7 || grade > 11
                || !int.TryParse(parts[3], NumberStyles.None,
                    CultureInfo.InvariantCulture, out score)
                || score < 0)
            {
                return false;
            }
            name = $"{parts[0]} {parts[1]}";
            return true;
        }

        private static bool IsName(string value, int max)
        {
            if (value.Length == 0 || value.Length > max)
            {
                return false;
            }
            foreach (char symbol in value)
            {
                if (char.IsWhiteSpace(symbol))
                {
                    return false;
                }
            }
            return true;
        }

        public static void FillOlympiadFile(string path)
        {
            int count = ConsoleInput.ReadRange(
                "Количество участников: ", 1, int.MaxValue);
            string line;
            int score;
            string name;
            using StreamWriter writer = new StreamWriter(
                path, false, Encoding.UTF8);
            writer.WriteLine(count);
            Console.WriteLine("Фамилия Имя класс баллы");
            for (int index = 0; index < count; index++)
            {
                line = ConsoleInput.ReadLine(
                    $"Участник {index + 1}: ");
                while (!IsParticipant(line, out score, out name))
                {
                    Console.WriteLine(
                        "Ошибка формата: имя до 15, "
                        + "фамилия до 20 символов, класс 7-11, "
                        + "баллы 0-2147483647; один пробел.");
                    line = ConsoleInput.ReadLine("Повторите: ");
                }
                writer.WriteLine(line);
            }
        }

        public static string FindBestNonWinner(string path)
        {
            Dictionary<int, (int Count, string Name)> leaders =
                new Dictionary<int, (int Count, string Name)>();
            using StreamReader reader = new StreamReader(
                path, Encoding.UTF8);
            int count;
            if (!int.TryParse(reader.ReadLine(), out count)
                || count < 1)
            {
                throw new FormatException(
                    "Некорректное число участников в файле.");
            }
            int score;
            string name;
            for (int index = 0; index < count; index++)
            {
                if (!IsParticipant(reader.ReadLine(),
                    out score, out name))
                {
                    throw new FormatException(
                        $"Ошибка в записи {index + 1}.");
                }
                UpdateLeaders(leaders, score, name);
            }
            if (reader.ReadLine() != null)
            {
                throw new FormatException(
                    "В файле есть лишние строки.");
            }
            int best = -1;
            int second = -1;
            foreach (int value in leaders.Keys)
            {
                if (value > best)
                {
                    second = best;
                    best = value;
                }
                else
                {
                    second = value;
                }
            }
            bool winners = best > 200
                && (leaders[best].Count == 1
                || (long)leaders[best].Count * 5 <= count);
            int target = winners ? second : best;
            if (target == -1)
            {
                throw new FormatException(
                    "Нет участника, не ставшего победителем.");
            }
            (int Count, string Name) result = leaders[target];
            return result.Count == 1
                ? result.Name : result.Count.ToString();
        }

        private static void UpdateLeaders(
            Dictionary<int, (int Count, string Name)> leaders,
            int score, string name)
        {
            (int Count, string Name) group;
            if (leaders.TryGetValue(score, out group))
            {
                leaders[score] = (group.Count + 1, group.Name);
            }
            else
            {
                leaders.Add(score, (1, name));
            }
            if (leaders.Count > 2)
            {
                int minimum = int.MaxValue;
                foreach (int value in leaders.Keys)
                {
                    minimum = Math.Min(minimum, value);
                }
                leaders.Remove(minimum);
            }
        }
    }
}
