using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExpertWinnerLang.Extensions;

namespace ExpertWinnerLang.Linker
{
    public static class FunctionsSet
    {
        public static Dictionary<string, IFunction> FunctionsMap { get; private set; }

        static FunctionsSet()
        {
            FunctionsMap = new Dictionary<string, IFunction>();
            MapAssembly(AppDomain.CurrentDomain.GetAssemblies().ToArray());
        }

        public static void MapAssembly(params Assembly[] assemblies)
        {
            var functionTypes = assemblies
                .SelectMany(GetFunctionsFromAssembly)
                .DistinctBy(type => type.GetCustomAttribute<FunctionAttribute>()!.Name);

            foreach (var functionType in functionTypes)
            {
                FunctionsMap.TryAdd(
                    functionType.GetCustomAttribute<FunctionAttribute>()!.Name,
                    (IFunction)Activator.CreateInstance(functionType)!);
            }
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
    }
}