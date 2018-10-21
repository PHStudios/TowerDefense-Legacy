using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerDefenseEngine
{
    [ContentTypeWriter]
    public class TowerStatisticsWriter : ContentTypeWriter<TowerStatistics>
    {
        protected override void Write(ContentWriter output, TowerStatistics value)
        {
            output.Write(value.Health);
            output.Write(value.Damage);
            output.Write(value.Speed);
            output.Write(value.Accuracy);
            output.Write(value.CriticalChance);
            output.Write(value.CriticalHitScalar);
            output.Write(value.Radius);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return typeof(TowerStatistics.TowerStatisticsReader).AssemblyQualifiedName;
        }
    }

    public class TowerStatistics
    {
        public int Health
        {
            get;
            set;
        }
        public int Damage
        {
            get;
            set;
        }
        public float Speed
        {
            get;
            set;
        }
        public float Accuracy
        {
            get;
            set;
        }
        public float CriticalChance
        {
            get;
            set;
        }
        public float CriticalHitScalar
        {
            get;
            set;
        }
        public int Radius
        {
            get;
            set;
        }

        #region Add, Subtract, and Multiply
        public static TowerStatistics Add(TowerStatistics a, TowerStatistics b)
        {
            TowerStatistics result = new TowerStatistics();

            result.Health = a.Health + b.Health;
            result.Damage = a.Damage + b.Damage;
            result.Speed = a.Speed + b.Speed;
            result.Accuracy = a.Accuracy + b.Accuracy;
            result.CriticalChance = a.CriticalChance + b.CriticalChance;
            result.CriticalHitScalar = a.CriticalHitScalar + b.CriticalHitScalar;
            result.Radius = a.Radius + b.Radius;

            return result;
        }

        public static TowerStatistics Add(TowerStatistics a, UpgradeStatistics b)
        {
            TowerStatistics result = new TowerStatistics();

            result.Health = a.Health + b.HealthIncrease;
            result.Damage = a.Damage + b.DamageIncrease;
            result.Speed = a.Speed + b.SpeedIncrease;
            result.Accuracy = a.Accuracy + b.AccuracyIncrease;
            result.CriticalChance = a.CriticalChance + b.CriticalChanceIncrease;
            result.CriticalHitScalar = a.CriticalHitScalar + b.CriticalHitScalarIncrease;
            result.Radius = a.Radius + b.RadiusIncrease;

            return result;
        }

        public static TowerStatistics Subtract(TowerStatistics a, TowerStatistics b)
        {
            TowerStatistics result = new TowerStatistics();

            result.Health = a.Health - b.Health;
            result.Damage = a.Damage - b.Damage;
            result.Speed = a.Speed - b.Speed;
            result.Accuracy = a.Accuracy - b.Accuracy;
            result.CriticalChance = a.CriticalChance - b.CriticalChance;
            result.CriticalHitScalar = a.CriticalHitScalar - b.CriticalHitScalar;
            result.Radius = a.Radius - b.Radius;

            return result;
        }

        public static TowerStatistics Multiply(TowerStatistics a, float b)
        {
            TowerStatistics result = new TowerStatistics();

            result.Health = (int)(a.Health * b);
            result.Damage = (int)(a.Damage * b);
            result.Speed = a.Speed * b;
            result.Accuracy = a.Accuracy * b;
            result.CriticalChance = a.CriticalChance * b;
            result.CriticalHitScalar = a.CriticalHitScalar * b;
            result.Radius = (int)(a.Radius * b);

            return result;
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("[H: {0}", Health));
            sb.Append(string.Format("; D: {0}", Damage));
            sb.Append(string.Format("; FR: {0}", Speed));
            sb.AppendLine(string.Format("; A: {0}]", Accuracy));
            sb.Append(string.Format("[CC: {0}", CriticalChance));
            sb.Append(string.Format("; CH: {0}", CriticalHitScalar));
            sb.Append(string.Format("; R: {0}]", Radius));

            return sb.ToString();
        }

        public string ToShortString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("Stats: [H: {0}", Health));
            sb.Append(string.Format("; D: {0}", Damage));
            sb.Append(string.Format("; FR: {0}]", Speed));
            
            return sb.ToString();
        }

        public class TowerStatisticsReader : ContentTypeReader<TowerStatistics>
        {
            protected override TowerStatistics Read(ContentReader input, TowerStatistics existingInstance)
            {
                TowerStatistics result = new TowerStatistics();

                result.Health = input.ReadInt32();
                result.Damage = input.ReadInt32();
                result.Speed = input.ReadSingle();
                result.Accuracy = input.ReadSingle();
                result.CriticalChance = input.ReadSingle();
                result.CriticalHitScalar = input.ReadSingle();
                result.Radius = input.ReadInt32();

                return result;
            }
        }
    }
}
