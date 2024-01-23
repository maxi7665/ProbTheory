using Core;
using Markdig;
using MathNet.Numerics.LinearAlgebra;
using ScottPlot;
using System.Web;

namespace Lab5
{
    internal class Lab5
    {
        private const string REPORT_NAME = "report5.html";
        private const string PLOT_NAME = "plot.png";

        /// <summary>
        /// Исходные данные
        /// </summary>
        private static readonly double[] x = [1, 2, 3, 4, 5];
        private static readonly double[] y = [2, 26, 28, 42, 70];

        /// <summary>
        /// Искомые точки интерполяции
        /// </summary>
        private static readonly double[] y0 = Enumerable.Repeat(y.Average(), x.Length).ToArray();
        private static double[] y1 = new double[x.Length];
        private static double[] y2 = new double[x.Length];

        private static double[] coeff1 = new double[x.Length];
        private static double[] coeff2 = new double[x.Length];

        /// <summary>
        /// Расчеты значений элементов функции аппроксимации на основе векторов x,y
        /// </summary>
        private static readonly double[] x_2 = x.Select(x => x * x).ToArray();
        private static readonly double[] xy = x.Select(x => x * y[(int)(x - 1)]).ToArray();
        private static readonly double[] x_3 = x.Select(x => x * x * x).ToArray();
        private static readonly double[] x_4 = x_2.Select(x => x * x).ToArray();
        private static readonly double[] x_2_y = Enumerable.Range(0, x.Length).Select(i => y[i] * x_2[i]).ToArray();


        static void Main(string[] args)
        {
            var html = Markdown.ToHtml("# Лабораторная работа №5");

            html += Markdown.ToHtml("Z = " + ArrayStr(y));

            html += Markdown.ToHtml("yi = " + y0[0]);
            html += Markdown.ToHtml("y0 = " + ArrayStr(y0));            
            html += Markdown.ToHtml("D = " + GetDispersion(y, y0));

            FillY1();
            FillY2();
            CreatePlot();

            var y1String = ArrayStr(y1);

            html += Markdown.ToHtml("yi = " + $"xi * {coeff1[0]} + {coeff1[1]}");
            html += Markdown.ToHtml("y1 = " + y1String);
            html += Markdown.ToHtml("D = " + GetDispersion(y, y1));


            var y2String = ArrayStr(y2);
            
            html += Markdown.ToHtml("yi = " + $"xi * xi * {coeff2[0]} + xi * {coeff2[1]} + {coeff2[2]};");
            html += Markdown.ToHtml("y2 = " + y2String);
            html += Markdown.ToHtml("D = " + GetDispersion(y, y2));

            html += (
                    HttpUtility.UrlDecode(
                    Markdown.ToHtml(
                            $"![График]({PLOT_NAME} \"График\")")));

            File.WriteAllTextAsync(REPORT_NAME, html);

            Utils.OpenPath(REPORT_NAME);
        }


        /// <summary>
        /// Заполнение первого массива
        /// </summary>
        public static void FillY1()
        {
            var a = new double[,]
            {
                { x.Sum(), x.Length },
                { x_2.Sum(), x.Sum() }
            };

            var b = new double[] { y.Sum(), xy.Sum() };

            var aMatrix = Matrix<double>.Build.DenseOfArray(a);
            var bArray = Vector<double>.Build.DenseOfArray(b);

            var vector = aMatrix.Solve(bArray);

            var res = vector.ToArray();

            for (int i = 0; i < y1.Length; i++)
            {
                y1[i] = x[i] * res[0] + res[1];
            }

            coeff1 = res;
        }

        /// <summary>
        /// Заполнение второго массива
        /// </summary>
        public static void FillY2()
        {
            var a = new double[,]
            {
                {x_2.Sum(), x.Sum(), x.Length},
                {x_3.Sum(), x_2.Sum(), x.Sum()},
                {x_4.Sum(), x_3.Sum(), x_2.Sum()}
            };

            var b = new double[] { y.Sum(), xy.Sum(), x_2_y.Sum() };

            var aMatrix = Matrix<double>.Build.DenseOfArray(a);
            var bArray = Vector<double>.Build.DenseOfArray(b);

            var vector = aMatrix.Solve(bArray);

            var res = vector.ToArray();

            for (int i = 0; i < y2.Length; i++)
            {
                y2[i] = x[i] * x[i] * res[0] + x[i] * res[1] + res[2];
            }

            coeff2 = res;
        }

        public static void CreatePlot()
        {
            Plot plot = new Plot();

            plot.Legend.IsVisible = true;

            var yScatter = plot.Add.Scatter(x, y);

            yScatter.LineWidth = 0;
            yScatter.MarkerSize = 10;
            yScatter.Label = "y";

            var y0Scatter = plot.Add.Scatter(x, y0);

            y0Scatter.LineWidth = 5;
            y0Scatter.MarkerSize = 5;
            y0Scatter.Label = "y0";

            var y1Scatter = plot.Add.Scatter(x, y1);

            y1Scatter.LineWidth = 5;
            y1Scatter.MarkerSize = 5;
            y1Scatter.Label = "y2";

            var y2Scatter = plot.Add.Scatter(x, y2);

            y2Scatter.LineWidth = 5;
            y2Scatter.MarkerSize = 5;
            y2Scatter.Label = "y2";

            plot.SavePng(PLOT_NAME, 800, 600);
        }

        public static string ArrayStr(double[] arr)
        {
            return arr
                .Select(v => v.ToString("F5"))
                .Aggregate((el1, el2) => $"{el1}, {el2}");
        }

        public static  double GetDispersion(double[] orig, double[] values)
        {
            var sum = 0.0;

            for (int i = 0; i < values.Length; i++)
            {
                double value = orig[i];
                sum += Math.Pow(value - values[i], 2);
            }

            return sum / (orig.Length - 1);
        }
    }
}
