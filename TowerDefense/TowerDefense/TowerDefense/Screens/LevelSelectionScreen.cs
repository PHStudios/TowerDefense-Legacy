using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using TowerDefenseEngine;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class LevelSelectionScreen : GameScreen
    {
        MapLoader ml;
        Texture2D background, pixel;
        Map selectedMap, previousMap, nextMap;
        Point center;
        Rectangle border, left, selected, right, description;
        float backgroundOpacity;

        SpriteFont main, levelinfosmall, levelinfolarge;

        Text selectText, numberOfLevelsText;

        int index;

        TimeSpan selectTime;

        float selectTimeSeconds;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        public override void InitializeScreen()
        {
            //CHANGE FOR DYNAMIC RESOLUTION
            center = new Point(1280 / 2, 720 / 2);
            border = new Rectangle(center.X - 608, center.Y - 300, 1216, 580);
            selected = new Rectangle((border.X + (border.Width / 2)) - (480 / 2),
                (border.Y + (border.Height / 2)) - (270 / 2), 480, 270);
            left = new Rectangle((border.X + (border.Width / 2)) - (selected.Width / 2) - 20 - 240,
                (border.Y + (border.Height / 2)) - (135 / 2), 240, 135);
            right = new Rectangle((border.X + (border.Width / 2)) + (selected.Width / 2) + 20,
                (border.Y + (border.Height / 2)) - (135 / 2), 240, 135);
            description = new Rectangle(border.X, border.Bottom + 10, border.Width, 50);


            backgroundOpacity = 0.5f;

            index = 0;

            InputMap.NewAction("Select Left", Microsoft.Xna.Framework.Input.Keys.Left);
            InputMap.NewAction("Select Right", Microsoft.Xna.Framework.Input.Keys.Right);
            InputMap.NewAction("Select Map", Microsoft.Xna.Framework.Input.Keys.Enter);
            InputMap.NewAction("Back", Microsoft.Xna.Framework.Input.Keys.Escape);

            selectTimeSeconds = 1;
            selectTime = TimeSpan.FromSeconds(selectTimeSeconds);

            Removing += new EventHandler(LevelSelectionScreen_Removing);
        }

        public override void LoadContent()
        {
            main = ScreenSystem.Content.Load<SpriteFont>(@"Fonts/menu");
            levelinfosmall = ScreenSystem.Content.Load<SpriteFont>(@"Fonts/LevelSelectSmall");
            levelinfolarge = ScreenSystem.Content.Load<SpriteFont>(@"Fonts/LevelSelectLarge");
            selectText = new Text("Select a map", main);
            selectText.Position = new Vector2(border.X, border.Y - selectText.Dimensions.Y);

            ml = ScreenSystem.Content.Load<MapLoader>("Maps\\MapLoader");
            numberOfLevelsText = new Text(String.Format("[{0} maps]", ml.Maps.Count), main);
            numberOfLevelsText.Position = new Vector2(border.X + selectText.Dimensions.X + 10,
                border.Y - numberOfLevelsText.Dimensions.Y);
            previousMap = null;
            selectedMap = ml.Maps[index];
            nextMap = (index + 1) < ml.Maps.Count ? ml.Maps[index + 1] : null;

            MapLoader.Singleton = ml;

            background = ScreenSystem.Content.Load<Texture2D>(@"Textures/Menu/MainMenuBackground");
            pixel = new Texture2D(ScreenSystem.GraphicsDevice, 1, 1);
            Color[] cArray = new Color[] { Color.Black };
            pixel.SetData<Color>(cArray);
        }

        protected override void UpdateScreen(GameTime gameTime)
        {
            if (InputMap.NewActionPress("Select Left"))
            {
                if (previousMap != null)
                {
                    nextMap = selectedMap;
                    selectedMap = previousMap;

                    index--;

                    previousMap = (index - 1) >= 0 ? ml.Maps[index - 1] : null;
                }
            }
            else if (InputMap.NewActionPress("Select Right"))
            {
                if (nextMap != null)
                {
                    previousMap = selectedMap;
                    selectedMap = nextMap;

                    index++;

                    nextMap = (index + 1) < ml.Maps.Count ? ml.Maps[index + 1] : null;
                }
            }
            else if (InputMap.NewActionPress("Select Map"))
            {
                if (selectedMap != null)
                {
                    FreezeScreen();
                    selectedMap.Reset();
                    ScreenSystem.AddScreen(new PlayScreen(this, selectedMap));
                }
            }
            else if (InputMap.NewActionPress("Back"))
            {
                ExitScreen();
                ScreenSystem.AddScreen(new MainMenuScreen());
            }
            
        }

        void LevelSelectionScreen_Removing(object sender, EventArgs e)
        {

        }

        protected override void DrawScreen(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;

            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(main, selectText.Value, selectText.Position, Color.White);
            spriteBatch.DrawString(main, numberOfLevelsText.Value, numberOfLevelsText.Position, Color.Orange);
            spriteBatch.Draw(pixel, border, Color.White * backgroundOpacity);
            DrawMapInformation(spriteBatch, previousMap, left, Color.White, levelinfosmall);
            DrawMapInformation(spriteBatch, nextMap, right, Color.White, levelinfosmall);
            DrawMapInformation(spriteBatch, selectedMap, selected, Color.White, levelinfolarge);
            spriteBatch.Draw(pixel, description, Color.White * backgroundOpacity);
            spriteBatch.DrawString(levelinfolarge, selectedMap.Description, new Vector2(description.X + 10, description.Y + (description.Height / 2) - 
                (levelinfolarge.MeasureString(selectedMap.Description).Y / 2)), Color.White);
        }

        private void DrawMapInformation(SpriteBatch sb, Map m, Rectangle r, Color c, SpriteFont sf)
        {
            if (m != null)
            {
                sb.DrawString(sf, m.Name, new Vector2(r.X, r.Y - sf.MeasureString(m.Name).Y), c);
                sb.Draw(m.Thumbnail, r, c);

                Vector2 position = new Vector2(r.X, r.Bottom);
                sb.DrawString(sf, m.TowersInfo, position, c);

                position.Y += sf.MeasureString(m.TowersInfo).Y;
                sb.DrawString(sf, m.WavesInfo, position, c);

                position.Y += sf.MeasureString(m.WavesInfo).Y;
                sb.DrawString(sf, m.DifficultyInfo, position, c);

            }
        }
    }
}
