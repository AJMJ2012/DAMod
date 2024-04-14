using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class PowerfulBows : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => lateInstantiation && !item.channel && item.CountsAsClass(DamageClass.Ranged) && (item.useAmmo == AmmoID.Arrow);
    public override void SetDefaults(Item item)
    {
        item.damage = (int)(item.damage * 1.5f);
        //item.knockBack = (int)(item.knockBack * 1.5f);
        item.useAnimation = (int)(item.useAnimation * 1.5f);
        item.useTime = (int)(item.useTime * 1.5f);
    }
}

