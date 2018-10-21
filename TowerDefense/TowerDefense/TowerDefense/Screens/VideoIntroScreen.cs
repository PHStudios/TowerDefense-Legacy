using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using TowerDefenseEngine;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public class VideoIntroScreen : GameScreen
    {
        VideoManager videoManager;
        public override bool AcceptsInput
        {
            get { return true; }
        }

        public override void InitializeScreen()
        {
            videoManager = VideoManager.singleton;

            InputMap.NewAction("Skip", Microsoft.Xna.Framework.Input.Keys.Space);

            Removing += new EventHandler(VideoIntroScreen_Removing);
        }

        public override void LoadContent()
        {
            videoManager.LoadVideo("Intro", Vector2.Zero);
            videoManager.Play();
        }

        void VideoIntroScreen_Removing(object sender, EventArgs e)
        {
            ScreenSystem.AddScreen(new MainMenuScreen());
        }

        protected override void UpdateScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputMap.NewActionPress("Skip") || videoManager.IsDonePlaying)
                ExitScreen();
        }

        protected override void DrawScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            videoManager.Draw(ScreenSystem.SpriteBatch);
        }
    }
}
