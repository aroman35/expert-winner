using System;
using System.Collections.Generic;
using ExpertWinnerLang.Compiler;
using ExpertWinnerLang.Exceptions;
using ExpertWinnerLang.InputParser;
using ExpertWinnerLang.Linker;

namespace ExpertWinnerLang.Runtime
{
    // https://en.wikipedia.org/wiki/Reverse_Polish_notation
    public class ExpertWinnerLangRuntime
    {
        private readonly ExpertWinnerLangCompiler _compiler;

        public ExpertWinnerLangRuntime(ExpertWinnerLangCompiler compiler)
        {
            _compiler = compiler;
        }

        public double Execute(IEnumerable<IEnumerable<double>> sequences, string formula)
        {
            _compiler.Compile(formula);
            
            var runtimeStack = new Stack<double[]>();
            while (_compiler.Output.TryDequeue(out var token))
            {
                if (token.Type == TokenType.Number && double.TryParse(token.Value, out var number))
                    runtimeStack.Push(new[]{number});
                if (token.Type == TokenType.Operator &&
                    runtimeStack.TryPop(out var rightNumber) &&
                    runtimeStack.TryPop(out var leftNumber)
                )
                    runtimeStack.Push(GetOperator(token)(leftNumber, rightNumber));
                if (token.Type == TokenType.Function && runtimeStack.TryPop(out var argument))
                {
                    if (FunctionsSet.QueriesMap.TryGetValue(token.Value, out var query))
                        runtimeStack.Push(query.Execute(sequences, (int)argument[0]));
                    else if (FunctionsSet.FunctionsMap.TryGetValue(token.Value, out var function))
                        runtimeStack.Push(new[] { function.Execute(argument) });
                    else
                        throw new FunctionNotFoundException(token.Value);
                }
            }
            
            return runtimeStack.Pop()[0];
        }

        private Func<double[], double[], double[]> GetOperator(QlToken token)
        {
            switch (token.Value)
            {
                case "+": return (a, b) => new []{a[0] + b[0]};
                case "-": return (a, b) => new []{a[0] - b[0]};
                case "*": return (a, b) => new []{a[0] * b[0]};
                case "/": return (a, b) => new []{a[0] / b[0]};
                default: throw new Exception($"Unknown operator {token.Value}");
            }
        }
    }
}