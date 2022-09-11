using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DarkValley.Titles
{
    public class Titles : ModPlayer
    {
        public bool text;
        public float alphaText = 255f;
        public float alphaText2 = 255f;
        public float alphaText3 = 255f;
        public float alphaText4 = 255f;
        public int BossID;

        public override void ResetEffects()
        {
            text = false;
        }

        public override void PreUpdate()
        {
            if (!GProjectile.AnyProjectiles(ModContent.ProjectileType<Title>()))
            {
                alphaText = 255f;
                alphaText2 = 255f;
            }
        }
    }
    public class Title : ModProjectile
    {
        public override string Texture => "DarkValley/Blank";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Titles modPlayer = Main.player[Projectile.owner].GetModPlayer<Titles>();
            modPlayer.text = true;
            modPlayer.BossID = (int)Projectile.ai[0];
            Projectile.velocity.X = 0f;
            Projectile.velocity.Y = 0f;
            if (Projectile.timeLeft <= 45)
            {
                if (modPlayer.alphaText < 255f)
                {
                    modPlayer.alphaText += 10f;
                    modPlayer.alphaText2 += 10f;
                }
                return;
            }
            if (Projectile.timeLeft <= 180)
            {
                modPlayer.alphaText -= 5f;
            }
            if (modPlayer.alphaText > 0f)
            {
                modPlayer.alphaText2 -= 5f;
            }
        }
    }
}
