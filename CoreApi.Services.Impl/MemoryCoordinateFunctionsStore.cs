namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // registered as singleton
    public class MemoryCoordinateFunctionsStore : ICoordinateFunctionsStore
    {
        public IList<Func<Variable, Variable, Function>> Get(int count)
        {
            // TODO move this to helper function
            var indexes = Enumerable.Range(1, count)
                .SelectMany(z => Enumerable.Range(0, z + 1)
                    .Select(t => Tuple.Create<int, int>(t, z - t)))
                .ToList();

            return indexes
                .Select(z => this.Get(z.Item1, z.Item2))
                .ToList();
        }

        public Func<Variable, Variable, Function> Get(int xIndex, int yIndex)
        {
            return (Variable x, Variable y) => (Function.Pow(x, xIndex) * Function.Pow(y, yIndex)) 
                / (1 + Function.Pow(y, yIndex * 2));
        }
    }
}
