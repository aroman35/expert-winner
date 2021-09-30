using System.Net.Sockets;

namespace ExpertWinnerLang.Linker
{
    public interface IFunction<TArgument, TReturn> : IFunction
    {
        TReturn Execute(params TArgument[] argument);
    }

    public interface IFunction
    {
    }
}