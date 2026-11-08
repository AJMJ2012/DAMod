using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class MagicDaggerAutoReuse : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.type == ItemID.MagicDagger;
    public override void SetDefaults(Item item)
    {
        item.autoReuse = true;
        item.useTime += 2;
        item.useAnimation += 2;
    }
}

