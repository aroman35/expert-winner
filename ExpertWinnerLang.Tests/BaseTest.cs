using System.Collections.Generic;
using System.Linq;
using ExpertWinnerLang.Compiler;
using ExpertWinnerLang.Functions;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Runtime;
using Shouldly;
using Xunit;

namespace ExpertWinnerLang.Tests
{
    public class BaseTest
    {
        private readonly Dictionary<string, double>[] _sourceData;
        private readonly ExpertWinnerLangCompiler _compiler;
        private readonly ExpertWinnerLangRuntime _runtime;

        public BaseTest()
        {
            _sourceData = new[]
            {
                new Dictionary<string, double>()
                {
                    { "total", 10.5d },
                    { "test_01", 250d },
                    { "test_2", 10d },
                    { "result", 4}
                },
                new Dictionary<string, double>()
                {
                    { "total", 20d },
                    { "test_01", 250d },
                    { "test_2", 10d },
                    { "result", 2}
                }
            };
            
            FunctionsSet.MapAssembly(typeof(Std).Assembly);
            _compiler = new ExpertWinnerLangCompiler();
            _runtime = new ExpertWinnerLangRuntime(_compiler);
        }
        
        [Fact]
        public void CombinedFormulaTest()
        {
            var formula = "(sum('total') * 2 + 39) / 4";
            var result = _runtime.Execute(_sourceData, formula);
            
            result.ShouldBe(25);
        }

        [Fact]
        public void StdTest()
        {
            var formula = "std('result')";
            var result = _runtime.Execute(_sourceData, formula);
            
            result.ShouldBe(1);
        }
    }
}