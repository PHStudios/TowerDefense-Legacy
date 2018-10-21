using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefenseEngine
{

    public enum MousePointerLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    /// <summary>
    /// Class to handle mouse presses and drawing
    /// </summary>
    public class Mouse
    {
        /// <summary>
        /// Handles the states of the Mouse.  Both the current state (this game loop) and the previous
        /// state (the previous game loop).
        /// </summary>
        MouseState previousState, currentState;

        /// <summary>
        /// The X, Y screen coordinates of the mouse
        /// </summary>
        public Vector2 Position
        {
            get;
            protected set;
        }

        /// <summary>
        /// The texture we want to use when drawing the mouse and calculating its rectangle
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The rectangular bounds of the mouse object
        /// </summary>
        public Rectangle Rectangle
        {
            get;
            protected set;
        }

        public MousePointerLocation PointerLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Is the left button pressed?
        /// </summary>
        public bool LeftClick
        {
            get { return currentState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Is the left button pressed now, but not previously?
        /// </summary>
        public bool NewLeftClick
        {
            get { return currentState.LeftButton == ButtonState.Pressed 
                && previousState.LeftButton == ButtonState.Released; }
        }

        /// <summary>
        /// Was the left button pressed previously, but not now?
        /// </summary>
        public bool LeftRelease
        {
            get { return !LeftClick && previousState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Is the right button pressed?
        /// </summary>
        public bool RightClick
        {
            get { return currentState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Is the right button pressed now, but not previously?
        /// </summary>
        public bool NewRightClick
        {
            get
            {
                return currentState.RightButton == ButtonState.Pressed
                    && previousState.RightButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Was the right button pressed previously, but not now?
        /// </summary>
        public bool RightRelease
        {
            get { return !RightClick && previousState.RightButton == ButtonState.Pressed; }
        }

        public bool NormalState
        {
            get { return !LeftClick && !LeftRelease && !RightClick && !RightRelease; }
        }

        public ClickableGameplayObject ClickedObject
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an empty mouse object.  
        /// </summary>
        public Mouse() { Position = new Vector2(-500, -500); }

        /// <summary>
        /// Creates a mouse object with the texture we provide.
        /// </summary>
        /// <param name="texture">The texture we wish to use for our mouse object</param>
        public Mouse(Texture2D texture)
        {
            this.texture = texture;
            Position = new Vector2(-500, -500);
        }

        /// <summary>
        /// Creates a new mouse object and loads the texture into memory
        /// </summary>
        /// <param name="content">The ContentManager object our game is using</param>
        /// <param name="assetName">The asset name of our texture</param>
        public Mouse(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
            Position = new Vector2(-500, -500);
        }

        /// <summary>
        /// Gets a new mouse state and changes the position and rectangle
        /// </summary>
        public void Update()
        {
            previousState = currentState;
            currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            Position = new Vector2(currentState.X, currentState.Y);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 2, 2);
        }

        /// <summary>
        /// Draws the object to the screen.  
        /// </summary>
        /// <param name="spriteBatch">The spritebatch object we want to use to draw the object.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }
    }
}
