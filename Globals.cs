using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.ItemDropRules;
using System.Linq;

namespace OriginHeavenMod
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
        
    }
    public class GItem : GlobalItem
    {
       
        
    }
}
