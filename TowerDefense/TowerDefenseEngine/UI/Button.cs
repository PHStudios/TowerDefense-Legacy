using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseEngine
{
    public enum UIButtonState
    {
        Active,
        Inactive
    }
    public class Button : ClickableGameplayObject
    {
        public Text ButtonText
        {
            get;
            private set;
        }

        public Color ForeColor
        {
            get;
            private set;
        }

        public object StoredObject
        {
            get;
            private set;
        }

        public UIButtonState State
        {
            get;
            private set;
        }

        public Button(Text text, Color c, object o)
        {
            ButtonText = text;
            ForeColor = c;
            StoredObject = o;
        }

        public Button(Texture2D bg, Vector2 position, object o)
        {
            Texture = bg;
            Position = position;
            StoredObject = o;
        }

        public Button(Texture2D bg, Vector2 position, Text text, Color c, object o)
        {
            ButtonText = text;
            Texture = bg;
            Position = position;
            StoredObject = o;
            ForeColor = c;
        }

        public void SetStoredObject(object obj)
        {
            StoredObject = obj;
        }

        public void SetColor(Color c)
        {
            ForeColor = c;
        }

        public void Activate()
        {
            State = UIButtonState.Active;
        }

        public void Deactivate()
        {
            State = UIButtonState.Inactive;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (ButtonText != null) ButtonText.Draw(spriteBatch, ForeColor);
        }
    }
}
