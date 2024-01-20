namespace Lab3.Processing
{
    internal class ValuesGenerator
    {
        public ValuesGenerator(Func<int, double[]> generator)
        {
            Generator = generator;
        }

        public Func<int, double[]> Generator { get; } 
    }
}
