using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public class AudioManager
    {
        public AudioEngine Engine
        {
            get;
            private set;
        }

        public WaveBank WaveBank
        {
            get;
            private set;
        }

        public SoundBank SoundBank
        {
            get;
            private set;
        }

        public Cue CurrentSong
        {
            get;
            private set;
        }

        public Cue CurrentSound
        {
            get;
            private set;
        }

        public static AudioManager singleton;

        public AudioManager(AudioEngine e, WaveBank wb, SoundBank sb)
        {
            Engine = e;
            WaveBank = wb;
            SoundBank = sb;
            singleton = this;
        }

        public void PlaySong(string cueName)
        {
            if (CurrentSong != null) CurrentSong.Stop(AudioStopOptions.Immediate);
            CurrentSong = SoundBank.GetCue(cueName);
            CurrentSong.Play();
        }

        public void PlaySound(string cueName)
        {
            try
            {
                CurrentSound = SoundBank.GetCue(cueName);
                CurrentSound.Play();
            }
            catch (InstancePlayLimitException iple) { }
        }

        public void Update()
        {
            if (CurrentSong != null && CurrentSong.IsStopped)
            {
                CurrentSong = SoundBank.GetCue(CurrentSong.Name);
                CurrentSong.Play();
            }
        }

        public void SetVolume(string categoryName, float volume)
        {
            volume = MathHelper.Clamp(volume, 0, 1);
            Engine.GetCategory(categoryName).SetVolume(volume);
        }
    }
}
