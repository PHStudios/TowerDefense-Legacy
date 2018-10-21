using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerDefenseEngine
{
    public class Wave
    {
        [ContentTypeWriter]
        public class WaveWriter : ContentTypeWriter<Wave>
        {
            protected override void Write(ContentWriter output, Wave value)
            {
                output.WriteObject(value.MonsterAssets);
                output.Write(value.Title);
                output.Write(value.BossWave);
                output.Write(value.MonsterSpawnTimer);
                output.Write(value.MoneyPerKill);
                output.Write(value.BossMoneyScalar);
            }

            public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
            {
                return typeof(Wave.WaveReader).AssemblyQualifiedName;
            }
        }

        [ContentSerializer]
        private List<string> MonsterAssets
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public List<Monster> Monsters
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        private List<Monster> AllMonsters
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public List<Monster> MonstersToRemove
        {
            get;
            protected set;
        }

        [ContentSerializerIgnore]
        public Map Map
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Path Path
        {
            get { return Map.ActivePath; }
        }

        [ContentSerializer]
        public string Title
        {
            get;
            set;
        }

        [ContentSerializer]
        public bool BossWave
        {
            get;
            set;
        }

        [ContentSerializer]
        public float MonsterSpawnTimer
        {
            get;
            set;
        }

        [ContentSerializer]
        public int MoneyPerKill
        {
            get;
            private set;
        }

        [ContentSerializer]
        public float BossMoneyScalar
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public bool IsDone
        {
            get;
            protected set;
        }

        public Wave()
        {
            MonsterAssets = new List<string>();
            Monsters = new List<Monster>();
            AllMonsters = new List<Monster>();
            MonstersToRemove = new List<Monster>();
        }

        public void Initialize(Map map)
        {
            Map = map;
            Reset();
        }

        public void Update(GameTime gameTime)
        {
            if(!IsDone)
            {
                if (Monsters.Count == 0) { IsDone = true; return; }
                foreach (Monster m in MonstersToRemove)
                {
                    Monsters.Remove(m);
                }

                MonstersToRemove.Clear();

                foreach (Monster m in Monsters)
                {
                    m.Update(gameTime);
                }
            }
        }

        public void Remove(Monster m)
        {
            MonstersToRemove.Add(m);
            if (m.Health > 0)
            {
                Session.singleton.DecreaseHealth(m.IsBoss ? 2 : 1);
            }
        }

        public void Reset()
        {
            Point p = Map.ToWorldCoordinates(Path.Start);
            if (Monsters.Count != AllMonsters.Count)
            {
                Monsters.Clear();
                foreach (Monster m in AllMonsters)
                {
                    Monsters.Add(m.Clone());
                }
            }

            for (int i = 0; i < Monsters.Count; i++)
            {
                Monsters[i].Wave = this;
                Monsters[i].Delay = MonsterSpawnTimer * (i + 1);
                Monsters[i].Position = new Vector2(p.X, p.Y);
            }

            IsDone = false;
        }

        public override string ToString()
        {
            return Title;
        }

        public class WaveReader : ContentTypeReader<Wave>
        {
            protected override Wave Read(ContentReader input, Wave existingInstance)
            {
                Wave wave = new Wave();

                wave.MonsterAssets.AddRange(input.ReadObject<List<string>>());
                Monster m = null;
                foreach (string s in wave.MonsterAssets)
                {
                    m = input.ContentManager.Load<Monster>(String.Format("Monsters\\{0}", s));
                    wave.Monsters.Add(m.Clone());
                    wave.AllMonsters.Add(m.Clone());
                }

                wave.Title = input.ReadString();
                wave.BossWave = input.ReadBoolean();
                wave.MonsterSpawnTimer = input.ReadSingle();
                wave.MoneyPerKill = input.ReadInt32();
                wave.BossMoneyScalar = input.ReadSingle();

                return wave;
            }
        }

        public Wave Clone()
        {
            Wave wave = new Wave();

            foreach (Monster m in Monsters)
            {
                wave.Monsters.Add(m.Clone());
                wave.AllMonsters.Add(m.Clone());
            }

            wave.Title = Title.Clone().ToString();
            wave.BossWave = BossWave;
            wave.MonsterSpawnTimer = MonsterSpawnTimer;
            wave.MoneyPerKill = MoneyPerKill;
            wave.BossMoneyScalar = BossMoneyScalar;

            return wave;
        }
    }
}
