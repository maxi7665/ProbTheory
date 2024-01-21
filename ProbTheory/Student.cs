namespace Lab4
{
    /// <summary>
    /// Code from https://www.meracalculator.com/math/t-distribution-critical-value-table.php
    /// </summary>
    public class Student
    {
        //easyRoundOf(resConvert(pStuT(bhname, df2)), 4);

        public static double GetT(double a, double v)
        {
            return resConvert(pStuT(a, v));
        }

        public static double stuT(double a, double b)
        {
            a = Math.Abs(a);

            var c = a / Math.Sqrt(b);
            var d = Math.Atan(c);

            if (1 == b)
            {
                return d / (Math.PI / 2);
            }

            var e = Math.Sin(d);
            var f = Math.Cos(d);

            var alpha = b % 2 == 1
                ? 1 - (d + e * f * stuComp(f * f, 2, b - 3, -1)) / (Math.PI / 2)
                : 1 - e * stuComp(f * f, 1, b - 3, -1);

            return 1.0 - alpha;
        }
        public static double stuComp(double a, double b, double c, double d)
        {
            double e = 1;
            var f = e;
            var g = b;
            for (; g <= c;)
            {
                e = e * a * g / (g - d);
                f += e;
                g += 2;
            }
            return f;
        }

        public static double pStuT(double a, double b)
        {
            var c = .5;
            var d = .5;
            double e = 0;

            for (; d > 1e-6;)
            {
                e = (double)1 / c - 1;
                d /= 2;

                var qt = 1 - stuT(e, b);

                if (qt > a)
                {
                    c -= d;
                }
                else
                {
                    c += d;
                }
            }

            return e;
        }
        public static double resConvert(double a)
        {
            //var b;
            return a >= 0 ? a + 5e-4 : a - 5e-4;
        }

        public static double easyRoundOf(double a, int b)
        {
            /*if (isNaN(a))
                return 0;*/

            var level = Math.Pow(10, b);

            var c = Math.Round(a * level) / level;

            return c;
        }
        /*function parseConv(a)
        {
            return parseFloat(a)
        }*/


    }
}
