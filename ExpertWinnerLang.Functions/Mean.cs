using System.Linq;
using ExpertWinnerLang.Linker;

namespace ExpertWinnerLang.Functions
{
    [Function("mean")]
    public class Mean : IFunction
    {
        public double Execute(params double[] argument)
        {
            if (!argument.Any())
                return double.NaN;
            
            var sum = FunctionsSet.FunctionsMap["sum"].Execute(argument);
            return sum / argument.Length;
        }
    }
}