using Lab3.Processing;
using static Core.Utils;

namespace Lab3
{
    internal class Lab3
    {
        private const int VARIANT = 2;
        private const int C = VARIANT;        

        static async Task Main(string[] args)
        {
            // подготовка набора данных
            await PrepareDataSet([15, 30, 100, 1000], _generators);
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
        /// 
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
            return $"{count}_{order}_values.txt";
        }


    }
}
