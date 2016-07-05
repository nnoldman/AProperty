using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AProperty
{
    public class Entity
    {
        public delegate void OnValueChange(double old, double cur);

        public OnValueChange OnValueChanged;

        public List<Modifier> Modifiers
        {
            get { return mModifiers; }
        }
        public int Index;
        internal Agent Agent;

        public double Max = double.MaxValue;
        public double Min = 0;

        List<Modifier> mModifiers = new List<Modifier>();
        double mBaseValue;
        double mGrowRate;
        double mValue;
        bool mDirty = true;

        public double BaseValue
        {
            get { return mBaseValue; }
            set { mBaseValue = value; }
        }
        public double BaseValueWithLV
        {
            get { return GetValue(Agent.LV); }
        }
        public double GrowRate
        {
            get { return mGrowRate; }
            set { mGrowRate = value; }
        }

        public double GetValue(int lv)
        {
            return mBaseValue + mGrowRate * (lv - 1);
        }
        double Clamp(double v, double min, double max)
        {
            if (v < min)
                v = min;
            if (v > max)
                v = max;
            return v;
        }

        internal void Recaculate()
        {
            double oldValue = mValue;

            double baseValue = BaseValueWithLV;

            double grow = 0;

            foreach (var modifier in mModifiers)
                grow += modifier.GetGrowValue(baseValue);

            mValue = baseValue + grow;
            mValue = Clamp(mValue, Min, Max);

            if (oldValue != mValue)
                if (OnValueChanged != null)
                    OnValueChanged(oldValue, mValue);
        }

        public void AddModifier(Modifier modifer)
        {
            modifer.Owner = this;
            mModifiers.Add(modifer);
            Dirty = true;
        }

        public bool Dirty
        {
            get
            {
                return mDirty;
            }
            internal set
            {
                mDirty = value;
                if (mDirty && this.Agent)
                    this.Agent.Dirty = true;
            }
        }

        public double Value
        {
            get
            {
                if (mDirty)
                {
                    Recaculate();
                    mDirty = false;
                }
                return mValue;
            }
        }
    }
}
