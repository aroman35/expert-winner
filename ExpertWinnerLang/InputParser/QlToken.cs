using System;
using System.Collections.Generic;

namespace ExpertWinnerLang.InputParser
{
    public class QlToken
    {
        private static readonly ICollection<char> Operators = new []{ '+', '-', '*', '/' };
        private static readonly ICollection<char> Specials = new []{ '(', ')' };
        public TokenType Type { get; }
        public bool IsHighPriority => Type == TokenType.Operator && Value is "*" or "/";
        public string Value { get; }

        public QlToken(char[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Value = new string(value);
            Type = value.Length switch
            {
                1 when Operators.Contains(value[0]) => TokenType.Operator,
                1 when Specials.Contains(value[0]) => TokenType.Brackets,
                1 when value[0] == ',' => TokenType.Separator,
                _ => int.TryParse(Value, out _) ? TokenType.Number : TokenType.Function
            };
        }

        public override string ToString()
        {
            return $"[{Type.ToString()}] {Value}";
        }
    }
}