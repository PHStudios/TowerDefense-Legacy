using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace TowerDefenseEngine
{
    public class MonsterStatistics
    {
        public int Health
        {
            get;
            protected set;
        }

        public float Speed
        {
            get;
            protected set;
        }

        #region Add, Subtract, and Multiply
        public static MonsterStatistics Add(MonsterStatistics a, MonsterStatistics b)
        {
            MonsterStatistics result = new MonsterStatistics();

            result.Health = a.Health + b.Health;
            result.Speed = a.Speed + b.Speed;

            return result;
        }

        public static MonsterStatistics Subtract(MonsterStatistics a, MonsterStatistics b)
        {
            MonsterStatistics result = new MonsterStatistics();

            result.Health = a.Health - b.Health;
            result.Speed = a.Speed - b.Speed;

            return result;
        }

        public static MonsterStatistics Multiply(MonsterStatistics a, float b)
        {
            MonsterStatistics result = new MonsterStatistics();

            result.Health = (int)(a.Health * b);
            result.Speed = a.Speed * b;

            return result;
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("Health: {0}", Health));
            sb.Append(string.Format("|Speed: {0}", Speed));

            return sb.ToString();
        }

        public class MonsterStatisticsReader : ContentTypeReader<MonsterStatistics>
        {
            protected override MonsterStatistics Read(ContentReader input, MonsterStatistics existingInstance)
            {
                MonsterStatistics result = new MonsterStatistics();

                result.Health = input.ReadInt32();
                result.Speed = input.ReadSingle();

                return result;
            }
        }
    }
}
