using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class FasterNailgun : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => lateInstantiation && !item.channel && item.type == ItemID.NailGun;
    public override void SetDefaults(Item item)
    {
        item.damage = (int)(item.damage / 2f);
        item.knockBack = (int)(item.knockBack / 2f);
        item.useAnimation = (int)(item.useAnimation / 2f);
        item.useTime = (int)(item.useTime / 2f);
    }
}

