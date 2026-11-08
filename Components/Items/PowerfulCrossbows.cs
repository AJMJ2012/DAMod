using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class PowerfulCrossbows : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => lateInstantiation && !item.channel && item.CountsAsClass(DamageClass.Ranged) && (item.Name.Contains("Repeater") || item.Name.Contains("Crossbow") || item.type == ItemID.ChlorophyteShotbow || item.type == ItemID.StakeLauncher);
    public override void SetDefaults(Item item)
    {
        item.damage = (int)(item.damage * 2f);
        //item.knockBack = (int)(item.knockBack * 2f);
        item.useAnimation = (int)(item.useAnimation * 2f);
        item.useTime = (int)(item.useTime * 2f);
    }
}

