using Terraria;
using Terraria.ModLoader;

using DALib.Checks;

namespace DAMod.Components.Items;

public class ToolsHitOnce : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.IsDestructiveTool();
    public override void SetDefaults(Item item)
    {
        item.useAnimation = (int)(item.useAnimation * 0.666f);
		item.useTime = item.useAnimation;
    }
}

