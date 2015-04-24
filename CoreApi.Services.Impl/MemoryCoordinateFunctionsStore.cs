namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // registered as singleton
    public class MemoryCoordinateFunctionsStore : ICoordinateFunctionsStore
    {
        public IList<Function> Get(int count, Variable x, Variable y)
        {
            // TODO move this to helper function
            var indexes = Enumerable.Range(0, (count * (count + 1)) / 2)
                .SelectMany(z => Enumerable.Range(0, z + 1)
                    .Select(t => Tuple.Create<int, int>(t, z - t)))
                .ToList();

            return indexes
                .Select(z => this.Get(z.Item1, z.Item2, x, y))
                .ToList();
        }

        public Function Get(int xIndex, int yIndex, Variable x, Variable y)
        {
            return (Function.Pow(x, xIndex) * Function.Pow(y, yIndex)) 
                / (1 + Function.Pow(y, yIndex * 2));
        }
    }
}
