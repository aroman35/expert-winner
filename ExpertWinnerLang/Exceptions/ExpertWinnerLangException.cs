using System;

namespace ExpertWinnerLang.Exceptions
{
    public abstract class ExpertWinnerLangException : Exception
    {
        protected ExpertWinnerLangException(string message) : base(message)
        {
            
        }
    }
}