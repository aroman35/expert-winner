using System.Collections.Generic;
using ExpertWinnerLang.Compiler;
using ExpertWinnerLang.Runtime;

namespace ExpertWinnerLang
{
    public class ExpertWinner
    {
        static ExpertWinner()
        {
            ExpertWinnerLangCompiler compiler = new();
            _runtime = new ExpertWinnerLangRuntime(compiler);
        }

        private static ExpertWinnerLangRuntime _runtime;

        public static double Execute(IEnumerable<IEnumerable<double>> data, string formula)
        {
            return _runtime.Execute(data, formula);
        }
    }
    
}