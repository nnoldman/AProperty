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
        public int LV;

        internal abstract double GetGrowValue(double baseValue);

        internal void AdvanceTime(double deltaTimeMS)
        {
            if (this.Timer.AdvanceTime(deltaTimeMS))
                this.Owner.Dirty = true;
        }
    }

    public class FixedModifier : Modifier
    {
        internal override double GetGrowValue( double baseValue)
        {
            return BaseValue + GrowRate * (this.LV - 1);
        }
    }

    public class PercentModifier : Modifier
    {
        internal override double GetGrowValue(double baseValue)
        {
            return baseValue * (BaseValue + GrowRate * (this.LV - 1)) * 0.01;
        }
    }

    public class PulseFixedModifer : Modifier
    {
        public PulseFixedModifer()
        {
            this.Timer.OnTimer = OnTimer;
        }
        internal override double GetGrowValue(double baseValue)
        {
            return 0;
        }

        internal void OnTimer()
        {
            FixedModifier modifier = new FixedModifier();
            modifier.BaseValue = this.BaseValue;
            modifier.GrowRate = this.GrowRate;
            modifier.TargetPropertyIndex = this.TargetPropertyIndex;
            modifier.SourceIndex = this.SourceIndex;
            modifier.Timer.Type = TimeType.Once;
            Owner.AddModifier(modifier);
        }
    }
}
