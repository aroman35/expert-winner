using System.Linq;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Linker.Abstractions;
using ExpertWinnerLang.Linker.Attributes;

namespace ExpertWinnerLang.Functions.Aggregate
{
    [Function("mean")]
    public class Mean : IFunction
    {
        public double Execute(double[] argument)
        {
            if (!argument.Any())
                return double.NaN;
            
            var sum = FunctionsSet.FunctionsMap["sum"].Execute(argument);
            return sum / argument.Length;
        }
    }
}