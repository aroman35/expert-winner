using System.Linq;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Linker.Abstractions;
using ExpertWinnerLang.Linker.Attributes;

namespace ExpertWinnerLang.Functions.Aggregate
{
    [Function("max")]
    public class Max : IFunction
    {
        public double Execute(double[] argument)
        {
            if (!argument.Any())
                return double.NaN;

            return argument.Max();
        }
    }
}