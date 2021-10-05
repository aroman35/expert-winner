using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExpertWinnerLang.Exceptions;
using ExpertWinnerLang.Extensions;
using ExpertWinnerLang.InputParser;

namespace ExpertWinnerLang.Compiler
{
    //https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D1%81%D0%BE%D1%80%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%BE%D1%87%D0%BD%D0%BE%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D1%86%D0%B8%D0%B8
    public class ExpertWinnerLangCompiler
    {
        private readonly Stack<QlToken> _callStack;
        public Queue<QlToken> Output { get; }

        public ExpertWinnerLangCompiler()
        {
            _callStack = new Stack<QlToken>();
            Output = new Queue<QlToken>();
        }

        internal void Compile(string inputFormula)
        {
            _callStack.Clear();
            Output.Clear();

            var specialTokens = new[] { '(', ')', '+', '-', '*', '/', ',' };
            var tokensStringSource = new List<LinkedList<char>>
            {
                new()
            };

            var chArr = inputFormula.Replace(" ", "").ToArray();
            var enumerator = chArr.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentToken = enumerator.Current;
                if (specialTokens.Contains((char)currentToken))
                {
                    tokensStringSource.Add(new LinkedList<char>());
                    tokensStringSource.Last().AddLast((char)currentToken);
                    tokensStringSource.Add(new LinkedList<char>());
                    continue;
                }

                tokensStringSource.Last().AddLast((char)currentToken);
            }

            var tokens = tokensStringSource.Where(x => x.Any()).ToQueue(x => new QlToken(x.ToArray()));

            CompileTokens(tokens);
        }

        private void CompileTokens(Queue<QlToken> tokens)
        {
            while (tokens.TryDequeue(out var token))
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        Output.Enqueue(token);
                        continue;
                    case TokenType.Function:
                        _callStack.Push(token);
                        continue;
                    case TokenType.Operator:
                    {
                        while (_callStack.TryPeek(out var opPriToken) && opPriToken.Type == TokenType.Operator && opPriToken.IsHighPriority)
                            Output.Enqueue(_callStack.Pop());
                        _callStack.Push(token);
                        continue;
                    }
                    case TokenType.Special when token.Value == "(":
                        _callStack.Push(token);
                        continue;
                    case TokenType.Special when token.Value == ")":
                    {
                        while (_callStack.TryPeek(out var csToken) && csToken.Value != "(")
                            Output.Enqueue(_callStack.Pop());

                        if (!_callStack.Any())
                            throw new Exception("\"(\" is missing");
                        var _ = _callStack.Pop();
                        if (_callStack.TryPeek(out var funcToken) && funcToken.Type == TokenType.Function)
                            Output.Enqueue(_callStack.Pop());
                        break;
                    }
                    case TokenType.NaN:
                        throw new CompilationException($"Invalid token {token}");
                    default:
                        throw new CompilationException($"Invalid token {token}");
                }
            }

            while (_callStack.TryPop(out var existingToken))
            {
                if (existingToken.Value == "(")
                    throw new Exception("\")\" is missing");
                Output.Enqueue(existingToken);
            }
        }

        // TODO: move to application
        private static string ReplaceFinds(string formula, string[] keys)
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