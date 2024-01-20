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
    }
}
