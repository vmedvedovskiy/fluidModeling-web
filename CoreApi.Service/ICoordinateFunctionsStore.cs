namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System;
    using System.Collections.Generic;

    public interface ICoordinateFunctionsStore
    {
        IList<Func<Variable, Variable, Function>> Get(int count);

        Func<Variable, Variable, Function> Get(int xIndex, int yIndex);
    }
}
