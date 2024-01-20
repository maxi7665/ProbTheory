using System.Diagnostics;

namespace Core
{
    public class Utils
    {
        public static void OpenPath(string fileName)
        {
            var processStart = new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = $"/c {fileName}"
            };

            var process = new Process();

            process.StartInfo = processStart;

            process.Start();
        }

        public static double[] GetUniformRandomValues(int count)
        {
            var randomValues = new double[count];

            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                randomValues[i] = random.NextDouble();
            }

            return randomValues;
        }

        public static async Task SaveValues(string fileName, double[] values)
        {
            await File.AppendAllLinesAsync(
                fileName, 
                values.Select(v => v.ToString()));
        }

        public static async Task<double[]> ReadValues(string fileName)
        {
            return (await File.ReadAllLinesAsync(fileName))
                .Select(double.Parse)
                .ToArray();
        }

        /// <summary>
        /// подтоговка значений равномерно распределенной случайной величины
        /// </summary>
        /// <param name="count"> кол-во значений </param>
        /// <param name="a"> значение a для линейного преобразования </param>
        /// <param name="b"> значение b для линейного преобразования </param>
        /// <returns></returns>

        public static double[] PrepareUniformData(int count, double a = 0, double b = 1)
        {
            var values = Utils.GetUniformRandomValues(count).Select(v => UniformLinearTransform(v, a, b));

            return values.ToArray();
        }

        /// <summary>
        /// Линейное преобразование значения для элемента ряда равномерного распределения
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double UniformLinearTransform(
            double value,
            double a,
            double b)
        {
            return a + value * (b - a);
        }



        public static double[] PrepareGaussData(int count, double m, double sigma)
        {
            var values = Utils.GetUniformRandomValues(count * 2);

            var ret = new double[count];

            for (int i = 0; i < count; i++)
            {
                ret[i] = CreateGaussValue(values[i], values[i + 1]);
            }

            ret = ret
                .Select(v => GaussLinearTransform(v, m, sigma))
                .ToArray();

            return ret;
        }


        /// <summary>
        /// Линейное преобразование элемента нормально распределеного ряда
        /// </summary>
        /// <param name="value"></param>
        /// <param name="m"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double GaussLinearTransform(
            double value,
            double m,
            double sigma)
        {
            return m + sigma * value;
        }


        /// <summary>
        /// распределение произведения двух независимых случайных величин, 
        /// одна из которых имеет распределение Релея, 
        /// а другая распределена по закону арксинуса, является нормальным
        /// </summary>
        /// <param name="uniformValue1"></param>
        /// <param name="uniformValue2"></param>
        /// <returns></returns>
        public static double CreateGaussValue(
            double uniformValue1,
            double uniformValue2)
        {
            return Math.Sin(2 * Math.PI * uniformValue1)
                * Math.Sqrt(-2 * Math.Log(uniformValue2));
        }
    }
}
