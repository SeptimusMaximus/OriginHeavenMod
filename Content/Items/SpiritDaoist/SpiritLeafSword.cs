using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OriginHeavenMod.Content.Items.SpiritDaoist
{
	public class SpiritLeafSword : ModItem
	{
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 13/2;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 1;
            Item.rare = ItemRarityID.Green;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(this.Item), player.position + new Vector2(0,10) + player.velocity, velocity*5 + Main.rand.NextVector2Circular(2, 2), ModContent.ProjectileType<SpiritLeafSword_Projectile>(), damage, 2f, player.whoAmI);
            return false;
        }

    }
}

