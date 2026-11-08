using Terraria;
using Terraria.ModLoader;

using DALib.Checks;

namespace DAMod.Components.Items;

public class IncreaseAmmoStack : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.IsNormalAmmo();
    public override void SetDefaults(Item item)
    {
        item.maxStack = 9999;
    }
}

