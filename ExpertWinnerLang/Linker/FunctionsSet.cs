using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExpertWinnerLang.Extensions;
using ExpertWinnerLang.Linker.Abstractions;
using ExpertWinnerLang.Linker.Attributes;

namespace ExpertWinnerLang.Linker
{
    public static class FunctionsSet
    {
        public static Dictionary<string, IFunction> FunctionsMap { get; private set; }
        public static Dictionary<string, IQuery> QueriesMap { get; private set; }

        static FunctionsSet()
        {
            FunctionsMap = new Dictionary<string, IFunction>();
            QueriesMap = new Dictionary<string, IQuery>();
        }

        public static bool MapAssembly(params Assembly[] assemblies)
        {
            var functionTypes = assemblies
                .SelectMany(assembly =>
                    GetFunctionsFromAssembly(assembly)
                        .Concat(GetQueriesFromAssembly(assembly)))
                .ToArray();

            var result = true;
            foreach (var functionType in functionTypes)
            {
                if (typeof(IFunction).IsAssignableFrom(functionType))
                    result = result && FunctionsMap.TryAdd(
                        functionType.GetCustomAttribute<FunctionAttribute>()!.Name,
                        (IFunction)Activator.CreateInstance(functionType)!);
                
                if (typeof(IQuery).IsAssignableFrom(functionType))
                    result = result && QueriesMap.TryAdd(
                        functionType.GetCustomAttribute<QueryAttribute>()!.Name,
                        (IQuery)Activator.CreateInstance(functionType)!);
            }

            return result;
        }

        private static IEnumerable<Type> GetFunctionsFromAssembly(Assembly assembly)
        {
            var funcType = typeof(IFunction);
            return assembly
                .GetTypes()
                .Where(type =>
                    funcType.IsAssignableFrom(type) &&
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any(ctor => !ctor.GetParameters().Any()) &&
                    type.GetCustomAttributes(typeof(FunctionAttribute)).Any())
                .DistinctBy(type => type.GetCustomAttribute<FunctionAttribute>()!.Name);
        }
        
        private static IEnumerable<Type> GetQueriesFromAssembly(Assembly assembly)
        {
            var funcType = typeof(IQuery);
            return assembly
                .GetTypes()
                .Where(type =>
                    funcType.IsAssignableFrom(type) &&
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any(ctor => !ctor.GetParameters().Any()) &&
                    type.GetCustomAttributes(typeof(QueryAttribute)).Any())
                .DistinctBy(type => type.GetCustomAttribute<QueryAttribute>()!.Name);
        }
    }
}