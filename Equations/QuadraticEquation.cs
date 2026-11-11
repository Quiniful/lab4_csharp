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
            int exponent;
            double discriminant = GetScaledDiscriminant(
                out _, out _, out _, out exponent);
            double result = Math.ScaleB(
                discriminant, 2 * exponent);
            CheckResult(result);
            return result;
        }

        private double GetScaledDiscriminant(out double a,
            out double b, out double c, out int exponent)
        {
            double maximum = Math.Max(Math.Abs(_a),
                Math.Max(Math.Abs(_b), Math.Abs(_c)));
            exponent = Math.ILogB(maximum);
            a = Math.ScaleB(_a, -exponent);
            b = Math.ScaleB(_b, -exponent);
            c = Math.ScaleB(_c, -exponent);
            if (a == 0 || b == 0 && _b != 0
                || c == 0 && _c != 0)
            {
                throw new OverflowException(
                    "Разброс коэффициентов слишком велик "
                    + "для вычислений double.");
            }
            return b * b - 4 * a * c;
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
            double a;
            double b;
            double c;
            double discriminant = GetScaledDiscriminant(
                out a, out b, out c, out _);
            if (discriminant < 0)
            {
                return Array.Empty<double>();
            }
            if (discriminant == 0)
            {
                double root = -(b / 2) / a;
                CheckResult(root);
                return new double[] { root };
            }
            double q = -0.5 * (b + Math.CopySign(
                Math.Sqrt(discriminant), b));
            double first = q / a;
            double second = c / q;
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
            return equation.GetScaledDiscriminant(
                out _, out _, out _, out _) >= 0;
        }

        public static bool operator ==(
            QuadraticEquation first, QuadraticEquation second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }
            return first is not null && second is not null
                && first._a == second._a
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
