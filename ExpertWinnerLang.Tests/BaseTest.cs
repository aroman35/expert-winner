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
        }
        
        [Fact]
        public void ConvertSourceToOutput()
        {
            var compiler = new ExpertWinnerLangCompiler();
            var runtime = new ExpertWinnerLangRuntime(compiler);
            var result = runtime.Execute(_sourceData, "(sum('total') * 2 + 39) / 4");
            
            result.ShouldBe(25);
        }

        [Fact]
        public void StdTest()
        {
            var formula = "std('result')";
            var compiler = new ExpertWinnerLangCompiler();
            var runtime = new ExpertWinnerLangRuntime(compiler);
            var result = runtime.Execute(_sourceData, formula);
            
            result.ShouldBe(1);
        }
    }
}