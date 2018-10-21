using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public enum ObjectClickedState
    {
        Normal,
        Clicked,
        Released
    }


    public abstract class ClickableGameplayObject : GameplayObject
    {
        public ObjectClickedState LeftState
        {
            get;
            protected set;
        }

        public ObjectClickedState RightState
        {
            get;
            protected set;
        }

        public Mouse ActiveMouse
        {
            get;
            protected set;
        }

        public ClickableGameplayObject()
            : base()
        {
            LeftState = RightState = ObjectClickedState.Normal;
        }

        public event EventHandler LeftClickEvent;
        public event EventHandler LeftHeldEvent;
        public event EventHandler LeftReleaseEvent;
        public event EventHandler LeftNormalEvent;

        public virtual void OnLeftClick() 
        {
            if (LeftClickEvent != null)
                LeftClickEvent(this, EventArgs.Empty);
        }
        public virtual void OnHeldLeftClick() 
        {
            if (LeftHeldEvent != null)
                LeftHeldEvent(this, EventArgs.Empty);
        }
        public virtual void OnLeftRelease() 
        {
            if (LeftReleaseEvent != null)
                LeftReleaseEvent(this, EventArgs.Empty);
        }
        public virtual void OnLeftNormal() 
        {
            if (LeftNormalEvent != null)
                LeftNormalEvent(this, EventArgs.Empty);
        }

        public event EventHandler RightClickEvent;
        public event EventHandler RightHeldEvent;
        public event EventHandler RightReleaseEvent;
        public event EventHandler RightNormalEvent;

        public virtual void OnRightClick() 
        {
            if (RightClickEvent != null)
                RightClickEvent(this, EventArgs.Empty);
        }
        public virtual void OnHeldRightClick() 
        {
            if (RightHeldEvent != null)
                RightHeldEvent(this, EventArgs.Empty);
        }
        public virtual void OnRightRelease()
        {
            if (RightReleaseEvent != null)
                RightReleaseEvent(this, EventArgs.Empty);
        }
        public virtual void OnRightNormal() 
        {
            if (RightNormalEvent != null)
                RightNormalEvent(this, EventArgs.Empty);
        }

        public event EventHandler Hover;
        public event EventHandler Leave;

        public virtual void OnHover() 
        {
            if (Hover != null)
                Hover(this, EventArgs.Empty);
        }
        public virtual void OnLeave()
        {
            if (Leave != null)
                Leave(this, EventArgs.Empty);
        }

        public void Update(GameTime gameTime, Mouse activeMouse)
        {
            base.Update(gameTime);

            bool intersect = activeMouse.Rectangle.Intersects(Rectangle);

            if ( intersect || (ActiveMouse != null && 
                (LeftState != ObjectClickedState.Normal || RightState != ObjectClickedState.Normal)))
            {
                ActiveMouse = activeMouse;

                if(intersect) OnHover();

                if (ActiveMouse.LeftClick && (LeftState == ObjectClickedState.Normal
                    || LeftState == ObjectClickedState.Released))
                {
                    LeftState = ObjectClickedState.Clicked;
                    OnLeftClick();
                }
                else if (ActiveMouse.LeftClick && LeftState == ObjectClickedState.Clicked)
                {
                    OnHeldLeftClick();
                }
                else if (ActiveMouse.LeftRelease && LeftState == ObjectClickedState.Clicked)
                {
                    LeftState = ObjectClickedState.Released;
                    OnLeftRelease();
                }
                else if (!ActiveMouse.LeftClick && LeftState == ObjectClickedState.Released)
                {
                    LeftState = ObjectClickedState.Normal;
                    OnLeftNormal();
                }

                if (ActiveMouse.RightClick && (RightState == ObjectClickedState.Normal
                    || RightState == ObjectClickedState.Released))
                {
                    RightState = ObjectClickedState.Clicked;
                    OnRightClick();
                }
                else if (ActiveMouse.RightClick && LeftState == ObjectClickedState.Clicked)
                {
                    OnHeldRightClick();
                }
                else if (ActiveMouse.RightRelease && RightState == ObjectClickedState.Clicked)
                {
                    RightState = ObjectClickedState.Released;
                    OnRightRelease();
                }
                else if (!ActiveMouse.RightClick && RightState == ObjectClickedState.Released)
                {
                    RightState = ObjectClickedState.Normal;
                    OnRightNormal();
                }

                if (LeftState != ObjectClickedState.Normal || RightState != ObjectClickedState.Normal)
                    ActiveMouse.ClickedObject = this;
                else if (ActiveMouse.ClickedObject == this)
                    ActiveMouse.ClickedObject = null;
            }

            else
            {
                if (ActiveMouse != null)
                {
                    ActiveMouse.ClickedObject = null;
                    ActiveMouse = null;
                    OnLeave();
                }
            }
        }
    }
}
