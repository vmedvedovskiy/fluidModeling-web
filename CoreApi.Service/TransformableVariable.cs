namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System;

    public class TransformableVariable : Variable
    {
        private int order;

        public TransformableVariable(int order)
        {
            this.order = order;
            this.Rule = GetDefaultRule();
        }

        public TransformableVariable(int order, Func<double, double, double> rule)
            : this(order)
        {
            if (rule == null)
            {
                this.Rule = GetDefaultRule();
            }
            else
            {
                this.Rule = rule;
            }
        }

        public Func<double, double, double> Rule { get; private set; }

        private Func<double, double, double> GetDefaultRule()
        {
            Func<double, double, double> defaultRule = (x, y) =>
            {
                if (order == 0) return x;
                else if (order == 1) return y;
                else return 0d;
            };

            return defaultRule;
        }
    }
}
