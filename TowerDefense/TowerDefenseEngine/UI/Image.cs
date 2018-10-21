using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public class Image
    {
        public Texture2D Texture
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public Rectangle Rectangle
        {
            get;
            private set;
        }

        public Image(Texture2D tex, Vector2 pos)
        {
            Texture = tex;
            Position = pos;
            Rectangle = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

    }
}
