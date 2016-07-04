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
            Prop prop = new Prop();
            prop.BaseValue = 50;
            prop.GrowRate = 5;

            Assert.AreEqual(prop.GetValue(1), 50 + (1 - 1) * 5);
            Assert.AreEqual(prop.GetValue(2), 50 + (2 - 1) * 5);
            Assert.AreEqual(prop.GetValue(100), 50 + (100 - 1) * 5);
        }

        [TestMethod]
        public void TestEntity()
        {
            Prop prop1 = new Prop();
            prop1.BaseValue = 50;
            prop1.GrowRate = 5;
            prop1.Index = (int)PropertyType.Attack;

            Entity e = new Entity();
            e.LV = 5;
            e.Property = prop1;

            Modifier m1 = new FixedModifier();
            m1.TargetPropertyIndex = (int)PropertyType.Attack;
            m1.SourceIndex = (int)PropertySource.Equip;
            m1.BaseValue = 10;
            m1.GrowRate = 1;
            m1.Timer.Type = TimeType.Forever;

            e.AddModifier(m1);

            Assert.AreEqual(e.Value, e.BaseValue + m1.BaseValue + (e.LV - 1) * m1.GrowRate);
        }

        [TestMethod]
        public void TestAgent()
        {
            Prop prop1 = new Prop();
            prop1.BaseValue = 50;
            prop1.GrowRate = 5;
            prop1.Index = (int)PropertyType.Attack;

            Entity e1 = new Entity();
            e1.LV = 5;
            e1.Property = prop1;

            Prop prop2 = new Prop();
            prop2.BaseValue = 50;
            prop2.GrowRate = 5;
            prop2.Index = (int)PropertyType.Defence;

            Entity e2 = new Entity();
            e2.LV = 10;
            e2.Property = prop2;

            Prop prop3 = new Prop();
            prop3.BaseValue = 50;
            prop3.GrowRate = 5;
            prop3.Index = (int)PropertyType.HpMax;

            Entity e3 = new Entity();
            e3.LV = 15;
            e3.Property = prop3;

            AProperty.Agent agent = new AProperty.Agent();
            agent.AddEntity(e1);
            agent.AddEntity(e2);
            agent.AddEntity(e3);
        }
    }
}
