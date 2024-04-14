using Terraria;
using Terraria.ModLoader;

using DALib.Checks;

namespace DAMod.Components.Items;

public class ToolsDealGenericDamage : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.IsDestructiveTool();
    public override void SetDefaults(Item item)
    {
        item.DamageType = DamageClass.Generic;
    }
}

