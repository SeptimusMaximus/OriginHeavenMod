using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using OriginHeavenMod.Assets.Audio;

namespace OriginHeavenMod.Content.Items.SpiritDaoist
{
	public class SpiritLeafSword_Projectile : ModProjectile
	{
        Vector2 velocityOT;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.width = 28;
            Projectile.height = 26;
            Projectile.ArmorPenetration = 5;
        }
        public override void OnSpawn(IEntitySource source)
        {
            velocityOT = Vector2.Zero;
            SoundEngine.PlaySound(OHSounds.Sound["SwordWhipWhoosh"].WithVolumeScale(0.3f).WithPitchOffset(Main.rand.NextFloat(0.8f, 1.1f)));
            Projectile.rotation = Projectile.velocity.ToRotation() + (MathF.PI/4) + Main.rand.NextFloat(-0.1f,0.1f);
        }
        public override void AI()
        {
            Projectile.position = Main.player[Projectile.owner].position + new Vector2(0, 10) + velocityOT;
            if (Main.rand.NextBool(10))
                Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity + Main.rand.NextVector2Circular(2, 2), GoreID.TreeLeaf_Normal, Main.rand.NextFloat(0.9f, 1.2f));
            Projectile.velocity -= (Projectile.velocity * 0.1f);
            velocityOT += Projectile.velocity;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OHUtils.Screenshake(4f, Projectile.Center, 1f);
            Gore.NewGore(Projectile.GetSource_OnHit((Entity)target), target.Center, Projectile.velocity + new Vector2(0,-20), GoreID.TreeLeaf_VanityTreeSakura, 1f);
            SoundEngine.PlaySound(OHSounds.Sound["Juicy Hit Sound MMM"].WithVolumeScale(0.4f).WithPitchOffset(Main.rand.NextFloat(0.4f,1.4f)));

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            Projectile.friendly = false;
            return false;
        }


    }
}

