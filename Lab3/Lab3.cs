using Core;
using Lab3.Processing;
using System.Net.Http.Headers;
using static Core.Utils;

namespace Lab3
{
    internal class Lab3
    {
        private const int VARIANT = 2;
        private const int C = VARIANT;

        private static readonly int[] sampleCounts = [15, 30, 100, 1000];

        private const string REPORT_FILENAME = "reportTemplate.xlsx";

        static async Task Main(string[] args)
        {
            // подготовка набора данных
            await PrepareDataSet(sampleCounts, _generators);

            var valuesParameters = await ProcessData(
                sampleCounts, 
                _generators.Count);

            CreateReport(valuesParameters);
        }

        private static void CreateReport(ValuesParameters[] parameters)
        {
            var newFileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.xlsx";

            using (var excelBuilder = new ExcelReportBuilder(
                REPORT_FILENAME,
                newFileName))
            {
                foreach (var param in parameters)
                {
                    excelBuilder.AppendReportLine(param);
                }

                excelBuilder.SaveReport();
            }

            Utils.OpenPath(newFileName);
        }

        /// <summary>
        /// Считывание и обработка данных
        /// </summary>
        /// <param name="counts"></param>
        /// <param name="generatorsNum"></param>
        /// <returns></returns>
        private async static Task<ValuesParameters[]> ProcessData(int[] counts, int generatorsNum)
        {
            var ret = new List<ValuesParameters>();

            foreach (var count in counts)
            {
                for (int i = 0; i < generatorsNum; i++)
                {
                    var fileName = BuildDataFileName(count, i + 1);

                    var values = await ReadValues(fileName);

                    var parameters = new ValuesParameters(
                        values, 
                        3, 
                        C);

                    ret.Add(parameters);
                }
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Генераторы последовательностей согласно заданию
        /// </summary>
        private static List<ValuesGenerator> _generators = new()
        {
            new (            
                count => PrepareGaussData(
                    count,
                    0,
                    (double) C / 100)
            ),
            new (            
                count => PrepareGaussData(
                    count,
                    0,
                    (double) C / 20)
            ),
            new (            
                count => PrepareUniformData(
                    count,
                    (double) - C / 100,
                    (double) C / 100)
            ),
            new (            
                count => PrepareUniformData(
                    count,
                    (double) - C / 20,
                    (double) C / 20)
            )
        };

        /// <summary>
        /// Подговка набора данных при необходимости
        /// </summary>
        /// <param name="counts">Список кол-ва</param>
        /// <param name="generators"></param>
        /// <returns></returns>
        private static async Task PrepareDataSet(
            int[] counts,
            List<ValuesGenerator> generators)
        {
            foreach (var count in counts)
            {
                for (int i = 0; i < generators.Count; i++)
                {
                    ValuesGenerator generator = generators[i];

                    string fileName = BuildDataFileName(count, i + 1);

                    if (!File.Exists(fileName))
                    {
                        // генерация помехи
                        double[] values = generator.Generator!(count);

                        // добавление к помехе коэффициента
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] += C;
                        }

                        await SaveValues(fileName, values);
                    }
                }
            }
        }

        private static string BuildDataFileName(int count, int order)
        {
            return $"{order}_{count}_values.txt";
        }
    }
}
