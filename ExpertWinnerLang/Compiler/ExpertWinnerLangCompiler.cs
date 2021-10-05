using System.Collections.Generic;
using System.Linq;
using ExpertWinnerLang.Exceptions;
using ExpertWinnerLang.Extensions;
using ExpertWinnerLang.InputParser;

namespace ExpertWinnerLang.Compiler
{
    // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
    internal class ExpertWinnerLangCompiler
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
                    case TokenType.Separator:
                    {
                        throw new CompilationException("Argument separators are not supported");
                        while (_callStack.TryPeek(out var specialToken) && specialToken.Value != "(")
                            Output.Enqueue(_callStack.Pop());
                        
                        continue;
                    }
                    case TokenType.Brackets when token.Value == "(":
                    {
                        Output.Enqueue(new QlToken("sep".ToCharArray()));
                        _callStack.Push(token);
                        continue;
                    }
                    case TokenType.Brackets when token.Value == ")":
                    {
                        while (_callStack.TryPeek(out var csToken) && csToken.Value != "(")
                            Output.Enqueue(_callStack.Pop());
                        
                        if (!_callStack.Any())
                            throw new CompilationException("\"(\" is missing");
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
                    throw new CompilationException("\")\" is missing");
                Output.Enqueue(existingToken);
            }
        }
    }
}