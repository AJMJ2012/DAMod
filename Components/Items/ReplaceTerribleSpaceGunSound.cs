using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DAMod.Components.Items;

public class ReplaceTerribleSpaceGunSound : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.UseSound == SoundID.Item157 && !ModLoader.HasMod("TerrariaOverhaul");
    public override void SetDefaults(Item item)
    {
        item.UseSound = SoundID.Item12;
    }
}

