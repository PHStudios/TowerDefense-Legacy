using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseEngine
{
    public class Tower
    {
        #region Writer
        [ContentTypeWriter]
        public class TowerWriter : ContentTypeWriter<Tower>
        {
            protected override void Write(ContentWriter output, Tower value)
            {
                output.Write(value.Name);
                output.Write(value.Description);
                output.Write(value.ThumbnailAsset);
                output.Write(value.TextureAsset);
                output.Write(value.RadiusTextureAsset);
                output.Write(value.Cost);
                output.Write(value.Level);
                output.Write(value.MaxLevel);
                output.WriteObject(value.InitialStatistics);
                output.WriteObject(value.UpgradeStatistics);
                output.Write(value.BulletAsset);
                output.Write(value.SellScalar);
            }

            public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
            {
                return typeof(Tower.TowerReader).AssemblyQualifiedName;
            }
        }

        #endregion

        [ContentSerializer]
        public string Name
        {
            get;
            private set;
        }

        [ContentSerializer]
        public string Description
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string ThumbnailAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D Thumbnail
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string TextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string RadiusTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D RadiusTexture
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Vector2 Origin
        {
            get;
            private set;
        }

        [ContentSerializer]
        public int Cost
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public int UpgradeCost
        {
            get { return ((Level + 1) * Cost) / 2; }
        }

        [ContentSerializerIgnore]
        public int TotalCost
        {
            get;
            private set;
        }

        int level;

        [ContentSerializer]
        public int Level
        {
            get { return level; }
            set
            {
                if (value < MaxLevel)
                {
                    level = value;
                    LevelUp();
                }
            }
        }

        [ContentSerializer]
        public int MaxLevel
        {
            get;
            private set;
        }

        TowerStatistics initialStatistics;

        [ContentSerializer]
        public TowerStatistics InitialStatistics
        {
            get { return initialStatistics; }
            set
            {
                initialStatistics = value;
                LevelUp();
            }
        }

        [ContentSerializerIgnore]
        public Map Map
        {
            get;
            private set;
        }

        private void LevelUp()
        {
            if (level > 0 && initialStatistics != null && UpgradeStatistics != null)
            {
                CurrentStatistics = TowerStatistics.Add(initialStatistics, UpgradeStatistics.Multiply(UpgradeStatistics, level));
            }
        }

        [ContentSerializerIgnore]
        public TowerStatistics CurrentStatistics
        {
            get;
            protected set;
        }

        UpgradeStatistics upgradeStatistics;

        [ContentSerializer]
        public UpgradeStatistics UpgradeStatistics
        {
            get { return upgradeStatistics; }
            set
            {
                upgradeStatistics = value;
                LevelUp();
            }
        }

        [ContentSerializer]
        private string BulletAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Bullet bulletBase
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public List<Bullet> Bullets
        {
            get;
            private set;
        }

        [ContentSerializer]
        public float SellScalar
        {
            get;
            private set;
        }

        Point mapLocation;

        [ContentSerializerIgnore]
        public Point MapLocation
        {
            get { return mapLocation; }
            set { 
                mapLocation = value;
                SetLocation();
            }
        }

        [ContentSerializerIgnore]
        public Rectangle Rectangle
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public float Rotation
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public bool Initialized
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public bool IsPlaced
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public ObjectClickedState LeftState
        {
            get;
            protected set;
        }

        [ContentSerializerIgnore]
        public Mouse ActiveMouse
        {
            get;
            protected set;
        }

        public event EventHandler LeftClickEvent;

        [ContentSerializerIgnore]
        public Monster Target
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public float PlacedTime
        {
            get;
            private set;
        }

        float timer;

        public void Initialize(Map map, ContentManager contentManager)
        {
            Map = map;

            if (!string.IsNullOrEmpty(ThumbnailAsset)) Thumbnail = contentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, ThumbnailAsset));
            if (!string.IsNullOrEmpty(TextureAsset))
            {
                Texture = contentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, TextureAsset));
                Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            }
            if (!string.IsNullOrEmpty(RadiusTextureAsset))
            {
                RadiusTexture = contentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, RadiusTextureAsset));
            }

            bulletBase.Initialize(map, contentManager);

            timer = CurrentStatistics.Speed;

            Initialized = true;
        }

        public void Initialize(Map map)
        {
            Map = map;
            Initialized = true;
            Bullets = new List<Bullet>();
        }

        private void SetLocation()
        {
            if (Map != null)
            {
                Point p = Map.ToWorldCoordinates(mapLocation.X, mapLocation.Y);
                Rectangle = new Rectangle((int)(p.X), (int)(p.Y), Texture.Width, Texture.Height);
                Position = new Vector2(p.X + Origin.X, p.Y + Origin.Y);
            }
        }

        public void PlaceTower()
        {
            IsPlaced = true;
            TotalCost = Cost;
        }

        public void Upgrade()
        {
            TotalCost += UpgradeCost;
            Level = level + 1;
        }

        public void Update(GameTime gameTime, Mouse activeMouse)
        {
            if (IsPlaced) PlacedTime += (float)(gameTime.ElapsedGameTime.TotalSeconds * Session.singleton.Speed);
            if (timer > 0)
            {
                timer -= (float)(gameTime.ElapsedGameTime.TotalSeconds * Session.singleton.Speed);
            }

            UpdateMouse(gameTime, activeMouse);

            foreach (Bullet b in Bullets)
            {
                b.Update(gameTime);
            }

            if (Target == null)
            {
                Target = FindTarget();
            }
            else
            {
                UpdateTarget(gameTime);
            }
            
        }

        private void UpdateTarget(GameTime gameTime)
        {
            float distance = Vector2.Distance(Target.Position, Position);
            if ((distance <= CurrentStatistics.Radius * Session.singleton.Map.TileDimensions.X && Target.IsActive)
                && (!(Map.ToMapCoordinates((int)Target.Position.X, (int)Target.Position.Y).Equals(Map.CastleLocation))))
            {
                float rate = bulletBase.Speed;
                Vector2 d = Vector2.Add(Target.Position, Vector2.Multiply(Target.Velocity, distance/rate)) - Position;
                Rotation = (float)Math.Atan2(d.Y, d.X);

                if (timer <= 0)
                {
                    Bullet b = bulletBase.Clone();
                    b.Fire(this);
                    Bullets.Add(b);
                    timer = CurrentStatistics.Speed;
                }
            }
            else
            {
                Target.DieEvent -= new EventHandler(m_DieEvent);
                Target = null;
            }
        }

        private Monster FindTarget()
        {
            Wave active = Session.singleton.Map.ActiveWave;
            Monster m = null;
            float distance = 0.0f, newdistance = 0.0f;
            if (active.Monsters.Count > 0)
            {
                m = active.Monsters[0];
                distance = Vector2.DistanceSquared(m.Position, Position);
                for (int i = 1; i < active.Monsters.Count; i++)
                {
                    newdistance = Vector2.DistanceSquared(active.Monsters[i].Position, Position);

                    if (newdistance < distance)
                    {
                        distance = newdistance;
                        m = active.Monsters[i];
                    }
                }
            }

            if ((Math.Sqrt(distance) <= CurrentStatistics.Radius * Session.singleton.Map.TileDimensions.X) && (m != null && m.IsActive))
            {
                m.DieEvent += new EventHandler(m_DieEvent);
                return m;
            }
            else
                return null;

        }

        void m_DieEvent(object sender, EventArgs e)
        {
            Target = null;
        }

        private void UpdateMouse(GameTime gameTime, Mouse activeMouse)
        {
            bool intersect = activeMouse.Rectangle.Intersects(Rectangle);

            if (intersect || (ActiveMouse != null &&
                (LeftState != ObjectClickedState.Normal)))
            {
                ActiveMouse = activeMouse;

                if (ActiveMouse.LeftClick && (LeftState == ObjectClickedState.Normal
                    || LeftState == ObjectClickedState.Released))
                {
                    LeftState = ObjectClickedState.Clicked;
                    if (LeftClickEvent != null)
                        LeftClickEvent(this, EventArgs.Empty);
                }
                else if (ActiveMouse.LeftRelease && (LeftState == ObjectClickedState.Clicked))
                {
                    LeftState = ObjectClickedState.Normal;
                }
            }

            else
            {
                if (ActiveMouse != null)
                {
                    ActiveMouse = null;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Bullet b in Bullets)
            {
                b.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void DrawRadius(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float scale = (CurrentStatistics.Radius * Session.singleton.Map.TileDimensions.X) / (float)RadiusTexture.Width;
            spriteBatch.Draw(RadiusTexture, Position, null, Color.White, 0.0f, 
                new Vector2(RadiusTexture.Width / 2, RadiusTexture.Height / 2), scale * 2, SpriteEffects.None, 1.0f);
        }

        #region Reader
        public class TowerReader : ContentTypeReader<Tower>
        {
            protected override Tower Read(ContentReader input, Tower existingInstance)
            {
                Tower t = new Tower();

                t.Name = input.ReadString();
                t.Description = input.ReadString();
                t.ThumbnailAsset = input.ReadString();
                t.TextureAsset = input.ReadString();
                t.RadiusTextureAsset = input.ReadString();
                t.Cost = input.ReadInt32();
                t.level = input.ReadInt32();
                if(t.level > 0) t.Name += " " + t.level.ToString();
                t.MaxLevel = input.ReadInt32();
                t.InitialStatistics = input.ReadObject<TowerStatistics>();
                t.CurrentStatistics = t.InitialStatistics;
                t.UpgradeStatistics = input.ReadObject<UpgradeStatistics>();
                t.BulletAsset = input.ReadString();
                t.bulletBase = input.ContentManager.Load<Bullet>(String.Format("Towers\\Bullets\\{0}", t.BulletAsset)).Clone();
                t.SellScalar = input.ReadSingle();
                t.MapLocation = new Point(-1, -1);

                return t;
            }
        }

        #endregion

        internal Tower Clone()
        {
            Tower t = new Tower();

            t.Name = Name;
            t.Description = Description;
            t.ThumbnailAsset = ThumbnailAsset;
            t.Thumbnail = Thumbnail;
            t.TextureAsset = TextureAsset;
            t.Texture = Texture;
            t.RadiusTextureAsset = RadiusTextureAsset;
            t.RadiusTexture = RadiusTexture;
            t.Cost = Cost;
            t.level = level;
            t.MaxLevel = MaxLevel;
            t.InitialStatistics = InitialStatistics;
            t.CurrentStatistics = InitialStatistics;
            t.UpgradeStatistics = UpgradeStatistics;
            t.BulletAsset = BulletAsset;
            t.bulletBase = bulletBase.Clone();
            t.SellScalar = SellScalar;
            t.MapLocation = MapLocation;
            t.Origin = Origin;

            return t;
        }
    }
}
