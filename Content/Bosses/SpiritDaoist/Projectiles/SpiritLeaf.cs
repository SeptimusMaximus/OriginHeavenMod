using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace OriginHeavenMod.NPCs.Bosses.SpiritDaoist.Projectiles
{
	public class SpiritLeaf : ModProjectile
	{
		private const int DETECTION_THRESHOLD = 480;
		private Vector2 finalVelocity;

		public override string GlowTexture => "OriginHeavenMod/NPCs/Bosses/SpiritDaoist/Projectiles/SpiritFlare";

		private int Target
		{
			get
			{
				return (int)Projectile.ai[0];
			}
			set
			{
                Projectile.ai[0] = value;
			}
		}

		private int State
		{
			get
			{
				return (int)Projectile.ai[0];
			}
			set
			{
                Projectile.ai[0] = value;
			}
		}

		private bool State1 => State == 0;

		private bool State2 => State == 1;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
		}

		public override void SetDefaults()
		{
            Projectile.height = 11;
            Projectile.width = 32;
            Projectile.aiStyle = -1;
            Projectile.damage = 24;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
		}

        public void DetectPlayer()
		{
			int closest = -1;
			if (State2)
			{
				float lastDistance = 0f;
				for (int i = 0; i < Main.player.Length; i++)
				{
					float distance = Main.player[i].DistanceSQ(Projectile.Center);
					if ((double)distance <= Math.Pow(480.0, 2.0) && (distance < lastDistance || lastDistance == 0f))
					{
						lastDistance = distance;
						closest = Main.player[i].whoAmI;
					}
				}
			}
			Target = closest;
		}

		private void SetVelocity()
		{
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 5f;
		}

		public void TryApproachTarget()
		{
			if (Target != -1 && Projectile.frameCounter < 150)
			{
				Player player = Main.player[Target];
				finalVelocity = -(Projectile.Center - player.Center).SafeNormalize(Vector2.UnitX) * Projectile.velocity.Length() * 1.04f;
			}

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, finalVelocity, 0.25f);
			
			if (State1)
				SetVelocity();
		}

		private void IncrementFrameCounter()
		{
            Projectile.frameCounter++;
		}

		private void ChangeState()
		{
			if (Projectile.frameCounter > 145)
				State = 1;
		}

		private void IfStartedSetFinalVelocity()
		{
			if (Projectile.frameCounter == 0)
				finalVelocity = Projectile.velocity;
		}

		private void FixRotation()
		{
            Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void AI()
		{
			FixRotation();
			IfStartedSetFinalVelocity();
			IncrementFrameCounter();
			ChangeState();
			DetectPlayer();
			TryApproachTarget();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
			RasterizerState save = graphicsDevice.RasterizerState;
			graphicsDevice.RasterizerState = RasterizerState.CullNone;
			VertexStrip vertexStrip = new();
			MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(6f);
			miscShaderData.Apply();
			vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, TrailColor, StripWidth, -Main.screenPosition + Projectile.Size / 2f);
			vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			graphicsDevice.RasterizerState = save;
			return true;
		}

		private Color TrailColor(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.Blue, Color.Pink, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, clamped: true));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 1f;
			float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 50f, num);
		}
	}
}