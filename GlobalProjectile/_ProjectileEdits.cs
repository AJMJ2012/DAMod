using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class _ProjectileEdits : GlobalProjectile {

		public override void AI(Projectile projectile) {
			// Mute the annoying magic missile
			if (projectile.aiStyle == ProjAIStyleID.MagicMissile) {
				projectile.soundDelay = 60;
			}
		}

		// Nerf Crit Damage
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			if (crit) damage = (int)((float)(damage / 2) * DAMod.critDamageMult);
		}
		public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit) {
			if (crit) damage = (int)((float)(damage / 2) * DAMod.critDamageMult);
		}
	}
}