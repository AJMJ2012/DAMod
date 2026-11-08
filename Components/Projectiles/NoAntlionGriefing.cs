using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Projectiles;

class NoAntlionGriefing : GlobalProjectile
{
	public override bool PreKill(Projectile projectile, int timeLeft)
	{
		if (projectile.type == ProjectileID.SandBallFalling && !projectile.friendly)
		{
			SoundEngine.PlaySound(SoundID.Dig);
			return false;
		}
		return true;
	}
}