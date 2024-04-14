using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class MeeleAutoReuse : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => lateInstantiation && item.CountsAsClass(DamageClass.Melee);
    public override void SetDefaults(Item item)
    {
        item.autoReuse = true;
        item.useTurn = true;
    }
}

