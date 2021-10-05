using System.Collections.Generic;
using System.Linq;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Linker.Abstractions;
using ExpertWinnerLang.Linker.Attributes;

namespace ExpertWinnerLang.Functions.Queries
{
    [Query("select")]
    public class Select : IQuery
    {
        public double[] Execute(IEnumerable<IEnumerable<double>> sequence, int arg)
        {
            var result = sequence
                .Where(row => row.Count() > arg)
                .Select(row => row.Skip(arg).First())
                .ToArray();

            return result;
        }
    }
}