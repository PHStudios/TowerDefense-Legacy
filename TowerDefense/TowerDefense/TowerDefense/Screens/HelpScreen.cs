using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public class HelpScreen : GameScreen
    {
        GameScreen screenBefore;
        SpriteFont font;

        List<Texture2D> helpTextures;
        int helpTexturesCount;

        int index;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        public HelpScreen(GameScreen before)
        {
            screenBefore = before;
        }

        public override void InitializeScreen()
        {
            InputMap.NewAction("Finished", Microsoft.Xna.Framework.Input.Keys.Escape);
            InputMap.NewAction("Next", Microsoft.Xna.Framework.Input.Keys.Space);
            helpTexturesCount = 3;
            index = 0;
            helpTextures = new List<Texture2D>(helpTexturesCount);

            EnableFade(Color.Black, 0.85f);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            font = content.Load<SpriteFont>(@"Fonts/help");

            for (int i = 0; i < helpTexturesCount; i++)
            {
                helpTextures.Add(content.Load<Texture2D>(String.Format("Textures\\Help\\help_{0}", i + 1)));
            }
        }

        protected override void DrawScreen(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;

            spriteBatch.Draw(helpTextures[index], new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.DrawString(font, "Press Space to advance to the next image", Vector2.Zero, Color.Red);
        }

        protected override void UpdateScreen(GameTime gameTime)
        {
            if (InputMap.NewActionPress("Finished"))
            {
                ExitScreen();
                screenBefore.ActivateScreen();
            }
            if (InputMap.NewActionPress("Next"))
            {
                if (index >= (helpTextures.Count - 1))
                    index = 0;
                else
                    index++;
            }
        }
    }
}
