using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TowerDefenseEngine;

namespace TowerDefense
{
    public class PlayScreen : GameScreen
    {
        UserInterface ui;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        Map map;
        Session session;
        List<Tower> towers;

        SpriteFont font;

        LevelSelectionScreen levelselect;

        public PlayScreen(LevelSelectionScreen lss, Map m)
        {
            map = m;
            levelselect = lss;
            towers = new List<Tower>(10);
            session = new Session(m);
            session.HealthDecreased += new EventHandler(session_HealthDecreased);
            session.MapFinished += new EventHandler(session_MapFinished);
        }

        void session_MapFinished(object sender, EventArgs e)
        {
            if (session.health >= 0)
            {
                ExitScreen();
                levelselect.ActivateScreen();
            }
            else
            {
                ExitScreen();
                ScreenSystem.AddScreen(new GameOverScreen(levelselect));
            }
        }

        void session_HealthDecreased(object sender, EventArgs e)
        {
            if (session.health < 0)
            {
                ExitScreen();
                ScreenSystem.AddScreen(new GameOverScreen(levelselect));
            }
        }

        public override void InitializeScreen()
        {
            //CHANGE FOR DYNAMIC RESOLUTION
            ui = new UserInterface(new MapRegion(session, new Rectangle(0, 0, 960, 690), ScreenSystem.GraphicsDevice),
                new WaveInformation(session, new Rectangle(0, 690, 1280, 30), ScreenSystem.GraphicsDevice, Color.Black), 
                new CommandInfoBar(session, new Rectangle(960, 0, 320, 690), ScreenSystem.GraphicsDevice), session);

            AudioManager.singleton.PlaySong(map.SongCueName);
            InputMap.NewAction("Pause", Microsoft.Xna.Framework.Input.Keys.Escape);
        }

        public override void LoadContent()
        {

            Texture2D tex = new Texture2D(ScreenSystem.GraphicsDevice, 1, 1);
            Color[] cArray = new Color[] { Color.White };
            tex.SetData<Color>(cArray);

            ui.LoadPlaceholder(tex);

            ui.Font = ScreenSystem.Content.Load<SpriteFont>("Fonts\\playfont");
        }

        protected override void DrawScreen(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;
            ui.DrawUI(gameTime, spriteBatch);
        }

        protected override void UpdateScreen(GameTime gameTime)
        {
            if (session.IsPaused)
            {
                FreezeScreen();
                ScreenSystem.AddScreen(new PauseScreen(this, session));
            }

            if (InputMap.NewActionPress("Pause"))
            {
                session.Pause();
            }

            session.Update(gameTime);
        }
    }
}
