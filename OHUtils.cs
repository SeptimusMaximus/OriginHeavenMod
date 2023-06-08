using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace OriginHeavenMod
{
	public static class OHUtils
	{
        public static void Screenshake(float strength, Vector2 position, float speed)
        {
            PunchCameraModifier modifier = new PunchCameraModifier(position, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), strength, speed, 20, 1000f, "Patchwork");
            Main.instance.CameraModifiers.Add(modifier);
        }
    }
}

