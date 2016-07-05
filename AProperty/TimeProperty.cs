using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AProperty
{
    public enum TimeType
    {
        Forever,
        Buffer,
        Once,
    }
    public class TimeProperty
    {
        public double Value = 2000;
        public TimeType Type;
        internal Action OnTimer;

        internal enum LifeState
        {
            Alive,
            Die,
        }

        internal LifeState Life
        {
            get { return mLife; }
        }

        internal double mElapseTime = 0;

        internal LifeState mLife = LifeState.Alive;

        internal bool AdvanceTime(double deltaTimeMS)
        {
            if (mLife == LifeState.Alive && this.Type == TimeType.Buffer)
            {
                int lastSecond = (int)(mElapseTime * 0.001);
                
                mElapseTime += deltaTimeMS;
                
                int curSecond = (int)(mElapseTime * 0.001);

                if (curSecond > lastSecond && OnTimer != null)
                    OnTimer();

                if (mElapseTime > Value)
                {
                    mLife = LifeState.Die;
                    return true;
                }
            }
            return false;
        }
    }
}
