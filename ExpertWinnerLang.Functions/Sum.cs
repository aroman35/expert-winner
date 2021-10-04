using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using ExpertWinnerLang.Linker;

namespace ExpertWinnerLang.Functions
{
    [Function("sum")]
    public class Sum : IFunction
    {
        public unsafe double Execute(params double[] argument)
        {
            if (argument.Length == 0)
                return double.NaN;
                
            var vectorSize = 256 / sizeof(double) / 8;
            var accVector = Vector256<double>.Zero;
            int i;
            fixed (double* ptr = argument) {
                for (i = 0; i <= argument.Length - vectorSize; i += vectorSize) {
                    var v = Avx2.LoadVector256(ptr + i);
                    accVector = Avx2.Add(accVector, v);
                }
            }
            double result = 0;
            var temp = stackalloc double[vectorSize];
            Avx2.Store(temp, accVector);
            
            for (var j = 0; j < vectorSize; j++)
                result += temp[j];
            for (; i < argument.Length; i++)
                result += argument[i];
            return result;
        }
    }
}