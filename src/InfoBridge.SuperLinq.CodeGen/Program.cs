using SuperOffice;

namespace InfoBridge.SuperLinq.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var gen = new Generator();
            string generatedCode = gen.GenerateString(gen.Build());
        }
    }
}
