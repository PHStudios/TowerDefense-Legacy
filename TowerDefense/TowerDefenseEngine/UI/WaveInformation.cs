using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseEngine
{
    public class WaveInformation
    {
        public Rectangle Rectangle
        {
            get;
            private set;
        }

        public Session Session
        {
            get;
            private set;
        }

        Texture2D background;

        public WaveInformation(Session s, Rectangle r, GraphicsDevice gd, Color c)
        {
            Session = s;
            Rectangle = r;
            background = new Texture2D(gd, 1, 1);
            Color[] carray = new Color[] { c };
            background.SetData<Color>(carray);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(background, Rectangle, Color.White);

            if (Session.Map.State == MapState.Active)
            {
                string active = Session.Map.ActiveWave.ToString();
                Vector2 dims = spriteFont.MeasureString(active);
                spriteBatch.DrawString(spriteFont, active, new Vector2(Rectangle.Left + 10, Rectangle.Top), Session.Map.ActiveWave.BossWave ? Session.Map.ErrorColor : Session.Map.ForeColor);

                string wavesleft = string.Empty;
                bool b = false;
                foreach (Wave w in Session.Map.WaveList)
                {
                    if (w == Session.Map.ActiveWave)
                    {
                        b = true;
                        continue;
                    }
                    else if (b)
                    {
                        wavesleft += String.Format("; {0}", w.ToString());
                    }
                    else
                    {
                        continue;
                    }
                }
                spriteBatch.DrawString(spriteFont, wavesleft, new Vector2(Rectangle.Left + 10 + dims.X, Rectangle.Top), Color.White * 0.5f);
            }
            else
            {
                string nextwave = String.Format("Wave \"{0}\" starts in {1} seconds.", Session.Map.ActiveWave.ToString(), (int)Session.Map.Timer);
                spriteBatch.DrawString(spriteFont, nextwave, new Vector2(Rectangle.Left + 10, Rectangle.Top), Session.Map.ForeColor);
            }
        }
    }
}
