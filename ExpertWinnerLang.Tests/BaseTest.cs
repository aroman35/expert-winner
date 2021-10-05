using System.Collections.Generic;
using System.Linq;
using ExpertWinnerLang.Compiler;
using ExpertWinnerLang.Functions;
using ExpertWinnerLang.Functions.Aggregate;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Runtime;
using Shouldly;
using Xunit;

namespace ExpertWinnerLang.Tests
{
    public class BaseTest
    {
        private readonly double[][] _data;
        private readonly ExpertWinnerLangRuntime _runtime;

        public BaseTest()
        {
            Dictionary<string, double>[] sourceDataSet = {
                new()
                {
                    { "total", 10.5d },
                    { "test_01", 250d },
                    { "test_2", 10d },
                    { "result", 4}
                },
                new()
                {
                    { "total", 20d },
                    { "test_01", 250d },
                    { "test_2", 10d },
                    { "result", 2}
                }
            };

            _data = sourceDataSet.Select(x => x.Values.ToArray()).ToArray();
            
            FunctionsSet.MapAssembly(typeof(Std).Assembly);
            ExpertWinnerLangCompiler compiler = new();
            _runtime = new ExpertWinnerLangRuntime(compiler);
        }
        
        [Fact]
        public void CombinedFormulaTest()
        {
            var formula = "(sum(select(0)) * 2 + 39) / 4";
            var result = _runtime.Execute(_data, formula);
            
            result.ShouldBe(25);
        }

        [Fact]
        public void StdTest()
        {
            var formula = "std(select(3))";
            var result = _runtime.Execute(_data, formula);
            
            result.ShouldBe(1);
        }
    }
}