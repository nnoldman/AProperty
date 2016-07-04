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

        internal Agent Agent;
        public Prop Property { get; set; }
        public OnValueChange OnValueChanged;
        internal bool mDirty = true;
        internal int mLV;
        internal double mFinalValue;
        internal List<Modifier> mModifiers = new List<Modifier>();

        public List<Modifier> Modifiers
        {
            get { return mModifiers; }
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
            double oldValue = mFinalValue;

            double baseValue = BaseValue;

            double grow = 0;

            foreach (var modifier in mModifiers)
                grow += modifier.GetGrowValue(mLV, baseValue);

            mFinalValue = baseValue + grow;
            mFinalValue = Clamp(mFinalValue, Property.Min, Property.Max);

            if (oldValue != mFinalValue)
                if (OnValueChanged != null)
                    OnValueChanged(oldValue, mFinalValue);
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

        public int LV
        {
            get { return mLV; }
            set { mLV = value; }
        }

        public double BaseValue
        {
            get { return Property.GetValue(mLV); }
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
                return mFinalValue;
            }
        }
    }
}
