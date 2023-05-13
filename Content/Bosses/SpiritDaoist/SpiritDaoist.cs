using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OriginHeavenMod;
using OriginHeavenMod.Content.Bosses.SpiritDaoist.Projectiles;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace OriginHeavenMod.Content.Bosses.SpiritDaoist
{
	// Re-coded by Vaema.
	// The "Spirit Daoist" is the first boss of the mod, it was made to be simple.

    [AutoloadBossHead]
    public class SpiritDaoist : ModNPC
    {
		private enum SpiritAttackState
        {
			SpiritLeaves,
			SunBeam,
			SpawnShield,
			TreeRootSpikes
        }

		private ref float State => ref NPC.ai[0];
		private ref float Timer => ref NPC.ai[1];

		private bool TitleSpawned;

		private Vector2 LastWarpPosition;

        public override void SetStaticDefaults()
        {
			Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 7;
			NPCID.Sets.TrailCacheLength[NPC.type] = 16;
		}

		public override void SetDefaults()
		{
            NPC.width = 68;
            NPC.height = 74;
            NPC.damage = 15;
            NPC.lifeMax = 1280;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = Item.buyPrice(0, 2, 50);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.lavaImmune = true;
			//NPC.BossBar = ModContent.GetInstance<BattlemageBossBar>();
			Music = MusicID.Boss3;
		}

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
			NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * balance);
			NPC.damage = (int)(NPC.damage * 0.8f * balance);
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
            writer.Write(TitleSpawned);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			TitleSpawned = reader.ReadBoolean();
		}

		public override void AI()
		{
			// Spawn the title on the player's screen.
			//if (!TitleSpawned)
			//{
			//	TitleSystem.ShowTitle(NPC, 4);
			//	TitleSpawned = true;
			//}

			// Make the boss be able to target the closest player if in multiplayer.
            NPC.TargetClosest();
			// Do a net update every frame.
			NPC.netUpdate = true;
			// Call the movement method.
			ProcessMovement();
			// Increment the timer.
			Timer++;

			switch ((SpiritAttackState)(int)State)
            {
				case SpiritAttackState.SpiritLeaves:
					SpiritLeaves();
					break;
            }
		}

		// Attack one: Shoot spirit leaves.
		private void SpiritLeaves()
        {
			// Projectile damage values.
			int projDamage = 16;
			if (Main.dayTime) // The boss "enrages" slightly if it's daytime.
			{
				projDamage = 20;
				if (Main.expertMode)
					projDamage = 36;
				if (Main.masterMode)
					projDamage = 52;
			}
			else // Inflict regular damage values during nighttime.
			{
				if (Main.expertMode)
					projDamage = 24;
				if (Main.masterMode)
					projDamage = 36;
			}

			if (Timer++ >= 5f && Timer <= 325f)
            {
				if (Timer % 75f == 0f)
                {
					for (int i = 0; i < 4; i++)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitX.RotatedBy((float)Math.PI / 2f * i) * 2.5f,
							ModContent.ProjectileType<SpiritLeaf>(), projDamage, 6f);
				}
			}
			
			if (Timer > 325f)
            {
				State = (float)SpiritAttackState.SunBeam;
				Timer = 0f;
				NPC.netUpdate = true;
            }
        }

		// Attack two: Sun beams will descend from the sky.
		private void SunBeams()
        {

        }

		// This method is for teleporting around the player.
		private void WarpAroundPlayer()
		{
			Player player = Main.player[NPC.target];
			Vector2 fixedTrial;
			Vector2 trialTile;
			do
			{
				Vector2 vector = player.Center + Main.rand.NextVector2Circular(150f, 150f) - NPC.frame.Size() / 2f;
				fixedTrial = vector + (vector - player.Center).SafeNormalize(Vector2.UnitX) * 64f - Vector2.UnitY * (NPC.height / 2);
				trialTile = new Vector2(fixedTrial.X / 16f, fixedTrial.Y / 16f);
			}
			while (Main.tile[(int)trialTile.X, (int)trialTile.Y].HasTile || !(player.DistanceSQ(fixedTrial) > 4096f));
				NPC.Teleport(fixedTrial, 1);

			LastWarpPosition = fixedTrial;
		}

		// This method is for teleporting away from a previous position.
		private void WarpAwayFromLastPos(float minDist)
		{
			Player player = Main.player[NPC.target];
			minDist = (float)Math.Pow(minDist, 2.0);
			Vector2 fixedTrial;
			while (true)
			{
				Vector2 vector = player.Center + Main.rand.NextVector2Circular(150f, 150f) - NPC.frame.Size() / 2f;
				fixedTrial = vector + (vector - player.Center).SafeNormalize(Vector2.UnitX) * 64f - Vector2.UnitY * (NPC.height / 2);
				if ((fixedTrial - LastWarpPosition).LengthSquared() > minDist)
				{
					Vector2 trialTile = new Vector2(fixedTrial.X / 16f, fixedTrial.Y / 16f);
					if (!Main.tile[(int)trialTile.X, (int)trialTile.Y].HasTile && player.DistanceSQ(fixedTrial) > 4096f)
						break;
				}
			}

            NPC.Teleport(fixedTrial, 1);
			LastWarpPosition = fixedTrial;
		}
		
		// The movement method.
		private void ProcessMovement()
		{
			_ = Main.player[NPC.target];

			if (State != 0f)
			{
				if (State == 1f && Timer % 60f == 0f)
					WarpAwayFromLastPos(64f);
			}
			else if (Timer == 0f)
				WarpAroundPlayer();

			NPC.velocity.Y = (float)Math.Sin(Timer / 4);
		}

		public override void BossLoot(ref string name, ref int potionType)
        {
			potionType = ItemID.LesserHealingPotion;
        }

        public override void OnKill()
        {
            
        }
    }
}