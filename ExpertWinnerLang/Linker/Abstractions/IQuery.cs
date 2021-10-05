using System.Collections.Generic;

namespace ExpertWinnerLang.Linker.Abstractions
{
    public interface IQuery
    {
        public double[] Execute(IEnumerable<IEnumerable<double>> sequence, int arg);
    }
}