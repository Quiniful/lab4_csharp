using System;
using System.Globalization;
using System.IO;

namespace Lab4
{
    internal static class ConsoleInput
    {
        public static string ReadLine(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (input == null)
            {
                throw new EndOfStreamException();
            }
            return input;
        }

        public static string ReadText(string message)
        {
            string input;
            while (true)
            {
                input = ReadLine(message).Trim();
                if (input.Length > 0)
                {
                    return input;
                }
                Console.WriteLine("Ошибка! Строка пустая.");
            }
        }

        public static int ReadInt(string message)
        {
            int result;
            while (!int.TryParse(ReadLine(message), out result))
            {
                Console.WriteLine("Введите целое число Int32.");
            }
            return result;
        }

        public static int ReadRange(
            string message, int min, int max)
        {
            int result;
            while (true)
            {
                result = ReadInt(message);
                if (result >= min && result <= max)
                {
                    return result;
                }
                Console.WriteLine(
                    $"Введите число от {min} до {max}");
            }
        }

        public static double ReadDouble(string message)
        {
            double result;
            string input;
            while (true)
            {
                input = ReadLine(message).Trim()
                    .Replace(',', '.');
                if (double.TryParse(input, NumberStyles.Float,
                    CultureInfo.InvariantCulture, out result)
                    && double.IsFinite(result))
                {
                    return result;
                }
                Console.WriteLine("Введите конечное число.");
            }
        }
    }
}
