
using Core;
using Lab2.Processing;
using Markdig;
using System.Text;
using System.Web;
using static Core.Utils;

namespace Lab2
{
    internal class Lab2
    {
        private const string TMP_DIRECTORY_NAME = "tmp";

        private const string UNIFORM_100_FILENAME = $"2_uniform_100.txt";
        private const string UNIFORM_1000_FILENAME = $"2_uniform_1000.txt";
        private const string GAUSS_100_FILENAME = $"2_gauss_100.txt";
        private const string GAUSS_1000_FILENAME = $"2_gauss_1000.txt";

        private static readonly string REPORT_FILENAME = "report.html";

        private const int VARIANT = 2;

        private static readonly double _a = (double)-VARIANT / 10;
        private static readonly double _b = (double)VARIANT / 2;

        private static readonly double _m = (double)VARIANT;
        private static readonly double _sigma = (double)VARIANT / 3;

        public static async Task Main(string[] args)
        {
            await PrepareDataSet();

            var uniform100Values = await Utils.ReadValues(UNIFORM_100_FILENAME);
            var uniform1000Values = await Utils.ReadValues(UNIFORM_1000_FILENAME);
            var gauss100Values = await Utils.ReadValues(GAUSS_100_FILENAME);
            var gauss1000Values = await Utils.ReadValues(GAUSS_1000_FILENAME);

            await PrepareFileEnvironment();

            await File.AppendAllLinesAsync(REPORT_FILENAME, [Markdown.ToHtml($"# Лабораторная работа №2")]);

            await File.AppendAllLinesAsync(
                REPORT_FILENAME,
                [Markdown.ToHtml($" Вариант №{VARIANT}"),
                    Markdown.ToHtml($"a = {_a}, b = {_b}"),
                    Markdown.ToHtml($"M = {_m}, sigma = {_sigma}")]);

            await File.AppendAllLinesAsync(REPORT_FILENAME, [Markdown.ToHtml($"## Равномерное распределение, l = 100")]);

            await ProcessValues(uniform100Values);

            await File.AppendAllLinesAsync(REPORT_FILENAME, [Markdown.ToHtml($"## Равномерное распределение, l = 1000")]);
            await ProcessValues(uniform1000Values);

            await File.AppendAllLinesAsync(REPORT_FILENAME, [Markdown.ToHtml($"## Нормальное распределение, l = 100")]);
            await ProcessValues(gauss100Values);

            await File.AppendAllLinesAsync(REPORT_FILENAME, [Markdown.ToHtml($"## Нормальное распределение, l = 1000")]);
            await ProcessValues(gauss1000Values);

            Utils.OpenPath(REPORT_FILENAME);
        }

        private static async Task PrepareFileEnvironment()
        {
            if (File.Exists(REPORT_FILENAME))
            {
                File.Delete(REPORT_FILENAME);
            }

            await File.WriteAllLinesAsync(REPORT_FILENAME,
                [@"
                <style>
                table, th, td {
                       border: 1px solid black;
                }
                </style>"]);

            if (Directory.Exists(TMP_DIRECTORY_NAME))
            {
                Directory.Delete(TMP_DIRECTORY_NAME, true);
            }

            if (!Directory.Exists(TMP_DIRECTORY_NAME))
            {
                Directory.CreateDirectory(TMP_DIRECTORY_NAME);
            }
        }

        private static async Task ProcessValues(
            double[] values)
        {
            var markdown = new StringBuilder();

            var stats = Metrics.CountMetrics(values, [5, 7]);

            var segmentStatistics = stats.QSegmentStatistics[5];

            markdown.AppendLine(Markdown.ToHtml($"### Статистические показатели"));

            markdown.AppendLine(BuildStatisticsHtml(stats));

            string fileName = BuildBars(segmentStatistics);

            markdown.AppendLine(Markdown.ToHtml($"### q = 5"));
            markdown.AppendLine(BuildTableHtml(segmentStatistics));

            markdown.AppendLine(HttpUtility.UrlDecode(Markdown.ToHtml($"![График]({fileName} \"График q = 5\")")));

            segmentStatistics = stats.QSegmentStatistics[7];

            fileName = BuildBars(segmentStatistics);

            markdown.AppendLine(Markdown.ToHtml($"### q = 7"));
            markdown.AppendLine(BuildTableHtml(segmentStatistics));
            markdown.AppendLine(HttpUtility.UrlDecode(Markdown.ToHtml($"![График]({fileName} \"График q = 7\")")));

            var html = markdown.ToString();

            await File.AppendAllLinesAsync(REPORT_FILENAME, [html]);
        }

        private static string BuildStatisticsHtml(DataSetMetrics metrics)
        {
            var markdown = new StringBuilder();

            markdown.AppendLine($"+ Мат. ожидание {metrics.ExpectValue}");
            markdown.AppendLine($"+ Медиана {metrics.Median}");
            markdown.AppendLine($"+ Мода {metrics.Mode}");
            markdown.AppendLine($"+ Дисперсия {metrics.D}");
            markdown.AppendLine($"+ Ср. кв. откл. {metrics.Sigma}");

            return Markdown.ToHtml(markdown.ToString());
        }

        private static MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        private static string BuildTableHtml(SegmentStatistics segmentStatistics)
        {
            var markdown = new StringBuilder();

            markdown.AppendLine("| Номер интервала  | x j  |  x j\\+1 |  n j  |");
            markdown.AppendLine("|------------------|------|--------|-------|");

            int cnt = 0;

            foreach (var segment in segmentStatistics.Parts)
            {
                cnt++;

                markdown.AppendLine($"| {cnt} | {Math.Round(segment.Value.From, 3)} | " +
                    $"{Math.Round(segment.Value.To, 3)} | {segment.Value.Values.Length} |");
            }

            markdown.AppendLine($"| Сумма |  |  | {segmentStatistics.ValuesCount} |");

            return Markdown.ToHtml(markdown.ToString(), pipeline);
        }

        private static string BuildBars(SegmentStatistics statistics)
        {
            var plot = PlotBuilder.BuildGistogram(statistics);

            var fileName = Path.GetRandomFileName() + ".png";

            fileName = Path.Combine(TMP_DIRECTORY_NAME, fileName);

            fileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            plot.SavePng(fileName, 800, 600);
            return fileName;
        }

        private static async Task PrepareDataSet()
        {
            if (!File.Exists(UNIFORM_100_FILENAME))
            {
                double[] values = PrepareUniformData(100, _a, _b);

                await Utils.SaveValues(UNIFORM_100_FILENAME, values);
            }

            if (!File.Exists(UNIFORM_1000_FILENAME))
            {
                double[] values = PrepareUniformData(1000, _a, _b);

                await Utils.SaveValues(UNIFORM_1000_FILENAME, values);
            }

            if (!File.Exists(GAUSS_100_FILENAME))
            {
                double[] values = PrepareGaussData(100, _m, _sigma);

                await Utils.SaveValues(GAUSS_100_FILENAME, values);
            }

            if (!File.Exists(GAUSS_1000_FILENAME))
            {
                double[] values = PrepareGaussData(1000, _m, _sigma);

                await Utils.SaveValues(GAUSS_1000_FILENAME, values);
            }
        }


        
    }
}
