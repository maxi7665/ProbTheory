namespace Lab3.Processing
{
    public class EstimateErrorEngine
    {

        /// <summary>
        /// Среднее значение - статистическое мат ожидание
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Average(IEnumerable<double> values)
        {
            decimal sum = 0;

            int cnt = 0;

            foreach (var value in values)
            {
                sum += (decimal)value;

                cnt++;
            }

            sum /= cnt;

            return (double)sum;
        }

        /// <summary>
        /// Полусумма
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double HalfSum(IEnumerable<double> values)
        {
            return (values.Max() + values.Min()) / 2;
        }

        /// <summary>
        /// Медиана
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Median(IEnumerable<double> values)
        {
            var cnt = values.Count();

            values = values.Order();

            if (cnt % 2 == 0)
            {
                return (values.ElementAt(cnt / 2) + values.ElementAt(cnt / 2 - 1)) / 2;
            }
            else
            {
                return values.ElementAt(cnt / 2);
            }
        }

        /// <summary>
        /// Среднее значение с отбросом K членов
        /// </summary>
        /// <param name="values"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static double SegmentAverage(
            IEnumerable<double> values, 
            int k)
        {
            var cnt = values.Count();

            values = values.Order();

            if (k >= cnt / 2)
            {
                throw new ApplicationException("k greater or equal than N/2");
            }

            return Average(values.Skip(k).Take(cnt - 2 * k));
        }

        /// <summary>
        /// Дисперсия
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Dispersion (IEnumerable<double> values)
        {
            var cnt = values.Count();

            decimal sum = 0;
            double average = Average(values);

            foreach (var value in values)
            {
                var difference = value - average;

                var quad = Math.Pow(difference, 2);

                sum += (decimal)quad;
            }

            var ret = sum / (cnt - 1);

            return (double)ret;
        }

        /// <summary>
        /// Относительная ошибка оценивания
        /// </summary>
        public static double RelativeEstimationError(double c, double x) =>
            Math.Abs(c - x) / c;
    }
}
