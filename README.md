# expert-winner
Impelementation on [Shunting-yard algorithm](https://en.wikipedia.org/wiki/Shunting-yard_algorithm) with .Net

This project accepts function strings in infix notation and executes them applying to a dataset (`IEnumerable<IEnumerable<double>>`)

**Usage:**
> Case 1. We have a dataset of `double[][]` and want to get the sum of the first elements in each 'row'
```c#
public void GetSumOfFirstElements(double[][] inputData)
{
    var formula = "(sum(select(0))";
    FunctionsSet.MapAssembly(typeof(Std).Assembly); // register your functions globaly
    var result = ExpertWinner.Execute(inputData, formula);
}
```
It is also available to use some combinated formulas like ```"(sum('total') * 2 + 39) / 4"```

## Default functions list:
Identifier | Meaning
------------ | -------------
min | find the minimum of an `double[]`
max | find the maximum of an `double[]`
std | Standard Deviation for `double[]`
sum | Sum of `double[]`
mean | average of all the observations for `double[]`

You can also add some custom functions by implementing `IFunction` interface and adding `[Function(FunctionName)]` attribute:
```c#
[Function("max")]
public class Max : IFunction
{
    public double Execute(double[] argument)
    {
        if (!argument.Any())
            return double.NaN;

        return argument.Max();
    }
}
```
You can use `select(index)` to query items by their index from a collection or implement `IQuery` interface with `[Query(QueryName)]` attribute to create your own implementation of queries
