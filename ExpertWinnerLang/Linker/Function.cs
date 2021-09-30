namespace ExpertWinnerLang.Linker
{
    public abstract class Function<TArgument, TReturn> : IFunction<TArgument, TReturn>
    {
        public abstract TReturn Execute(params TArgument[] argument);
    }
}