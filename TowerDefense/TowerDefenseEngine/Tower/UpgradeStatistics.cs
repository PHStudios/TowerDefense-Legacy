using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerDefenseEngine
{
    [ContentTypeWriter]
    public class UpgradeStatisticsWriter : ContentTypeWriter<UpgradeStatistics>
    {
        protected override void Write(ContentWriter output, UpgradeStatistics value)
        {
            output.Write(value.HealthIncrease);
            output.Write(value.DamageIncrease);
            output.Write(value.SpeedIncrease);
            output.Write(value.AccuracyIncrease);
            output.Write(value.CriticalChanceIncrease);
            output.Write(value.CriticalHitScalarIncrease);
            output.Write(value.RadiusIncrease);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return typeof(UpgradeStatistics.UpgradeStatisticsReader).AssemblyQualifiedName;
        }
    }

    public class UpgradeStatistics
    {
        [ContentSerializer(Optional = true)]
        public int HealthIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public int CostIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public int DamageIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public float SpeedIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public float AccuracyIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public float CriticalChanceIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public float CriticalHitScalarIncrease
        {
            get;
            set;
        }
        [ContentSerializer(Optional = true)]
        public int RadiusIncrease
        {
            get;
            set;
        }

        #region Add, Subtract, and Multiply
        public static UpgradeStatistics Add(UpgradeStatistics a, UpgradeStatistics b)
        {
            UpgradeStatistics result = new UpgradeStatistics();

            result.HealthIncrease = a.HealthIncrease + b.HealthIncrease;
            result.DamageIncrease = a.DamageIncrease + b.DamageIncrease;
            result.SpeedIncrease = a.SpeedIncrease + b.SpeedIncrease;
            result.AccuracyIncrease = a.AccuracyIncrease + b.AccuracyIncrease;
            result.CriticalChanceIncrease = a.CriticalChanceIncrease + b.CriticalChanceIncrease;
            result.CriticalHitScalarIncrease = a.CriticalHitScalarIncrease + b.CriticalHitScalarIncrease;
            result.RadiusIncrease = a.RadiusIncrease + b.RadiusIncrease;

            return result;
        }

        public static UpgradeStatistics Subtract(UpgradeStatistics a, UpgradeStatistics b)
        {
            UpgradeStatistics result = new UpgradeStatistics();

            result.HealthIncrease = a.HealthIncrease - b.HealthIncrease;
            result.DamageIncrease = a.DamageIncrease - b.DamageIncrease;
            result.SpeedIncrease = a.SpeedIncrease - b.SpeedIncrease;
            result.AccuracyIncrease = a.AccuracyIncrease - b.AccuracyIncrease;
            result.CriticalChanceIncrease = a.CriticalChanceIncrease - b.CriticalChanceIncrease;
            result.CriticalHitScalarIncrease = a.CriticalHitScalarIncrease - b.CriticalHitScalarIncrease;
            result.RadiusIncrease = a.RadiusIncrease - b.RadiusIncrease;

            return result;
        }

        public static UpgradeStatistics Multiply(UpgradeStatistics a, float b)
        {
            UpgradeStatistics result = new UpgradeStatistics();

            result.HealthIncrease = (int)(a.HealthIncrease * b);
            result.DamageIncrease = (int)(a.DamageIncrease * b);
            result.SpeedIncrease = a.SpeedIncrease * b;
            result.AccuracyIncrease = a.AccuracyIncrease * b;
            result.CriticalChanceIncrease = a.CriticalChanceIncrease * b;
            result.CriticalHitScalarIncrease = a.CriticalHitScalarIncrease * b;
            result.RadiusIncrease = (int)(a.RadiusIncrease * b);

            return result;
        }
        #endregion

        public class UpgradeStatisticsReader : ContentTypeReader<UpgradeStatistics>
        {
            protected override UpgradeStatistics Read(ContentReader input, UpgradeStatistics existingInstance)
            {
                UpgradeStatistics result = new UpgradeStatistics();

                result.HealthIncrease = input.ReadInt32();
                result.DamageIncrease = input.ReadInt32();
                result.SpeedIncrease = input.ReadSingle();
                result.AccuracyIncrease = input.ReadSingle();
                result.CriticalChanceIncrease = input.ReadSingle();
                result.CriticalHitScalarIncrease = input.ReadSingle();
                result.RadiusIncrease = input.ReadInt32();

                return result;
            }
        }

    }
}
