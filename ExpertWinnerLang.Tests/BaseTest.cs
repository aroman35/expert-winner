using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExpertWinnerLang.Compiler;
using ExpertWinnerLang.Functions.Aggregate;
using ExpertWinnerLang.Linker;
using ExpertWinnerLang.Runtime;
using Shouldly;
using Xunit;

namespace ExpertWinnerLang.Tests
{
    public class BaseTest
    {
        private readonly Dictionary<string, double>[] _sourceDataSet;
        private readonly double[][] _data;
        private readonly ExpertWinnerLangRuntime _runtime;

        public BaseTest()
        {
           _sourceDataSet = new [] {
                new Dictionary<string, double>
                {
                    { "total", 10.5d },
                    { "test_01", 250d },
                    { "test_2", 10d },
                    { "result", 4}
                },
                new Dictionary<string, double>
                {
                    { "total", 20d },
                    { "test_01", 250d },
                    { "test_2", 10d },
                    { "result", 2}
                }
            };

            _data = _sourceDataSet.Select(x => x.Values.ToArray()).ToArray();
            
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

        [Fact]
        public void StdWithSelectTest()
        {
            var formula = "std('result')";
            
            var headers = _sourceDataSet[0].Keys.ToArray();
            formula = ReplaceSelect(formula, headers);
            var result = _runtime.Execute(_data, formula);
            
            result.ShouldBe(1);
        }
        
        [Fact]
        public void CombinedFormulaWithSelectTest()
        {
            var formula = "(sum('total') * 2 + 39) / 4";
            
            var headers = _sourceDataSet[0].Keys.ToArray();
            formula = ReplaceSelect(formula, headers);
            var result = _runtime.Execute(_data, formula);
            
            result.ShouldBe(25);
        }
        
        private static string ReplaceSelect(string formula, string[] keys)
        {
            var pattern = "([']([a-zA-z0-9]*)['])|([\"]([a-zA-z0-9]*)[\"])";
            var regex = Regex.Match(formula, pattern);
            var hasFinds = regex.Success;
            while (hasFinds)
            {
                if (!regex.Success) continue;
                var formulated = $"select({Array.IndexOf(keys, regex.Value.Replace("'", "").Replace("\"", ""))})";
                formula = Regex.Replace(formula, $"({regex.Value})", formulated);
                regex = regex.NextMatch();
                hasFinds = regex.Success;
            }

            return formula;
        }
    }
}