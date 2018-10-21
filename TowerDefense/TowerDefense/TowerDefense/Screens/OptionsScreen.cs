using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using TowerDefenseEngine;

namespace TowerDefense
{
    public enum OptionsState
    {
        Normal,
        Music,
        Sound
    }
    public class OptionsScreen : GameScreen
    {
        SpriteFont font, smallfont;

        GameScreen screenBefore;

        UIBlock musicBlock, soundBlock;

        OptionsState OptionsState;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        Texture2D inactiveButtonTexture, activeButtonTexture;

        Color Normal, Selected;

        Mouse mouse;

        string backmessage;

        public OptionsScreen(GameScreen before)
        {
            screenBefore = before;
            OptionsState = TowerDefense.OptionsState.Normal;
        }

        public override void InitializeScreen()
        {
            InputMap.NewAction("Volume Up", Microsoft.Xna.Framework.Input.Keys.Up);
            InputMap.NewAction("Volume Down", Microsoft.Xna.Framework.Input.Keys.Down);
            InputMap.NewAction("Finished", Microsoft.Xna.Framework.Input.Keys.Escape);

            EnableFade(Color.Black, 0.85f);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            font = content.Load<SpriteFont>(@"Fonts/menu");
            smallfont = content.Load<SpriteFont>(@"Fonts/menusmall");

            inactiveButtonTexture = content.Load<Texture2D>("Textures\\Menu\\inactivebutton");
            activeButtonTexture = content.Load<Texture2D>("Textures\\Menu\\activebutton");

            Selected = new Color(214, 232, 223);
            Normal = new Color(104, 173, 178);

            mouse = new Mouse(content.Load<Texture2D>("Textures\\menu\\mouse"));

            musicBlock = new UIBlock(ScreenSystem.GraphicsDevice, new Rectangle(10, 110, ScreenSystem.Viewport.Width - 10, 100), Color.White);
            soundBlock = new UIBlock(ScreenSystem.GraphicsDevice, new Rectangle(10, 220, ScreenSystem.Viewport.Width - 10, 100), Color.White);


            Vector2 pos = new Vector2((int)(musicBlock.Dimensions.Left + inactiveButtonTexture.Width), (int)(musicBlock.Dimensions.Top + inactiveButtonTexture.Height));
            Button b = new Button(inactiveButtonTexture, pos,
                new Text(String.Format("Music:{0}%", (int)(Settings.MusicVolume * 100)), font, new Vector2(pos.X - (inactiveButtonTexture.Width / 2) + 10, pos.Y - (inactiveButtonTexture.Height / 2))), Normal, this);
            b.LeftClickEvent += new EventHandler(music_LeftClickEvent);
            musicBlock.Add("MusicButton", b);

            Text minstructions = new Text("Select the Music button to change settings", smallfont, new Vector2(b.Position.X + (b.Texture.Width / 2) + 10, b.ButtonText.Position.Y));
            musicBlock.Add("MusicInstructions", minstructions);

            pos = new Vector2((int)(soundBlock.Dimensions.Left + inactiveButtonTexture.Width), (int)(soundBlock.Dimensions.Top + inactiveButtonTexture.Height));
            b = new Button(inactiveButtonTexture, pos, 
                new Text(String.Format("Sound:{0}%", (int)(Settings.SoundVolume * 100)), font, new Vector2(pos.X - (inactiveButtonTexture.Width / 2) + 10, pos.Y - (inactiveButtonTexture.Height / 2))), Normal, this);
            b.LeftClickEvent += new EventHandler(sound_LeftClickEvent);
            soundBlock.Add("SoundButton", b);

            minstructions = new Text("Select the Sound button to change settings", smallfont, new Vector2(b.Position.X + (b.Texture.Width / 2) + 10, b.ButtonText.Position.Y));
            soundBlock.Add("SoundInstructions", minstructions);

            backmessage = "Press Escape to go back";
        }

        void sound_LeftClickEvent(object sender, EventArgs e)
        {
            if (OptionsState == TowerDefense.OptionsState.Normal)
            {
                Button b = sender as Button;
                b.Texture = activeButtonTexture;
                Vector2 pos = new Vector2((int)(soundBlock.Dimensions.Left + b.Texture.Width), (int)(soundBlock.Dimensions.Top + b.Texture.Height));
                b.Position = pos;
                b.ButtonText.Position = new Vector2(pos.X - (b.Texture.Width / 2) + 10, pos.Y - (b.Texture.Height / 2));
                b.SetColor(Selected);

                soundBlock.GetText("SoundInstructions").Value = "Press the Up or Down arrow keys to change the value.\nClick the button again to deselect.";
                soundBlock.GetText("SoundInstructions").Position = new Vector2(b.Rectangle.Right + 10, b.ButtonText.Position.Y);

                OptionsState = TowerDefense.OptionsState.Sound;
            }
            else if (OptionsState == TowerDefense.OptionsState.Sound)
            {
                Button b = sender as Button;
                b.Texture = inactiveButtonTexture;
                Vector2 pos = new Vector2((int)(soundBlock.Dimensions.Left + b.Texture.Width), (int)(soundBlock.Dimensions.Top + b.Texture.Height));
                b.Position = pos;
                b.ButtonText.Position = new Vector2(pos.X - (b.Texture.Width / 2) + 10, pos.Y - (b.Texture.Height / 2));
                b.SetColor(Normal);

                soundBlock.GetText("SoundInstructions").Value = "Select the Sound button to change settings";
                soundBlock.GetText("SoundInstructions").Position = new Vector2(b.Position.X + (b.Texture.Width / 2) + 10, b.ButtonText.Position.Y);

                OptionsState = TowerDefense.OptionsState.Normal;
            }
        }

        void music_LeftClickEvent(object sender, EventArgs e)
        {
            if (OptionsState == TowerDefense.OptionsState.Normal)
            {
                Button b = sender as Button;
                b.Texture = activeButtonTexture;
                Vector2 pos = new Vector2((int)(musicBlock.Dimensions.Left + b.Texture.Width), (int)(musicBlock.Dimensions.Top + b.Texture.Height));
                b.Position = pos;
                b.ButtonText.Position = new Vector2(pos.X - (b.Texture.Width / 2) + 10, pos.Y - (b.Texture.Height / 2));
                b.SetColor(Selected);

                musicBlock.GetText("MusicInstructions").Value = "Press the Up or Down arrow keys to change the value.\nClick the button again to deselect.";
                musicBlock.GetText("MusicInstructions").Position = new Vector2(b.Rectangle.Right + 10, b.ButtonText.Position.Y);

                OptionsState = TowerDefense.OptionsState.Music;
            }
            else if (OptionsState == TowerDefense.OptionsState.Music)
            {
                Button b = sender as Button;
                b.Texture = inactiveButtonTexture;
                Vector2 pos = new Vector2((int)(musicBlock.Dimensions.Left + b.Texture.Width), (int)(musicBlock.Dimensions.Top + b.Texture.Height));
                b.Position = pos;
                b.ButtonText.Position = new Vector2(pos.X - (b.Texture.Width / 2) + 10, pos.Y - (b.Texture.Height / 2));
                b.SetColor(Normal);

                musicBlock.GetText("MusicInstructions").Value = "Select the Music button to change settings";
                musicBlock.GetText("MusicInstructions").Position = new Vector2(b.Position.X + (b.Texture.Width / 2) + 10, b.ButtonText.Position.Y);

                OptionsState = TowerDefense.OptionsState.Normal;
            }
        }

        protected override void DrawScreen(GameTime gameTime)
        {
            if (OptionsState == TowerDefense.OptionsState.Normal || OptionsState == TowerDefense.OptionsState.Music) musicBlock.Draw(gameTime, ScreenSystem.SpriteBatch, font);
            if (OptionsState == TowerDefense.OptionsState.Normal || OptionsState == TowerDefense.OptionsState.Sound) soundBlock.Draw(gameTime, ScreenSystem.SpriteBatch, font);

            ScreenSystem.SpriteBatch.DrawString(smallfont, backmessage, new Vector2((ScreenSystem.Viewport.Width - smallfont.MeasureString(backmessage).X) / 2, soundBlock.Dimensions.Bottom + smallfont.LineSpacing), Color.White);

            mouse.Draw(ScreenSystem.SpriteBatch);
        }

        protected override void UpdateScreen(GameTime gameTime)
        {
            mouse.Update();

            foreach (var b in musicBlock.Buttons)
            {
                b.Value.Update(gameTime, mouse);
            }

            foreach (var b in soundBlock.Buttons)
            {
                b.Value.Update(gameTime, mouse);
            }

            if (InputMap.NewActionPress("Finished"))
            {
                ExitScreen();
                screenBefore.ActivateScreen();
            }
            if(OptionsState == TowerDefense.OptionsState.Music)
            {
                if (InputMap.NewActionPress("Volume Up") && Settings.MusicVolume < 1.0f)
                {
                    Settings.MusicVolume += 0.05f;
                    AudioManager.singleton.SetVolume("Music", Settings.MusicVolume);
                    musicBlock.GetButton("MusicButton").ButtonText.Value = String.Format("Music:{0}%", (int)(Settings.MusicVolume * 100));
                }

                if (InputMap.NewActionPress("Volume Down") && Settings.MusicVolume > 0)
                {
                    Settings.MusicVolume -= 0.05f;
                    AudioManager.singleton.SetVolume("Music", Settings.MusicVolume);
                    musicBlock.GetButton("MusicButton").ButtonText.Value = String.Format("Music:{0}%", (int)(Settings.MusicVolume * 100));
                }
            }
            else if (OptionsState == TowerDefense.OptionsState.Sound)
            {
                if (InputMap.NewActionPress("Volume Up") && Settings.SoundVolume < 1.0f)
                {
                    Settings.SoundVolume += 0.05f;
                    AudioManager.singleton.SetVolume("Sound", Settings.SoundVolume);
                    soundBlock.GetButton("SoundButton").ButtonText.Value = String.Format("Sound:{0}%", (int)(Settings.SoundVolume * 100));
                    AudioManager.singleton.PlaySound("Hit");
                }

                if (InputMap.NewActionPress("Volume Down") && Settings.SoundVolume > 0)
                {
                    Settings.SoundVolume -= 0.05f;
                    AudioManager.singleton.SetVolume("Sound", Settings.SoundVolume);
                    soundBlock.GetButton("SoundButton").ButtonText.Value = String.Format("Sound:{0}%", (int)(Settings.SoundVolume * 100));
                    AudioManager.singleton.PlaySound("Hit");
                }
            }
        }
    }
}
