using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Projectiles;

class MuteMagicMissile : GlobalProjectile
{
    public override void AI(Projectile projectile)
    {
        if (projectile.aiStyle == ProjAIStyleID.MagicMissile)
        {
            projectile.soundDelay = 60;
        }
    }
}
