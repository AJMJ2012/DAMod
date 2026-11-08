using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

public class CloudToSummonDamage : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation)
    {
        if (item.shoot > ProjectileID.None)
        {
            Projectile projectile = new();
            projectile.SetDefaults(item.shoot);
            return projectile.aiStyle == ProjAIStyleID.RainCloud;
        }
        return false;
    }
    public override void SetDefaults(Item item)
    {
        item.DamageType = DamageClass.Summon;
    }
}

