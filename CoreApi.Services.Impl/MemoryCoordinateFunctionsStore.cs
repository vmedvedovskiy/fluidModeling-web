namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Collections.Generic;

    public class MemoryCoordinateFunctionsStore : ICoordinateFunctionsStore
    {
        public IList<Function> Get(int count, Variable x, Variable y)
        {
            throw new NotImplementedException();
        }

        public Function Get(int xIndex, int yIndex, Variable x, Variable y)
        {
            return Function.Pow(x, xIndex) * Function.Pow(y, yIndex);
        }
    }
}
