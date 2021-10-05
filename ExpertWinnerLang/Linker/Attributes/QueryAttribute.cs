using System;

namespace ExpertWinnerLang.Linker.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class QueryAttribute : Attribute
    {
        public QueryAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}