using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using ParticleSystemLibrary;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public enum BulletState
    {
        InActive,
        Active,
        Hit
    }

    public enum BulletType
    {
        Normal,
        AoE,
        DoT
    }

    public class Bullet
    {
        #region Writer
        [ContentTypeWriter]
        public class BulletWriter : ContentTypeWriter<Bullet>
        {
            protected override void Write(ContentWriter output, Bullet value)
            {
                output.Write(value.Type.ToString());
                output.Write(value.AoERadius);
                output.Write(value.dotDamagePercentage);
                output.Write(value.dotTime);
                output.Write(value.TextureAsset);
                output.Write(value.ParticleAsset);
                output.Write(value.Speed);
                output.Write(value.DieTime);
            }


            public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
            {
                return typeof(Bullet.BulletReader).AssemblyQualifiedName;
            }
        }
        #endregion

        [ContentSerializerIgnore]
        public string Name
        {
            get;
            private set;
        }

        [ContentSerializer]
        public BulletType Type
        {
            get;
            private set;
        }

        [ContentSerializer]
        public int AoERadius
        {
            get;
            private set;
        }

        [ContentSerializer]
        public float dotDamagePercentage
        {
            get;
            private set;
        }

        [ContentSerializer]
        public float dotTime
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

        [ContentSerializer]
        private string ParticleAsset
        {
            get;
            set;
        }

        [ContentSerializer]
        public float Speed
        {
            get;
            private set;
        }

        [ContentSerializer]
        private float DieTime
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public BulletState State
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Tower Owner
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Texture2D Texture
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

        [ContentSerializerIgnore]
        public Rectangle Rectangle
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Texture2D ParticleTexture
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
        public Vector2 Velocity
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
        public float Timer
        {
            get;
            private set;
        }

        ParticleSystem ParticleSystem;
        Emitter emitter;

        public void Initialize(Map map, ContentManager contentManager)
        {
            Name = String.Format("{0}-{1}", map.Name, TextureAsset);
            Texture = contentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\Bullets\\{1}", map.Name, TextureAsset));
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            ParticleTexture = contentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\Bullets\\{1}", map.Name, ParticleAsset));

            State = BulletState.InActive;
        }

        public void Update(GameTime gameTime)
        {
            if (State == BulletState.Active)
            {
                Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds * Session.singleton.Speed);
                CalculateBoundingRectangle(CalculateMatrix());
                foreach (Monster m in Owner.Map.ActiveWave.Monsters)
                {
                    if (m.IsActive && Rectangle.Intersects(m.Rectangle))
                    {
                        State = BulletState.Hit;
                        Hit(m);
                        return;
                    }
                }
            }
            else if (State == BulletState.Hit)
            {
                if (Timer <= 0) State = BulletState.InActive;
                else
                {
                    Timer -= (float)gameTime.ElapsedGameTime.TotalSeconds * Session.singleton.Speed;
                    ParticleSystem.Update(gameTime);
                }
            }
        }

        public void Fire(Tower t)
        {
            Owner = t;
            Position = new Vector2(t.Position.X, t.Position.Y);
            Velocity = new Vector2(Speed * (float)Math.Cos(t.Rotation), Speed * (float)Math.Sin(t.Rotation));
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            State = BulletState.Active;
        }

        public void Hit(Monster m)
        {
            Timer = DieTime;
            m.hit(this, Owner);

            if (Type == BulletType.AoE)
            {
                PerformAOEAttack(m);
            }
            else if (Type == BulletType.DoT)
            {
                m.ApplyDOT((int)(Owner.CurrentStatistics.Damage * dotDamagePercentage), dotTime);
            }

            ParticleSystem = new ParticleSystem();
            ParticleSystem.SystemTimer = Timer;
            ParticleSystem.ParticleLongevity = Timer;
            ParticleSystem.BirthRate = 0;
            ParticleSystem.InitialParticles = 5;
            ParticleSystem.TextureName = ParticleAsset;

            ParticleSystem.BirthRevolutions = 0;
            ParticleSystem.DeathRevolutions = 8f;

            ParticleSystem.Initialize();

            emitter = new Emitter(ParticleTexture);
            emitter.Radius = new Vector2(0, 0);
            emitter.Position = Position;
            emitter.Velocity = new Vector2(Velocity.X / 5.0f, Velocity.Y / 5.0f);
            emitter.Velocity.Normalize();

            ParticleSystem.Emitter = emitter;



        }

        private void PerformAOEAttack(Monster monster)
        {
            foreach (Monster m in Session.singleton.Map.ActiveWave.Monsters)
            {
                if (m == monster) continue;

                float distance = (new Vector2(Position.X - m.Position.X, Position.Y - m.Position.Y)).Length();
                if (distance <= AoERadius * Session.singleton.Map.TileDimensions.X)
                {
                    m.hit(this, Owner);
                }
            }
        }

        /// <summary>
        /// Calculates the transform matrix of the object with the origin,
        /// rotation, scale, and position.  This will need to be done every
        /// game loop because chances are the position changed.
        /// </summary>
        private Matrix CalculateMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(1.0f) *
                Matrix.CreateTranslation(new Vector3(Position, 0));
        }

        /// <summary>
        /// Calculates the bounding rectangle of the object using the object's transform
        /// matrix to make a correct rectangle.
        /// </summary>
        private void CalculateBoundingRectangle(Matrix Transform)
        {
            if (Texture != null)
            {
                Rectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Vector2 leftTop = Vector2.Transform(new Vector2(Rectangle.Left, Rectangle.Top), Transform);
                Vector2 rightTop = Vector2.Transform(new Vector2(Rectangle.Right, Rectangle.Top), Transform);
                Vector2 leftBottom = Vector2.Transform(new Vector2(Rectangle.Left, Rectangle.Bottom), Transform);
                Vector2 rightBottom = Vector2.Transform(new Vector2(Rectangle.Right, Rectangle.Bottom), Transform);

                Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                  Vector2.Min(leftBottom, rightBottom));
                Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                          Vector2.Max(leftBottom, rightBottom));

                Rectangle = new Rectangle((int)min.X, (int)min.Y,
                    (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (State == BulletState.Active)
            {
                spriteBatch.Draw(Texture, Rectangle, null, Color.White, Rotation, Origin, SpriteEffects.None, 1.0f);
            }
            else if (State == BulletState.Hit)
            {
                ParticleSystem.DrawOnExistingSpritebatchCycle(gameTime, spriteBatch);
            }
                
        }



        #region Content Reader
        public class BulletReader : ContentTypeReader<Bullet>
        {
            protected override Bullet Read(ContentReader input, Bullet existingInstance)
            {
                Bullet b = new Bullet();

                b.Type = (BulletType)Enum.Parse(typeof(BulletType), input.ReadString());
                b.AoERadius = input.ReadInt32();
                b.dotDamagePercentage = input.ReadSingle();
                b.dotTime = input.ReadSingle();
                b.TextureAsset = input.ReadString();
                b.ParticleAsset = input.ReadString();
                b.Speed = input.ReadSingle();
                b.DieTime = input.ReadSingle();

                if (string.IsNullOrEmpty(b.TextureAsset)) throw new Exception("You must provide a texture asset in the " + input.AssetName + ".xml file.");
                if (string.IsNullOrEmpty(b.ParticleAsset)) throw new Exception("You must provide a particle asset in the " + input.AssetName + ".xml file.");

                return b;
            }

        }
        #endregion


        internal Bullet Clone()
        {
            Bullet b = new Bullet();

            b.Type = Type;
            b.AoERadius = AoERadius;
            b.dotDamagePercentage = dotDamagePercentage;
            b.dotTime = dotTime;
            b.TextureAsset = TextureAsset;
            b.ParticleAsset = ParticleAsset;
            b.Speed = Speed;
            b.DieTime = DieTime;

            b.Texture = Texture;
            b.Origin = Origin;
            b.ParticleTexture = ParticleTexture;

            b.State = BulletState.InActive;

            return b;
        }
    }
}
