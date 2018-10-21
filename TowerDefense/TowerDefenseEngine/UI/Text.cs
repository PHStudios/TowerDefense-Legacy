using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public class Text
    {
        string textValue;
        public string Value
        {
            get { return textValue; }
            set
            {
                textValue = value;
                if(Font != null) Dimensions = Font.MeasureString(value);
                Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y);
            }
        }

        SpriteFont font;
        public SpriteFont Font
        {
            get { return font; }
            private set
            {
                font = value;
                Dimensions = font.MeasureString(textValue);
                Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y);
            }
        }

        public Vector2 Dimensions
        {
            get;
            private set;
        }

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y);
            }
        }

        public Vector2 Velocity
        {
            get;
            set;
        }

        public Rectangle Rectangle
        {
            get;
            private set;
        }

        public Text(string value, SpriteFont font)
        {
            Value = value;
            Font = font;
        }

        public Text(string value, SpriteFont font, Vector2 pos)
        {
            Value = value;
            Font = font;
            Position = pos;
        }

        public Text(string value, Vector2 pos)
        {
            Value = value;
            Position = pos;
        }

        public Text(string value, Vector2 pos, Vector2 vel)
        {
            Value = value;
            Position = pos;
            Velocity = vel;
        }

        public void Update(GameTime gameTime)
        {
            float deltaseconds = (float)gameTime.ElapsedGameTime.TotalSeconds * Session.singleton.Speed;
            position += Vector2.Multiply(Velocity, deltaseconds);
        }

        public void Draw(SpriteBatch spriteBatch, Color c)
        {
            if (font != null)
            {
                spriteBatch.DrawString(font, Value, Position, c);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Color c)
        {
            spriteBatch.DrawString(spriteFont, Value, Position, c);
        }
    }
}
