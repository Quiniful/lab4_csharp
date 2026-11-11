using System;

namespace Lab4
{
    public class QuadraticEquation
    {
        private double _a;
        private double _b;
        private double _c;

        public QuadraticEquation() : this(1, 0, 0)
        {
        }

        public QuadraticEquation(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public QuadraticEquation(QuadraticEquation other)
        {
            ArgumentNullException.ThrowIfNull(other);
            _a = other._a;
            _b = other._b;
            _c = other._c;
        }

        public double A
        {
            get
            {
                return _a;
            }
            set
            {
                CheckFinite(value);
                if (value == 0)
                {
                    throw new ArgumentException(
                        "Коэффициент a не может быть нулём.");
                }
                _a = value;
            }
        }

        public double B
        {
            get
            {
                return _b;
            }
            set
            {
                CheckFinite(value);
                _b = value;
            }
        }

        public double C
        {
            get
            {
                return _c;
            }
            set
            {
                CheckFinite(value);
                _c = value;
            }
        }

        private static void CheckFinite(double value)
        {
            if (!double.IsFinite(value))
            {
                throw new ArgumentException(
                    "Коэффициенты должны быть конечными.");
            }
        }

        private double GetDiscriminant()
        {
            double result = _b * _b - 4 * _a * _c;
            CheckResult(result);
            return result;
        }

        private static void CheckResult(double value)
        {
            if (!double.IsFinite(value))
            {
                throw new OverflowException(
                    "Результат не представим типом double. "
                    + "Используйте меньшие коэффициенты.");
            }
        }

        public double[] FindRoots()
        {
            double discriminant = GetDiscriminant();
            if (discriminant < 0)
            {
                return Array.Empty<double>();
            }
            if (discriminant == 0)
            {
                double root = -(_b / 2) / _a;
                CheckResult(root);
                return new double[] { root };
            }
            double q = -0.5 * (_b + Math.CopySign(
                Math.Sqrt(discriminant), _b));
            double first = q / _a;
            double second = _c / q;
            CheckResult(first);
            CheckResult(second);
            return first <= second
                ? new double[] { first, second }
                : new double[] { second, first };
        }

        public override string ToString()
        {
            return $"a = {_a}; b = {_b}; c = {_c}";
        }

        public static QuadraticEquation operator ++(
            QuadraticEquation equation)
        {
            ArgumentNullException.ThrowIfNull(equation);
            return new QuadraticEquation(equation._a + 1,
                equation._b + 1, equation._c + 1);
        }

        public static QuadraticEquation operator --(
            QuadraticEquation equation)
        {
            ArgumentNullException.ThrowIfNull(equation);
            return new QuadraticEquation(equation._a - 1,
                equation._b - 1, equation._c - 1);
        }

        public static implicit operator double(
            QuadraticEquation equation)
        {
            ArgumentNullException.ThrowIfNull(equation);
            return equation.GetDiscriminant();
        }

        public static explicit operator bool(
            QuadraticEquation equation)
        {
            ArgumentNullException.ThrowIfNull(equation);
            return equation.GetDiscriminant() >= 0;
        }

        public static bool operator ==(
            QuadraticEquation first, QuadraticEquation second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }
            if (first is null || second is null)
            {
                return false;
            }
            return first._a == second._a
                && first._b == second._b
                && first._c == second._c;
        }

        public static bool operator !=(
            QuadraticEquation first, QuadraticEquation second)
        {
            return !(first == second);
        }

        public override bool Equals(object other)
        {
            return other is QuadraticEquation equation
                && this == equation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_a, _b, _c);
        }
    }
}
