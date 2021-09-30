using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using ExpertWinnerLang.Compiler;
using ExpertWinnerLang.Extensions;
using ExpertWinnerLang.Functions;
using ExpertWinnerLang.InputParser;
using ExpertWinnerLang.Linker;
using Xunit;

namespace ExpertWinnerLang.Tests
{
    public class BaseTest
    {
        [Fact]
        public void ConvertSourceToOutput()
        {
            var source = new[]
            {
                new Dictionary<string, dynamic>()
                {
                    { "total", 10.5 },
                    { "test_01", 250 },
                    { "test_2", 10 }
                },
                new Dictionary<string, dynamic>()
                {
                    { "total", 10.5 },
                    { "test_01", 250 },
                    { "test_2", 10 }
                }
            };
            
            var keysArray = source.Select(x => x.Keys.ToArray()).GroupBy(x => x).FirstOrDefault()!.Key;
            // var formula = ReplaceFinds("sum('total') / sum('test_01') + mean(avg(\"test_01\")) + avg('test_2')", keysArray);
            var formula = ReplaceFinds("1 + 2 * 3", keysArray);
            var specialTokens = new[] { '(', ')', '+', '-', '*', '/', ',' };
            var tokensStringSource = new List<List<char>>()
            {
                new()
            };

            var chArr = formula.Replace(" ", "").ToArray();
            var enumerator = chArr.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentToken = enumerator.Current;
                if (specialTokens.Contains((char)currentToken))
                {
                    tokensStringSource.Add(new List<char>());
                    tokensStringSource.Last().Add((char)currentToken);
                    tokensStringSource.Add(new List<char>());
                }
                else
                {
                    tokensStringSource.Last().Add((char)currentToken);
                }
            }

            var tokenStrings = tokensStringSource.Where(x => x.Any()).ToQueue(x => new QlToken(x.ToArray()));
            var compiler = new ExpertWinnerLangCompiler();
            compiler.Compile(tokenStrings);

            
            while (compiler.Output.TryDequeue(out var token))
            {
            }
        }

        [Fact]
        public unsafe void Test()
        {
        }
        
        public static string ReplaceFinds(string formula, string[] keys)
        {
            var pattern = "([']([a-zA-z0-9]*)['])|([\"]([a-zA-z0-9]*)[\"])";
            var regex = Regex.Match(formula, pattern);
            var hasFinds = regex.Success;
            while (hasFinds)
            {
                if (!regex.Success) continue;
                var formulated = $"find({Array.IndexOf(keys, regex.Value.Replace("'", "").Replace("\"", ""))})";
                formula = Regex.Replace(formula, $"({regex.Value})", formulated);
                regex = regex.NextMatch();
                hasFinds = regex.Success;
            }

            return formula;
        }
    }
}