using System;

namespace ExpertWinnerLang.Linker.Attributes
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