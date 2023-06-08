using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace OriginHeavenMod.Assets.Audio
{
	public class OHSounds : ModSystem
	{
		public static Dictionary<string, SoundStyle> Sound;

        public override void Load()
        {
            Sound = new Dictionary<string, SoundStyle>();
            Sound["SwordWhipWhoosh"] = NewSound("SwordWhipSoundWoosh");
            Sound["Juicy Hit Sound MMM"] = NewSound("CRONCH");
        }

        public override void Unload()
        {
            Sound = null;
        }



        public SoundStyle NewSound(string soundfileName, float pitch = 1f, bool loop = false, int MaxInstances = 0)
        {
            var sound = new SoundStyle("OriginHeavenMod/Assets/Audio/" + soundfileName);
            sound.Pitch = pitch;
            sound.IsLooped = loop;
            sound.MaxInstances = MaxInstances;
            return sound;
        }
    }
}

