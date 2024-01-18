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
    }
}
