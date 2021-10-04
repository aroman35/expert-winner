using System.Linq;
using ExpertWinnerLang.Linker;

namespace ExpertWinnerLang.Functions
{
    [Function("min")]
    public class Min : IFunction
    {
        public double Execute(params double[] argument)
        {
            if (!argument.Any())
                return double.NaN;

            return argument.Min();
        }
    }
}