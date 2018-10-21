using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public class VideoManager
    {
        public VideoPlayer Player
        {
            get;
            private set;
        }

        public Video ActiveVideo
        {
            get;
            private set;
        }

        public string ContentFolder
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public bool IsDonePlaying
        {
            get { return Player.State == MediaState.Stopped; }
        }

        ContentManager Content
        {
            get;
            set;
        }

        public static VideoManager singleton;

        public VideoManager(ContentManager content, string dir)
        {
            Content = content;
            ContentFolder = dir;
            singleton = this;

            Player = new VideoPlayer();
        }

        public void LoadVideo(string videoAsset, Vector2 pos)
        {
            if (Player.State == MediaState.Playing) Player.Stop();
            ActiveVideo = Content.Load<Video>(ContentFolder + videoAsset);
            Position = pos;
        }

        public void Play()
        {
            if(ActiveVideo != null) Player.Play(ActiveVideo);
        }

        public void Stop()
        {
            Player.Stop();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Player.GetTexture(), Position, Color.White);
        }
    }
}
