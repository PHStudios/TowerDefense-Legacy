using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerDefense.MenuEntries;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense
{
    public class GameOverScreen : MenuScreen
    {
        MainMenuEntry play, quit;

        string prevEntry, nextEntry, selectedEntry, cancelMenu;
        public override string PreviousEntryActionName
        {
            get { return prevEntry; }
        }

        public override string NextEntryActionName
        {
            get { return nextEntry; }
        }

        public override string SelectedEntryActionName
        {
            get { return selectedEntry; }
        }

        public override string MenuCancelActionName
        {
            get { return cancelMenu; }
        }

        LevelSelectionScreen levelselect;

        public GameOverScreen(LevelSelectionScreen lss)
        {
            prevEntry = "MenuUp";
            nextEntry = "MenuDown";
            selectedEntry = "MenuAccept";
            cancelMenu = "MenuCancel";

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            Selected = Highlighted = new Color(214, 232, 223);
            Normal = new Color(104, 173, 178);

            levelselect = lss;
        }


        public override void InitializeScreen()
        {
            InputMap.NewAction(PreviousEntryActionName, Keys.Up);
            InputMap.NewAction(NextEntryActionName, Keys.Down);
            InputMap.NewAction(SelectedEntryActionName, Keys.Enter);
            InputMap.NewAction(MenuCancelActionName, Keys.Escape);

            play = new MainMenuEntry(this, "Again?", 
                "GAME OVER - PLAY THE GAME AGAIN?");
            quit = new MainMenuEntry(this, "Quit", 
                "DONE PLAYING FOR NOW?");

            Removing += new EventHandler(GameOverRemoving);
            Entering += new TransitionEventHandler(
                MainMenuScreen_Entering);
            Exiting += new TransitionEventHandler(
                MainMenuScreen_Exiting);

            play.Selected += new EventHandler(PlaySelect);
            quit.Selected += new EventHandler(QuitSelect);

            MenuEntries.Add(play);
            MenuEntries.Add(quit);

            Viewport view = ScreenSystem.Viewport;
            SetDescriptionArea(new Rectangle(100, view.Height - 
                100,
                view.Width - 100, 50), new Color(11, 38, 40), 
                new Color(29, 108, 117),
                new Point(10, 0), 0.5f);

        }

        public override void LoadContent()
        {
            
            ContentManager content = ScreenSystem.Content;
            SpriteFont = content.Load<SpriteFont>(@"Fonts/menu");

            Texture2D entryTexture = 
                content.Load<Texture2D>(
@"Textures/Menu/MenuEntries");

            BackgroundTexture = content.Load<Texture2D>(
@"Textures/Menu/MainMenuBackground");
            
            //Loads the title texture.
            TitleTexture = content.Load<Texture2D>(
@"Textures/Menu/LogoWithText");

            //Sets the title position
            InitialTitlePosition = TitlePosition = 
                new Vector2((ScreenSystem.Viewport.Width - 
                    TitleTexture.Width) / 2, 50);

            //Initialize is called before LoadContent, so if you want to 
            //use relative position with the line spacing like below,
            //you need to do it after load content and spritefont
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].AddTexture(entryTexture, 2, 1,
                    new Rectangle(0, 0, entryTexture.Width / 2, 
                        entryTexture.Height),
                    new Rectangle(entryTexture.Width / 2, 0, 
                        entryTexture.Width / 2, 
                        entryTexture.Height));
                MenuEntries[i].AddPadding(14, 0);

                if (i == 0)
                    MenuEntries[i].SetPosition(new Vector2(180, 
                        250), true);
                else
                {
                    int offsetY = MenuEntries[i - 1].EntryTexture 
                        == null ? SpriteFont.LineSpacing
                        : 8;
                    MenuEntries[i].SetRelativePosition(
                        new Vector2(0, offsetY), 
                        MenuEntries[i - 1], true);
                }
            }
        }

        public override void UnloadContent()
        {
            SpriteFont = null;
        }

        void MainMenuScreen_Entering(object sender, 
            TransitionEventArgs tea)
        {
            float effect = (float)Math.Pow(tea.percent - 1, 2) 
                * -100;
            foreach (MenuEntry entry in MenuEntries)
            {
                entry.Acceleration = new Vector2(effect, 0);
                entry.Position = entry.DefaultPosition + 
                    entry.Acceleration;
                entry.Opacity = tea.percent;
            }

            TitlePosition = InitialTitlePosition + 
                new Vector2(0, effect);
            TitleOpacity = tea.percent;
        }

        void MainMenuScreen_Exiting(object sender, 
            TransitionEventArgs tea)
        {
            float effect = (float)Math.Pow(tea.percent - 1, 2) 
                * 100;
            foreach (MenuEntry entry in MenuEntries)
            {
                entry.Acceleration = new Vector2(effect, 0);
                entry.Position = entry.DefaultPosition + 
                    entry.Acceleration;
                entry.Scale = tea.percent;
                entry.Opacity = tea.percent;
            }

            TitlePosition = InitialTitlePosition - 
                new Vector2(0, effect);
            TitleOpacity = tea.percent;
        }

        void PlaySelect(object sender, EventArgs e)
        {
            ExitScreen();
            levelselect.ActivateScreen();
        }

        void QuitSelect(object sender, EventArgs e)
        {
            ScreenSystem.Game.Exit();
        }


        void GameOverRemoving(object sender, EventArgs e)
        {
            MenuEntries.Clear();
        }
    }
}
