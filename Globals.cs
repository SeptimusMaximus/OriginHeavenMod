using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using DarkValley.Items;
using DarkValley.Items.Weapons.Melee;
using DarkValley.Items.Weapons.Ranged;
using DarkValley.Items.Weapons.Magic;
using DarkValley.Items.Weapons.Summon;
using DarkValley.Items.Weapons.Throwing;
using DarkValley.Items.Weapons.Assassin.Caltrops;
using DarkValley.Items.Weapons.Assassin.Knives;
using DarkValley.Items.Weapons.Assassin.Scythes;
using DarkValley.Items.Minerals.Golstra;
using DarkValley.Items.Materials;

namespace DarkValley
{
    public class GProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public static bool AnyProjectiles(int type)
        {
            for (int i = 0; i < 1000; i++)
            {
                if ((Main.projectile[i]).active && Main.projectile[i].type == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class GNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeOfBall>(), 5));
            }

            /*if (Array.IndexOf(new int[] { NPCID.WyvernBody, NPCID.WyvernBody2, NPCID.WyvernHead, NPCID.WyvernTail }, npc.type) > -1)
            {

                LeadingConditionRule downedGolem = new(new DownedGolemCondition);
                downedGolem.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AirSlayer>(), 50));
                //leadingConditionRule.OnSuccess(/*Additional rules as new lines);
                npcLoot.Add(downedGolem);
            } commented because i don't know how to fix the bug known as getting items to drop after golem. -Stratis
            */
        }
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            if (Main.hardMode == true && NPC.downedMechBossAny == true)
                globalLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolstraCrystal>(), 1, 16, 32));

        }
    }
    public class GItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.SpikyBall)
            {
                item.StatsModifiedBy.Add(Mod); // Notify the game that we've made a functional change to this item.
                item.DamageType = ModContent.GetInstance<AssassinClass>();
            }
            if (item.type == ItemID.StarCannon)
            {
                item.StatsModifiedBy.Add(Mod);
                item.damage = 60;
                item.rare = ItemRarityID.Pink;
            }
            if (item.type == ItemID.SuperStarCannon)
            {
                item.StatsModifiedBy.Add(Mod);
                item.damage = 75;
                item.rare = ItemRarityID.Yellow;
            }
            // Change damage to 50!
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            
            // In addition to this code, we also do similar code in Common/GlobalNPCs/ExampleNPCLoot.cs to edit the boss loot for non-expert drops. Remember to do both if your edits should affect non-expert drops as well.

            // Boss Drop Edits
            if (item.type == ItemID.EyeOfCthulhuBossBag)
            {
                foreach (var rule in itemLoot.Get())
                {
                    if (rule is OneFromOptionsNotScaledWithLuckDropRule oneFromOptionsDrop && oneFromOptionsDrop.dropIds.Contains(ItemID.DemoniteOre) && oneFromOptionsDrop.dropIds.Contains(ItemID.CrimtaneOre))
                    {
                        var original = oneFromOptionsDrop.dropIds.ToList();
                        original.Add(ModContent.ItemType<EyeOfBall>());
                        oneFromOptionsDrop.dropIds = original.ToArray();
                    }
                }
            }
        }
    }
}
