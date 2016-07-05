using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AProperty;

namespace UnitTestProject1
{
    /// <summary>
    /// 技能效果(攻击-防御-生命-武力-智力-命中-闪避-暴击-韧性-移动速度-生命回复-单次生命回复-技能命中-伤害系数-真实伤害)共14个
    /// </summary>
    public enum PropertyType
    {
        Attack,
        Defence,
        HpMax,
        Strength,
        Intellect,
        Hit,
        Dodge,
        Bang,
        Tenacity,
        Speed,
        HpRate,
        HpRateSingle,
        SkillHit,
        DamageFactor,
        RealDamage,
        Captain,
        AttackDistance,
        DamageCount,
        Count,
    }
    public enum PropertySource
    {
        Invalid,
        Base,
        SkillBuffer,
        Equip,
    }
    public enum PropertyTimeType
    {
        Invalid,
        Buffer,
        Once,
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestProp()
        {
            Entity prop = new Entity();
            prop.BaseValue = 50;
            prop.GrowRate = 5;

            Assert.AreEqual(prop.GetValue(1), 50 + (1 - 1) * 5);
            Assert.AreEqual(prop.GetValue(2), 50 + (2 - 1) * 5);
            Assert.AreEqual(prop.GetValue(100), 50 + (100 - 1) * 5);
        }

        [TestMethod]
        public void TestEntity()
        {
            Entity prop1 = new Entity();
            prop1.BaseValue = 50;
            prop1.GrowRate = 5;
            prop1.Index = (int)PropertyType.Attack;


            Modifier m1 = new FixedModifier();
            m1.TargetPropertyIndex = (int)PropertyType.Attack;
            m1.SourceIndex = (int)PropertySource.Equip;
            m1.BaseValue = 10;
            m1.GrowRate = 1;
            m1.Timer.Type = TimeType.Forever;
            m1.LV = 5;

            prop1.AddModifier(m1);

            AProperty.Agent agent = new AProperty.Agent();
            agent.AddEntity(prop1);

            Assert.AreEqual(prop1.Value, prop1.BaseValue + m1.BaseValue + (m1.LV - 1) * m1.GrowRate);
        }

        [TestMethod]
        public void TestPulseFixedModifer()
        {
            Entity e1 = new Entity();
            e1.BaseValue = 50;
            e1.GrowRate = 5;
            e1.Index = (int)PropertyType.Attack;


            PulseFixedModifer m1 = new PulseFixedModifer();
            m1.TargetPropertyIndex = (int)PropertyType.Attack;
            m1.SourceIndex = (int)PropertySource.SkillBuffer;
            m1.BaseValue = 10;
            m1.GrowRate = 1;
            m1.LV = 3;
            m1.Timer.Type = TimeType.Buffer;
            m1.Timer.Value = 2000;

            e1.AddModifier(m1);

            AProperty.Agent agent = new AProperty.Agent();
            agent.LV = 4;
            agent.AddEntity(e1);

            agent.Update(500);

            double basevalue = 50 + (4 - 1) * 5;
            double modifierBaseValue = 10 + (3 - 1) * 1;

            //agent.Update(500);
            //agent.Update(500);
            //agent.Update(500);
            //agent.Update(500);
            //Assert.AreEqual(e1.Value, basevalue);
            //Assert.AreEqual(e1.Value, basevalue);
            //agent.Update(500);
            //Assert.AreEqual(e1.Value, basevalue);
            //agent.Update(500);
            //Assert.AreEqual(e1.Value, basevalue);
        }
    }
}
