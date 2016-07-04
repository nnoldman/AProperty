using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AProperty
{
    public class Prop
    {
        public int Index;

        public double Max = double.MaxValue;
        public double Min = double.MinValue;

        public double BaseValue
        {
            get { return mBaseValue; }
            set { mBaseValue = value; }
        }

        public double GrowRate
        {
            get { return mGrowRate; }
            set { mGrowRate = value; }
        }

        double mBaseValue;
        double mGrowRate;

        public double GetValue(int lv)
        {
            return mBaseValue + mGrowRate * (lv - 1);
        }
    }
}
