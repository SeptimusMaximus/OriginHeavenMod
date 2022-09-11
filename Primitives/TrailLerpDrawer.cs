using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.Graphics;

namespace DarkValley.Primitives
{
	// Token: 0x02000643 RID: 1603
	public struct TrailLerpDrawer
	{
		// Token: 0x060033C1 RID: 13249 RVA: 0x0061AC98 File Offset: 0x00618E98
		public void Draw(Projectile proj)
		{
			float num = proj.ai[1];
			MiscShaderData miscShaderData = GameShaders.Misc["EmpressBlade"];
			int num2 = 1;
			int num3 = 0;
			int num4 = 0;
			float num5 = 0.6f;
			miscShaderData.UseShaderSpecificData(new Vector4((float)num2, (float)num3, (float)num4, num5));
			miscShaderData.Apply(default(DrawData?));
			TrailLerpDrawer._vertexStrip.PrepareStrip(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, new int?(proj.oldPos.Length), true);
			TrailLerpDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0061AD88 File Offset: 0x00618F88
		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(this.ColorStart, this.ColorEnd, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, true));
			result.A /= 2;
			return result;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x0001E66A File Offset: 0x0001C86A
		private float StripWidth(float progressOnStrip)
		{
			return 10f;
		}

		// Token: 0x0400533E RID: 21310
		public const int TotalIllusions = 1;

		// Token: 0x0400533F RID: 21311
		public const int FramesPerImportantTrail = 60;

		// Token: 0x04005340 RID: 21312
		private static VertexStrip _vertexStrip = new VertexStrip();

		// Token: 0x04005341 RID: 21313
		public Color ColorStart;

		// Token: 0x04005342 RID: 21314
		public Color ColorEnd;
	}
}
