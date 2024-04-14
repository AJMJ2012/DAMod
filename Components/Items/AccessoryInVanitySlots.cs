using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class AccessoryInVanitySlots : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.accessory;
    public override void SetDefaults(Item item)
    {
        item.hasVanityEffects = true;
    }
}

