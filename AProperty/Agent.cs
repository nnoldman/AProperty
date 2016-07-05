using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AProperty
{
    public class Agent
    {
        internal int mLV = 1;

        public int LV
        {
            get { return mLV; }
            set { mLV = value; }
        }

        public static implicit operator bool(Agent agent)
        {
            return agent != null;
        }
        public List<Entity> Propertices
        {
            get { return mPropertices; }
        }

        internal List<Entity> mPropertices = new List<Entity>();

        public bool Dirty
        {
            get { return mDirty; }
            set { mDirty = value; }
        }

        internal bool mDirty = false;

        public void AddEntity(Entity e)
        {
            Debug.Assert(mPropertices.Find((item) => item.Index == e.Index) == null);
            e.Agent = this;
            mPropertices.Add(e);
        }


        public void AddModifier(Modifier modifier)
        {
            Entity e = this.mPropertices.Find(item => item.Index == modifier.TargetPropertyIndex);
            e.AddModifier(modifier);
            this.Dirty = true;
        }

        static List<Modifier> mCacheGarbages = new List<Modifier>();

        public void Update(double deltaTimeMS)
        {
            Advance(deltaTimeMS);

            if (Dirty)
                Recaculate();
        }

        void Recaculate()
        {
            foreach (var entity in mPropertices)
            {
                mCacheGarbages.Clear();

                foreach (var m in entity.Modifiers)
                {
                    if (m.Timer.Life == TimeProperty.LifeState.Die)
                        mCacheGarbages.Add(m);
                }
                mCacheGarbages.ForEach((item) => entity.Modifiers.Remove(item));
                entity.Recaculate();
            }
            Dirty = false;
        }

        internal void Advance(double deltaTimeMS)
        {
            foreach (var entity in mPropertices)
            {
                foreach (var m in entity.Modifiers)
                {
                    m.AdvanceTime(deltaTimeMS);
                }
            }
        }
    }
}
