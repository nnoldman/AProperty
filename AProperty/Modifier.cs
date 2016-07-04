using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AProperty
{
    public abstract class Modifier
    {
        internal Entity Owner;

        public TimeProperty Timer = new TimeProperty();
        public int SourceIndex;
        public double GrowRate;
        public double BaseValue;
        public int TargetPropertyIndex;

        internal abstract double GetGrowValue(int lv, double baseValue);
    }

    public class FixedModifier : Modifier
    {
        internal override double GetGrowValue(int lv, double baseValue)
        {
            return BaseValue + GrowRate * (lv - 1);
        }
    }

    public class PercentModifier : Modifier
    {
        internal override double GetGrowValue(int lv, double baseValue)
        {
            return baseValue * (BaseValue + GrowRate * (lv - 1)) * 0.01;
        }
    }

    public class PulseFixedModifer : Modifier
    {
        internal PulseFixedModifer()
        {
            this.Timer.OnTimer = OnTimer;
        }
        internal override double GetGrowValue(int lv, double baseValue)
        {
            return 0;
        }

        internal void OnTimer()
        {
            FixedModifier modifier = new FixedModifier();
            modifier.Timer.Type = TimeType.Once;
            Owner.AddModifier(modifier);
        }
    }
}
