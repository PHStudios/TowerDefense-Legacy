using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.IO;

namespace TowerDefenseEngine
{
    public abstract class Ability
    {
        [ContentSerializerIgnore]
        public string AssetName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public TowerStatistics TowerModifiers
        {
            get;
            set;
        }

        public MonsterStatistics MonsterModifiers
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public int Level
        {
            get;
            set;
        }

        public int MaxLevel
        {
            get;
            set;
        }

        public string IconAssetName
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D Icon
        {
            get;
            protected set;
        }

        [ContentSerializerIgnore]
        public Tower Owner
        {
            get;
            protected set;
        }

        [ContentSerializerIgnore]
        public Monster Target
        {
            get;
            protected set;
        }

        public bool TargetRequired
        {
            get;
            set;
        }

        public float Duration
        {
            get;
            set;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void AddedToTower(Tower t)
        {
            if (Owner != null) throw new Exception("A single tower ability cannot be added to more than one tower.");
            Owner = t;
        }

        public virtual void AttackedEnemy(Monster m)
        {
            Target = m;
        }

        public class AbilityReader : ContentTypeReader<Ability>
        {
            protected override Ability Read(ContentReader input, Ability existingInstance)
            {
                if (existingInstance == null)
                {
                    throw new Exception("Ability's existing instance is NULL.");
                }

                existingInstance.AssetName = input.AssetName;
                existingInstance.Name = input.ReadString();
                existingInstance.Description = input.ReadString();
                existingInstance.TowerModifiers = input.ReadRawObject<TowerStatistics>();
                existingInstance.MonsterModifiers = input.ReadRawObject<MonsterStatistics>();
                existingInstance.Level = 1;
                existingInstance.MaxLevel = input.ReadInt32();
                existingInstance.IconAssetName = input.ReadString();
                if (!string.IsNullOrEmpty(existingInstance.IconAssetName))
                    existingInstance.Icon = input.ContentManager.Load<Texture2D>(System.IO.Path.Combine("@Textures\\Icons\\Ability\\", existingInstance.IconAssetName));
                existingInstance.TargetRequired = input.ReadBoolean();
                existingInstance.Duration = input.ReadSingle();

                return existingInstance;
            }
        }

    }
}
