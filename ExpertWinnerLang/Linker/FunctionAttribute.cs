using System;

namespace ExpertWinnerLang.Linker
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class FunctionAttribute : Attribute
    {
        public FunctionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}