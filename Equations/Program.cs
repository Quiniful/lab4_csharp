using System;
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
            QuadraticEquation equation = new QuadraticEquation();
            Console.WriteLine(
                $"Конструктор без параметров: {equation}");
            equation = ReadEquation();
            QuadraticEquation copy =
                new QuadraticEquation(equation);
            Console.WriteLine(
                $"Конструктор копирования: {copy}");
            int command = -1;
            while (command != 0)
            {
                Console.WriteLine(
                    $"Текущее уравнение: {equation}");
                Console.WriteLine("1 - Новое уравнение");
                Console.WriteLine(
                    "2 - Изменить свойство A, B или C");
                Console.WriteLine("3 - Вычислить корни");
                Console.WriteLine("4 - ++ (префикс и постфикс)");
                Console.WriteLine("5 - -- (префикс и постфикс)");
                Console.WriteLine(
                    "6 - Приведения double и bool");
                Console.WriteLine("7 - Сравнить уравнения");
                Console.WriteLine("0 - Выход");
                command = ConsoleInput.ReadRange(
                    "Команда: ", 0, 7);
                try
                {
                    Execute(command, ref equation);
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine(error.Message);
                }
                catch (OverflowException error)
                {
                    Console.WriteLine(error.Message);
                }
            }
        }

        private QuadraticEquation ReadEquation()
        {
            double a = ConsoleInput.ReadDouble("a (не ноль): ");
            while (a == 0)
            {
                a = ConsoleInput.ReadDouble(
                    "a не может быть 0: ");
            }
            double b = ConsoleInput.ReadDouble("b: ");
            double c = ConsoleInput.ReadDouble("c: ");
            return new QuadraticEquation(a, b, c);
        }

        private void Execute(int command,
            ref QuadraticEquation equation)
        {
            switch (command)
            {
                case 1:
                    equation = ReadEquation();
                    break;
                case 2:
                    SetCoefficient(equation);
                    break;
                case 3:
                    PrintRoots(equation);
                    break;
                case 4:
                case 5:
                    ShowChange(ref equation, command == 4);
                    break;
                case 6:
                    double discriminant = equation;
                    Console.WriteLine(
                        $"Дискриминант: {discriminant}");
                    Console.WriteLine(
                        $"Корни есть: {(bool)equation}");
                    break;
                case 7:
                    Compare(equation);
                    break;
            }
        }

        private void SetCoefficient(QuadraticEquation equation)
        {
            int field = ConsoleInput.ReadRange(
                "1 - A, 2 - B, 3 - C: ", 1, 3);
            double value = ConsoleInput.ReadDouble("Значение: ");
            switch (field)
            {
                case 1:
                    equation.A = value;
                    break;
                case 2:
                    equation.B = value;
                    break;
                case 3:
                    equation.C = value;
                    break;
            }
        }

        private void PrintRoots(QuadraticEquation equation)
        {
            double[] roots = equation.FindRoots();
            Console.WriteLine(roots.Length == 0
                ? "Действительных корней нет."
                : $"Корни: {string.Join("; ", roots)}");
        }

        private void ShowChange(
            ref QuadraticEquation equation, bool increase)
        {
            int form = ConsoleInput.ReadRange(
                "1 - префикс, 2 - постфикс: ", 1, 2);
            QuadraticEquation result;
            if (increase)
            {
                result = form == 1 ? ++equation : equation++;
            }
            else
            {
                result = form == 1 ? --equation : equation--;
            }
            Console.WriteLine($"Результат выражения: {result}");
            Console.WriteLine($"После операции: {equation}");
        }

        private void Compare(QuadraticEquation equation)
        {
            Console.WriteLine("Второе уравнение:");
            QuadraticEquation other = ReadEquation();
            Console.WriteLine($"==: {equation == other}");
            Console.WriteLine($"!=: {equation != other}");
            Console.WriteLine(
                $"Equals: {equation.Equals(other)}");
            Console.WriteLine(
                $"Хеш первого: {equation.GetHashCode()}");
            Console.WriteLine(
                $"Хеш второго: {other.GetHashCode()}");
        }
    }
}
