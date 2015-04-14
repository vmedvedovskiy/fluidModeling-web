namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System.Collections.Generic;

    public interface ICoordinateFunctionsStore
    {
        IList<Function> Get(int count, Variable x, Variable y);

        Function Get(int xIndex, int yIndex, Variable x, Variable y);
    }
}
