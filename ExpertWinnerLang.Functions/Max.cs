using System.Linq;
using ExpertWinnerLang.Linker;

namespace ExpertWinnerLang.Functions
{
    [Function("max")]
    public class Max : IFunction
    {
        public double Execute(params double[] argument)
        {
            if (!argument.Any())
                return double.NaN;

            return argument.Max();
        }
    }
}