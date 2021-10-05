using System;
using System.Linq;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Linker.Abstractions;
using ExpertWinnerLang.Linker.Attributes;

namespace ExpertWinnerLang.Functions.Aggregate
{
    [Function("std")]
    public class Std : IFunction
    {
        public double Execute(double[] argument)
        {
            if (!argument.Any())
                return double.NaN;
            
            var mean = FunctionsSet.FunctionsMap["mean"].Execute(argument);
            var sum = FunctionsSet.FunctionsMap["sum"].Execute(argument.Select(x => Math.Pow(x - mean, 2)).ToArray());
            var result = Math.Sqrt(sum / argument.Length);

            return result;
        }
    }
}