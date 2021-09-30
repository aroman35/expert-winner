using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Compile(Queue<QlToken> tokens)
        {
            while (tokens.TryDequeue(out var token))
            {
                if (token.Type == TokenType.Number)
                {
                    Output.Enqueue(token);
                    continue;
                }

                if (token.Type == TokenType.Function)
                {
                    _callStack.Push(token);
                    continue;
                }

                if (token.Type == TokenType.Operator)
                {
                    while (_callStack.TryPeek(out var opPriToken) && opPriToken.Type == TokenType.Operator && opPriToken.IsHighPriority)
                    {
                        Output.Enqueue(_callStack.Pop());
                    }
                    _callStack.Push(token);
                    continue;
                }

                if (token.Type == TokenType.Special && token.Value == "(")
                {
                    _callStack.Push(token);
                    continue;
                }

                if (token.Type == TokenType.Special && token.Value == ")")
                {
                    while (_callStack.TryPeek(out var csToken) && csToken.Value != "(")
                    {
                        Output.Enqueue(_callStack.Pop());
                    }

                    if (!_callStack.Any())
                        throw new Exception("\"(\" is missing");
                    var _ = _callStack.Pop();
                    if (_callStack.Peek().Type == TokenType.Function)
                        Output.Enqueue(_callStack.Pop());
                }
            }

            while (_callStack.TryPop(out var existingToken))
            {
                if (existingToken.Value == "(")
                    throw new Exception("\")\" is missing");
                Output.Enqueue(existingToken);
            }
        }
    }
}