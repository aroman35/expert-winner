using System.Linq;
using ExpertWinnerLang.Linker.Abstractions;
using ExpertWinnerLang.Linker.Attributes;

namespace ExpertWinnerLang.Functions.Aggregate
{
    [Function("min")]
    public class Min : IFunction
    {
        public double Execute(double[] argument)
        {
            if (!argument.Any())
                return double.NaN;

            return argument.Min();
        }
    }
}