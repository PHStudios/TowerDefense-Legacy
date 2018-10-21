using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseEngine
{
    public class MapRegion
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

        public Texture2D ValidPlacement
        {
            get;
            private set;
        }

        public Texture2D InvalidPlacement
        {
            get;
            private set;
        }

        public Texture2D SpawnPlacement
        {
            get;
            private set;
        }

        public Texture2D MonsterHealthDisplay
        {
            get;
            private set;
        }

        public Tower SelectedActiveTower
        {
            get;
            private set;
        }

        public MapRegion(Session s, Rectangle r, GraphicsDevice gd)
        {
            Session = s;
            Rectangle = r;

            ValidPlacement = new Texture2D(gd, 1, 1);
            Color[] c = new Color[1];
            c[0] = Session.Map.ForeColor;
            ValidPlacement.SetData<Color>(c);

            InvalidPlacement = new Texture2D(gd, 1, 1);
            c[0] = Color.Red;
            InvalidPlacement.SetData<Color>(c);

            SpawnPlacement = new Texture2D(gd, 1, 1);
            c[0] = Color.Blue;
            SpawnPlacement.SetData<Color>(c);

            MonsterHealthDisplay = new Texture2D(gd, 1, 1);
            c[0] = Session.Map.ForeColor;
            MonsterHealthDisplay.SetData<Color>(c);

            Session.TowerPurchased += new TowerDefenseEngine.Session.PurhcaseTowerEventHandler(Session_TowerPurchased);
            Session.TowerSold += new TowerDefenseEngine.Session.SellTowerEventHandler(Session_TowerSold);

        }

        void Session_TowerSold(object sender, TowerEventArgs ptea)
        {
            if (ptea.t == SelectedActiveTower) SelectedActiveTower = null;
        }

        void Session_TowerPurchased(object sender, TowerEventArgs ptea)
        {
            ptea.t.LeftClickEvent += new EventHandler(t_LeftClickEvent);
        }

        void t_LeftClickEvent(object sender, EventArgs e)
        {
            Tower t = sender as Tower;
            if(t.PlacedTime > 1)
                SelectedActiveTower = t;
        }


        public void Update(GameTime gameTime)
        {
            if (Session.UI.mouse.NewLeftClick && Rectangle.Intersects(Session.UI.mouse.Rectangle) && Session.SelectedTower != null)
            {
                Point mousePointerInMap = Session.Map.ToMapCoordinates((int)Session.UI.mouse.Position.X, (int)Session.UI.mouse.Position.Y);

                if (Session.Map.IsValidPlacement(mousePointerInMap.X, mousePointerInMap.Y))
                {
                    Session.PurchaseTower(Session.SelectedTower.Clone(), mousePointerInMap);
                    Session.DeselectTower();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            int tilecode;
            for (int y = 0; y < Session.Map.Dimensions.Y; y++)
            {
                for (int x = 0; x < Session.Map.Dimensions.X; x++)
                {
                    tilecode = Session.Map.GroundTiles[(y * Session.Map.Dimensions.X) + x];
                    int tcy = tilecode / Session.Map.NumberOfTilesInSheet.X;
                    int tcx = tilecode - (tcy * Session.Map.NumberOfTilesInSheet.X);
                    spriteBatch.Draw(Session.Map.TileSheet, new Vector2(x * Session.Map.TileDimensions.X, y * Session.Map.TileDimensions.Y), new Rectangle(tcx * Session.Map.TileDimensions.X, tcy * Session.Map.TileDimensions.Y, 64, 64), Color.White);
                }
                for (int i = 0; i < Session.Map.ActivePath.ShortestPath.path.Count; i++)
                {
                    Tile t = Session.Map.ActivePath.ShortestPath.path[i];
                    tilecode = t.TileCode;
                    if (tilecode > 0)
                    {
                        int tcy = tilecode / Session.Map.NumberOfTilesInSheet.X;
                        int tcx = tilecode - (tcy * Session.Map.NumberOfTilesInSheet.X);
                        spriteBatch.Draw(Session.Map.TileSheet, new Vector2(t.MapLocation.X * Session.Map.TileDimensions.X, t.MapLocation.Y * Session.Map.TileDimensions.Y), new Rectangle(tcx * Session.Map.TileDimensions.X, tcy * Session.Map.TileDimensions.Y, 64, 64), Color.White);
                    }
                }
            }

            foreach (Monster m in Session.Map.ActiveWave.Monsters)
            {
                m.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(MonsterHealthDisplay, new Rectangle(m.Rectangle.Left, m.Rectangle.Top - 4, (int)(m.Rectangle.Width * ((float)m.Health / (float)m.MaxHealth)), 2), Color.White);

            }

            Session.Map.Spawn.Draw(gameTime, spriteBatch);
            Session.Map.Castle.Draw(gameTime, spriteBatch);

            if (SelectedActiveTower != null)
            {
                SelectedActiveTower.DrawRadius(gameTime, spriteBatch);
            }

            foreach (Tower t in Session.ActivePlayer.PlacedTowers)
            {
                t.Draw(gameTime, spriteBatch);
            }

            if (/*Rectangle.Intersects(Session.UI.mouse.Rectangle) && */Session.SelectedTower != null)
            {
                for (int y = 0; y < Session.Map.Dimensions.Y; y++)
                {
                    for (int x = 0; x < Session.Map.Dimensions.X; x++)
                    {
                        if(Session.Map.IsValidPlacement(x, y))
                        {
                            spriteBatch.Draw(ValidPlacement, new Rectangle(x * Session.Map.TileDimensions.X, y * Session.Map.TileDimensions.Y, Session.Map.TileDimensions.X, Session.Map.TileDimensions.Y), Color.White * 0.5f);  //Allow for dynamic setting
                        }
                        else
                        {
                            if (Session.Map.SpawnLocation.X == x && Session.Map.SpawnLocation.Y == y)
                            {
                                spriteBatch.Draw(SpawnPlacement, new Rectangle(x * Session.Map.TileDimensions.X, y * Session.Map.TileDimensions.Y, Session.Map.TileDimensions.X, Session.Map.TileDimensions.Y), Color.White * 0.5f);  //Allow for dynamic setting
                            }
                            else
                            {
                                spriteBatch.Draw(InvalidPlacement, new Rectangle(x * Session.Map.TileDimensions.X, y * Session.Map.TileDimensions.Y, Session.Map.TileDimensions.X, Session.Map.TileDimensions.Y), Color.White * 0.5f);  //Allow for dynamic setting
                            }
                        }
                    }

                }

                Point mousePointerInMap = Session.Map.ToMapCoordinates((int)Session.UI.mouse.Position.X, (int)Session.UI.mouse.Position.Y);
                Point mousePointerInWorld = Session.Map.ToWorldCoordinates(mousePointerInMap.X, mousePointerInMap.Y);

                if (Session.Map.IsValidPlacement(mousePointerInMap.X, mousePointerInMap.Y))
                {
                    spriteBatch.Draw(Session.SelectedTower.Texture, new Vector2(mousePointerInWorld.X, mousePointerInWorld.Y), Color.White * 0.5f);
                }
            }
        }

        internal void ResetTowerReferences()
        {
            SelectedActiveTower = null;
        }
    }
}
