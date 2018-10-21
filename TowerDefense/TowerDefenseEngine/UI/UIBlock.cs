using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public class UIBlock
    {
        Texture2D border, pixel;
        Rectangle borderTop, borderBottom, borderRight;

        public Rectangle Dimensions
        {
            get;
            private set;
        }

        public Session Session
        {
            get;
            private set;
        }

        public Dictionary<string, Button> Buttons
        {
            get;
            private set;
        }

        public Dictionary<string, Text> Text
        {
            get;
            private set;
        }

        public Dictionary<string, Image> Images
        {
            get;
            private set;
        }

        Color color;

        public UIBlock(GraphicsDevice gd, Texture2D borderTexture, Color borderColor, Rectangle dims, Session s)
        {
            border = borderTexture;
            pixel = new Texture2D(gd, 1, 1);
            Color[] c = new Color[1];
            c[0] = borderColor;
            pixel.SetData<Color>(c);

            Dimensions = dims;
            if (border != null)
            {
                borderTop = new Rectangle(Dimensions.Right - border.Width, Dimensions.Top, border.Width, border.Height);
                borderRight = new Rectangle(Dimensions.Right - 1, Dimensions.Top + border.Height, 1, Dimensions.Height - (border.Height * 2));
                borderBottom = new Rectangle(Dimensions.Right - border.Width, Dimensions.Bottom - border.Height, border.Width, border.Height);
            }

            Session = s;

            Buttons = new Dictionary<string, Button>();
            Text = new Dictionary<string, Text>();
            Images = new Dictionary<string, Image>();
        }

        public UIBlock(GraphicsDevice gd, Rectangle dims, Color c)
        {
            Dimensions = dims;
            color = c;

            Buttons = new Dictionary<string, Button>();
            Text = new Dictionary<string, Text>();
            Images = new Dictionary<string, Image>();
        }

        public void Add(string s, Text t)
        {
            Text.Add(s, t);
        }

        public Text GetText(string name)
        {
            return Text[name];
        }

        public void Add(string s, Button b)
        {
            Buttons.Add(s, b);
        }

        public Button GetButton(string name)
        {
            if (Buttons.ContainsKey(name)) return Buttons[name];
            else return null;
        }

        public void Add(string s, Image i)
        {
            Images.Add(s, i);
        }

        public Image GetImage(string name)
        {
            return Images[name];
        }

        public void ClearAll()
        {
            Text.Clear();
            Buttons.Clear();
            Images.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (border != null)
            {
                spriteBatch.Draw(border, borderTop, Color.White);
                spriteBatch.Draw(pixel, borderRight, Color.White);
                spriteBatch.Draw(border, borderBottom, Color.White);
            }

            foreach (var bentry in Buttons)
            {
                bentry.Value.Draw(gameTime, spriteBatch);
            }

            foreach (var ientry in Images)
            {
                ientry.Value.Draw(spriteBatch);
            }

            foreach (var tentry in Text)
            {
                if(Session != null)
                    tentry.Value.Draw(spriteBatch, tentry.Value.Font == null ? spriteFont : tentry.Value.Font, Session.Map.ForeColor);
                else
                    tentry.Value.Draw(spriteBatch, tentry.Value.Font == null ? spriteFont : tentry.Value.Font, color);
            }
        }
    }
}
