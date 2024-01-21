using Core;
using Markdig;
using System.Text;
using System.Web;
using MathNet.Numerics.Distributions;
using Microsoft.Win32.SafeHandles;

namespace Lab4
{
    internal class Lab4
    {
        private const string SOURCE_FILENAME = "sourceData.txt";
        private const string REPORT_FILENAME = "report4.html";

        public static readonly double[] _sourceData = Utils.ReadValues(SOURCE_FILENAME)
            .GetAwaiter()
            .GetResult();

        private const string TMP_DIRECTORY_NAME = "tmp";

        private const double A = 0.025;

        static async Task Main(string[] args)
        {
            PrepareEnvironment();

            var metrics = Metrics.CountMetrics(
                _sourceData,
                [(int)Math.Round(0.55 * Math.Pow(_sourceData.Length, 0.4) + 1),
                 (int)Math.Round(1.25 * Math.Pow(_sourceData.Length, 0.4) - 1)]);

            await CreateReport(metrics);
        }

        private static async Task CreateReport(DataSetMetrics metrics)
        {
            await File.WriteAllLinesAsync(
                REPORT_FILENAME,
                [@"
                <style>
                table, th, td {
                       border: 1px solid black;
                }
                </style>"]);

            var html = new StringBuilder();

            html.AppendLine(Markdown.ToHtml("# Лабораторная работа №4"));

            html.AppendLine(Markdown.ToHtml("# 1. Гистограмма"));

            foreach (var (order, stat) in metrics.QSegmentStatistics)
            {
                var fileName = BuildBars(stat);

                html.AppendLine(Markdown.ToHtml($"## q = {order}"));
                html.AppendLine(Utils.BuildSegmentStatTableHtml(stat));

                html.AppendLine(
                    HttpUtility.UrlDecode(
                        Markdown.ToHtml(
                            $"![График]({fileName} \"График q = {order}\")")));
            }                        

            var staticticsHtml = BuildStatisticsHtml(metrics);

            html.AppendLine(Markdown.ToHtml("# 2. Основные статистические характеристики"));

            html.AppendLine(staticticsHtml);

            html.AppendLine(Markdown.ToHtml("# 3. Проверка гипотезы о нормальности распределения"));
            
            html.AppendLine(BuildNormHtmlTable(metrics));

            html.Append(Markdown.ToHtml("По таблице квантилей 𝜒2 распределения, " +
                "при заданном уровне значимости α=0,025 " +
                "и числе степеней свободы 𝜈=𝑘−𝑟−1=7−2−1=4, критическое значение = 11.143. " +
                "Оснований для отклонения гипотезы нет."));

            await File.AppendAllLinesAsync(
                REPORT_FILENAME, 
                [html.ToString()]);

            Utils.OpenPath(REPORT_FILENAME);
        }

        private static string BuildStatisticsHtml(DataSetMetrics metrics)
        {
            var markdown = new StringBuilder();

            markdown.AppendLine($"+ Мат. ожидание {metrics.ExpectValue}");
            markdown.AppendLine($"+ Медиана {metrics.Median}");
            markdown.AppendLine($"+ Мода {metrics.Mode}");
            markdown.AppendLine($"+ Дисперсия {metrics.D}");
            markdown.AppendLine($"+ Ср. кв. откл. {metrics.Sigma}");
            markdown.AppendLine($"+ Коэффициент ассиметрии {metrics.Skewness}");
            markdown.AppendLine($"+ Коэффициент эксцесса {metrics.ExcessKurtosis}");
            markdown.AppendLine($"+ Ст. ошибка среднего {metrics.StandardError}");
            markdown.AppendLine($"+ Доверительный интервал для среднего, a = " +
                $"{A}: {metrics.ExpectValue} ± {metrics.GetTValue(A) * metrics.StandardError}");

            return Markdown.ToHtml(markdown.ToString());
        }

        private static MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        private static string BuildNormHtmlTable(DataSetMetrics metrics)
        {
            var markdown = new StringBuilder();

            var segments = metrics.QSegmentStatistics.MaxBy(kv => kv.Key).Value;

            //SegmentStatisticsPart? prevPart = null;

            markdown.AppendLine("|№|x j|x j+1|nj|Ф(x)| Ф(x+1)|pj|N * pj|(n_j-N∙p_j )^2|(n_j-N∙p_j )^2/(N∙p_j )|");
            markdown.AppendLine("|---|---|---|---|---|---|---|---|---|---|");

            Func<double, double> norm = value => Norm(value, metrics.ExpectValue, metrics.Sigma);

            int cnt = 0;

            double countedSum = 0;

            foreach (var (_, part) in segments.Parts)
            {
                cnt++;

                var fFrom = cnt == 1 ? 0 : norm(part.From);
                var fTo = cnt == segments.Parts.Count ? 1 : norm(part.To);
                var diff = fTo - fFrom;

                var counted1 = Math.Pow(part.Values.Length - segments.ValuesCount * diff, 2);
                var counted2 = counted1 / (segments.ValuesCount * diff);

                var f = "0.0000";

                markdown.Append($"|{cnt:F5}|{part.From:F5}|{part.To:F5}|{part.Values.Length:F5}");
                markdown.Append($"|{fFrom:F5}|{fTo:F5}|{diff:F5}|{diff * segments.ValuesCount:F5}");
                markdown.AppendLine($"|{counted1:F5}|{counted2:F5}|");

                countedSum += counted2;
            }

            markdown.Append($"|Сумма|-|-|{segments.ValuesCount:F5}");
            markdown.Append($"|-|-|1|{ segments.ValuesCount:F5}");
            markdown.AppendLine($"|χ02=|{countedSum:F5}|");

            return Markdown.ToHtml(markdown.ToString(), pipeline);
        }

        /// <summary>
        /// Нормальная функция распределения
        /// </summary>
        /// <param name="X">Значение</param>
        /// <param name="M">Математическое ожидание</param>
        /// <param name="S">Среднее квадратное отклонение</param>
        /// <returns></returns>
        private static double Norm(double X, double M = 0, double S = 1)
        {
            return Normal.CDF(M, S, X);
        }

        private static string BuildBars(SegmentStatistics statistics)
        {
            var plot = PlotBuilder.BuildGistogram(statistics);

            var fileName = Path.GetRandomFileName() + ".png";

            fileName = Path.Combine(TMP_DIRECTORY_NAME, fileName);

            plot.SavePng(fileName, 800, 600);
            return fileName;
        }

        private static void PrepareEnvironment()
        {
            if (Directory.Exists(TMP_DIRECTORY_NAME))
            {
                Directory.Delete(TMP_DIRECTORY_NAME, true);
            }

            if (!Directory.Exists(TMP_DIRECTORY_NAME))
            {
                Directory.CreateDirectory(TMP_DIRECTORY_NAME);
            }
        }
    }
}
