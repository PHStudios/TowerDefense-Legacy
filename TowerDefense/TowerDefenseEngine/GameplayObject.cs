using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerDefenseEngine
{
    /// <summary>
    /// Holds the states our object can have
    /// </summary>
    public enum ObjectStatus
    {
        Active,
        Dying,
        Dead
    }

    [ContentTypeWriter]
    public class GameplayObjectWriter : ContentTypeWriter<GameplayObject>
    {
        protected override void Write(ContentWriter output, GameplayObject value)
        {
            output.Write(value.Name);
            output.Write(value.Description);
            output.Write(value.Alpha);
            output.Write(value.Speed);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return typeof(GameplayObject.GameplayObjectReader).AssemblyQualifiedName;
        }
    }

    public class GameplayObject
    {
        #region Description
        [ContentSerializerIgnore]
        public string AssetName
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            set;
        }

        [ContentSerializer(Optional = true)]
        public string Description
        {
            get;
            set;
        }
        #endregion

        #region Status Data
        /// <summary>
        /// The current status of the game object.  No public set since this should all be done internally
        /// If you need to set this externally, add the set in.  Use protected set if you only set with
        /// derived classes.
        /// </summary>
        ObjectStatus status;
        public ObjectStatus Status
        {
            get { return status; }
        }

        #endregion

        #region Graphics Data
        /// <summary>
        /// The current texture of our game object
        /// </summary>
        [ContentSerializerIgnore]
        Texture2D texture;

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                //When we set the texture, we also want to get
                //the data right away and calculate the matrix
                if (value != null)
                {
                    texture = value;
                    TextureData = new Color[value.Width * value.Height];
                    Texture.GetData(TextureData);

                    //Calculate the matrix of the object
                    CalculateMatrix();
                }
            }
        }

        //The color data of every pixel in
        //the image
        [ContentSerializerIgnore]
        public Color[] TextureData
        {
            get;
            private set;
        }

        /// <summary>
        /// A rectangle of the object used for rectangular collision
        /// </summary>
        [ContentSerializerIgnore]
        Rectangle rectangle;

        [ContentSerializerIgnore]
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        /// <summary>
        /// Transform matrix for correct rectangle collision and
        /// per-pixel collision
        /// </summary>
        [ContentSerializerIgnore]
        public Matrix Transform
        {
            get;
            private set;
        }

        /// <summary>
        /// Origin property used for the center of the game object
        /// </summary>
        [ContentSerializerIgnore]
        public Vector2 Origin
        {
            get
            {
                return new Vector2(texture.Width / 2, texture.Height / 2);
            }
        }

        /// <summary>
        /// Opacity and alpha of the object
        /// </summary>
        public float Alpha
        {
            get;
            set;
        }

        /// <summary>
        /// Color defaulted to White.  The Color property uses the Alpha property
        /// to automatically use the opacity.
        /// </summary>
        [ContentSerializerIgnore]
        Color color = Color.White;

        [ContentSerializerIgnore]
        public Color Color
        {
            get
            {
                return color * Alpha;
            }
            set { color = value; }
        }
        #endregion

        #region Physics Data
        /// <summary>
        /// The location of the game object in the game world
        /// </summary>
        [ContentSerializerIgnore]
        Vector2 position = Vector2.Zero;

        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Object's velocity to update the position
        /// </summary>
        [ContentSerializerIgnore]
        Vector2 velocity = Vector2.Zero;

        [ContentSerializerIgnore]
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Object's acceleration to update the velocity and speed
        /// </summary>
        [ContentSerializerIgnore]
        Vector2 acceleration = Vector2.Zero;

        [ContentSerializerIgnore]
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        /// <summary>
        /// Where our object is looking
        /// </summary>
        [ContentSerializerIgnore]
        float rotation = 0f;

        [ContentSerializerIgnore]
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Speed of our object, used with rotation and updated velocity.
        /// </summary>
        public float Speed
        {
            get;
            set;
        }
        #endregion

        #region Die Data
        /// <summary>
        /// The time it takes for our object to die
        /// </summary>
        [ContentSerializerIgnore]
        TimeSpan dieTime = TimeSpan.Zero;

        [ContentSerializerIgnore]
        public TimeSpan DieTime
        {
            get { return dieTime; }
            set { dieTime = value; }
        }

        /// <summary>
        /// 0.0 - fully alive, 1.0 - fully dead.
        /// </summary>
        [ContentSerializerIgnore]
        float diePercent = 0.0f;
        #endregion

        #region Initialization Methods
        public GameplayObject()
        {
            Alpha = 1.0f;
            Initialize();
        }
        /// <summary>
        /// Initialize method, used for initializing object's variables and fields
        /// </summary>
        public virtual void Initialize()
        {
            if (!(status == ObjectStatus.Active))
                status = ObjectStatus.Active;
        }
        #endregion

        #region Update and Draw Methods
        /// <summary>
        /// Update the game object. 
        /// </summary>
        /// <param name="gameTime">The GameTime object from the active screen</param>
        public virtual void Update(GameTime gameTime)
        {
            if (status == ObjectStatus.Active)
            {
                float speed = Session.singleton == null ? 1.0f : Session.singleton.Speed;
                velocity += Vector2.Multiply(acceleration, (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
                position += Vector2.Multiply(velocity, (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
                CalculateMatrix();
                CalculateBoundingRectangle();
            }
            else if (status == ObjectStatus.Dying)
            {
                Dying(gameTime);
            }
            else if (status == ObjectStatus.Dead)
            {
                Dead(gameTime);
            }
        }

        /// <summary>
        /// Calculates the transform matrix of the object with the origin,
        /// rotation, scale, and position.  This will need to be done every
        /// game loop because chances are the position changed.
        /// </summary>
        private void CalculateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(1.0f) *
                Matrix.CreateTranslation(new Vector3(position, 0));
        }

        /// <summary>
        /// Calculates the bounding rectangle of the object using the object's transform
        /// matrix to make a correct rectangle.
        /// </summary>
        private void CalculateBoundingRectangle()
        {
            if (texture != null)
            {
                rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Vector2 leftTop = Vector2.Transform(new Vector2(rectangle.Left, rectangle.Top), Transform);
                Vector2 rightTop = Vector2.Transform(new Vector2(rectangle.Right, rectangle.Top), Transform);
                Vector2 leftBottom = Vector2.Transform(new Vector2(rectangle.Left, rectangle.Bottom), Transform);
                Vector2 rightBottom = Vector2.Transform(new Vector2(rectangle.Right, rectangle.Bottom), Transform);

                Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                  Vector2.Min(leftBottom, rightBottom));
                Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                          Vector2.Max(leftBottom, rightBottom));

                rectangle = new Rectangle((int)min.X, (int)min.Y,
                    (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }

        public void NormalizeVelocity()
        {
            velocity.Normalize();
        }

        /// <summary>
        /// Logic to perform when the object is dying, but not dead yet
        /// </summary>
        /// <param name="gameTime">The GameTime object from the active screen</param>
        public virtual void Dying(GameTime gameTime)
        {
            if (diePercent >= 1)
                status = ObjectStatus.Dead;
            else
            {
                float dieDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / dieTime.TotalMilliseconds);
                diePercent += dieDelta;
            }
        }

        /// <summary>
        /// Logic to perform when the object is completely dead.
        /// </summary>
        /// <param name="gameTime">The GameTime object from the active screen</param>
        public virtual void Dead(GameTime gameTime) { }

        /// <summary>
        /// Collision logic with another GameplayObject
        /// </summary>
        /// <param name="target"></param>
        public virtual void Collision(GameplayObject target) { }

        /// <summary>
        /// Method called when you want the object to die
        /// </summary>
        public void Die()
        {
            if (status == ObjectStatus.Active)
            {
                if (dieTime != TimeSpan.Zero)
                    status = ObjectStatus.Dying;
                else
                    status = ObjectStatus.Dead;
            }
        }

        /// <summary>
        /// Draws the gameplay object to the game world.  spriteBatch.Begin HAS to be called before this.
        /// </summary>
        /// <param name="gameTime">The GameTime object from the active screen</param>
        /// <param name="spriteBatch">The spriteBatch from the GameScreen (the one you used to call .Begin</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                if (texture != null)
                {
                    spriteBatch.Draw(texture, position, null, Color, rotation, Origin, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        public void DrawBounds(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                if (bounds != null)
                    spriteBatch.Draw(bounds, rectangle, Color.White);
            }
        }
        #endregion

        public static Texture2D bounds;

        public class GameplayObjectReader : ContentTypeReader<GameplayObject>
        {
            protected override GameplayObject Read(ContentReader input, GameplayObject existingInstance)
            {
                GameplayObject result = existingInstance;

                if (result == null) result = new GameplayObject();

                result.AssetName = input.AssetName;
                result.Name = input.ReadString();
                result.Description = input.ReadString();
                result.Alpha = input.ReadSingle();
                result.Speed = input.ReadSingle();

                return result;
            }
        }
    }
}