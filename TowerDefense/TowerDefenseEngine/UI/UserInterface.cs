using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseEngine
{
    public class UserInterface
    {
        public MapRegion MapRegion
        {
            get;
            private set;
        }
        public WaveInformation WaveInformation
        {
            get;
            private set;
        }
        public CommandInfoBar CommandInfoBar
        {
            get;
            private set;
        }

        SpriteFont font;
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
                CommandInfoBar.Initialize(value);
            }
        }

        Texture2D regionPlaceholder;

        Session session;

        public Mouse mouse
        {
            get;
            private set;
        }

        public UserInterface(MapRegion mapRegion, WaveInformation waveInformation,
            CommandInfoBar commandInfoBar, Session s)
        {
            MapRegion = mapRegion;
            WaveInformation = waveInformation;
            CommandInfoBar = commandInfoBar;
            session = s;
            mouse = new Mouse(session.Map.MouseTexture);
            session.SetUI(this);
        }

        public void LoadPlaceholder(Texture2D placeholder)
        {
            regionPlaceholder = placeholder;
        }

        public void UpdateUI(GameTime gameTime)
        {
            mouse.Update();
            MapRegion.Update(gameTime);
            WaveInformation.Update(gameTime);
            CommandInfoBar.Update(gameTime);
        }

        public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MapRegion.Draw(gameTime, spriteBatch, Font);
            WaveInformation.Draw(gameTime, spriteBatch, Font);
            CommandInfoBar.Draw(gameTime, spriteBatch, Font);

            mouse.Draw(spriteBatch);
        }
    }
}
