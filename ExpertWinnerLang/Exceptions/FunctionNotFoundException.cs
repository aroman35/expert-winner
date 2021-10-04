namespace ExpertWinnerLang.Exceptions
{
    public class FunctionNotFoundException : ExpertWinnerLangException
    {
        public string FunctionName { get; }

        public FunctionNotFoundException(string functionName) : base($"Function \"{functionName}\" was not found")
        {
            FunctionName = functionName;
        }
    }
}