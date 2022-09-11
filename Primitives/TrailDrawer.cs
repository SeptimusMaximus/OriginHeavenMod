using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.Graphics;

namespace DarkValley.Primitives
{
	// Token: 0x02000643 RID: 1603
	public struct TrailDrawer
	{
		// Token: 0x060033C1 RID: 13249 RVA: 0x0061AC98 File Offset: 0x00618E98
		public void Draw(Projectile proj)
		{
			float num = proj.ai[1];
			MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
			int num2 = 1;
			int num3 = 0;
			int num4 = 0;
			float num5 = 0.6f;
			miscShaderData.UseShaderSpecificData(new Vector4((float)num2, (float)num3, (float)num4, num5));
			miscShaderData.Apply(default(DrawData?));
			TrailDrawer._vertexStrip.PrepareStrip(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.SolidColor), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, new int?(proj.oldPos.Length), true);
			TrailDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0061AD88 File Offset: 0x00618F88
		private Color SolidColor(float progressOnStrip)
		{
			Color result = Solid;
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

		public Color Solid;

	}
}
