using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.UI;
using System.Collections.Generic;
using OrginHeavenMod.Titles;
using ReLogic.Graphics;

namespace OrginHeavenMod
{
	public class TitleSystem : ModSystem
	{

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            Titles.Titles modPlayer = Main.player[Main.myPlayer].GetModPlayer<Titles.Titles>();
            if (modPlayer.text)
            {
                int textLayer = layers.FindIndex((GameInterfaceLayer layer) => layer.Name.Equals("Vanilla: Inventory"));
                LegacyGameInterfaceLayer interfaceLayer = new LegacyGameInterfaceLayer("DarkValley: UI", delegate
                {
                    BossTitle(modPlayer.BossID);
                    return true;
                }, InterfaceScaleType.UI);
                layers.Insert(textLayer, interfaceLayer);
            }
        }
        public void BossTitle(int BossID)
        {
            string BossName = "";
            string BossTitle = "";
            Color titleColor = Color.White;
            switch (BossID)
            {
                case 0:
                    BossName = "BossName";
                    BossTitle = "BossTitle";
                    titleColor = Color.White;
                    break;
                
            }
            Vector2 textSize = FontAssets.DeathText.Value.MeasureString(BossName);
            Vector2 textSize2 = FontAssets.DeathText.Value.MeasureString(BossTitle) * 0.35f;
            float textPositionLeft = (Main.screenWidth / 2) - textSize.X / 2f;
            float text2PositionLeft = (Main.screenWidth / 2) - textSize2.X / 2f;
            Titles.Titles modPlayer = Main.player[Main.myPlayer].GetModPlayer<Titles.Titles>();
            float alpha = modPlayer.alphaText;
            float alpha2 = modPlayer.alphaText2;
            DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.DeathText.Value, BossTitle, new Vector2(text2PositionLeft, (Main.screenHeight / 2 - 350)), titleColor * ((255f - alpha2) / 255f), 0f, Vector2.Zero, 0.6f, 0, 0f);
            DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.DeathText.Value, "- " + BossName + " -", new Vector2(textPositionLeft, (Main.screenHeight / 2 - 300)), titleColor * ((255f - alpha) / 255f), 0f, Vector2.Zero, 1f, 0, 0f);
        }// Boss Title Types

        public static void ShowTitle(NPC npc, int ID)
        {
            if (DarkValleyConfig.Instance.BossTitleText)
            {
                Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Title>(), 0, 0f, Main.myPlayer, ID);
            }
        }
    }
}
