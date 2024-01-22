using Lab4;

namespace Core
{
    public class DataSetMetrics
    {
        /// <summary>
        /// Кол-во наблюдений
        /// </summary>
        public double N { get; set; }


        /// <summary>
        /// Размах
        /// </summary>
        public double R { get; set; }
        
        /// <summary>
        /// Математическое ожидание
        /// </summary>
        public double ExpectValue { get; internal set; }
        
        /// <summary>
        /// Мода
        /// </summary>
        public double Mode { get; internal set; }
        
        /// <summary>
        /// Медиана
        /// </summary>
        public double Median { get; internal set; }

        /// <summary>
        /// Статистика сегментированная (распреление для гистограммы)
        /// </summary>
        public Dictionary<int, SegmentStatistics> QSegmentStatistics { get; set; } = new();
        
        /// <summary>
        /// Дисперсия
        /// </summary>
        public double D { get; internal set; }

        /// <summary>
        /// СКО
        /// </summary>
        public double Sigma => Math.Sqrt(D);


        /// <summary>
        /// Коэффициент ассиметрии
        /// </summary>
        public double Skewness { get; internal set; }

        /// <summary>
        /// Коэффициент  эксцесса
        /// </summary>
        public double ExcessKurtosis { get; internal set; }

        /// <summary>
        /// Стандартная ошибка среднего
        /// </summary>
        public double StandardError { get; internal set; }

        /// <summary>
        /// Получить квантиль распределения Стьюдента
        /// </summary>
        /// <param name="a">Уровень значимости α</param>
        /// <returns></returns>
        public double GetTValue(double a)
        {
            var v = N - 1;

            return Student.GetT(a, v);
        }
    }
}
