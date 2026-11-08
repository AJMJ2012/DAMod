using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DAMod.Components.Items;

public class FasterThrowAnimations : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation)
    {
        if (item.shoot > ProjectileID.None)
        {
            Projectile projectile = new();
            projectile.SetDefaults(item.shoot);
            return item.useStyle == ItemUseStyleID.Shoot && item.noUseGraphic && !item.channel && projectile.aiStyle != ProjAIStyleID.Spear;
        }
        return false;
    }
    public override void SetDefaults(Item item)
    {
        item.useStyle = ItemUseStyleID.Swing;
        int newAnimation = Math.Min(item.useAnimation, 15);
        item.reuseDelay += item.useAnimation - newAnimation;
        item.useAnimation = newAnimation;
    }
}

