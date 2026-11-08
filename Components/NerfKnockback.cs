using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class NerfKnockback : GlobalNPC
{
	public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
	{
		if (Config.Server.NerfKnockback)
		{
			modifiers.Knockback *= 0.75f;
		}
	}
}

class NerfKnockbackPlayer : ModPlayer
{
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
	{
		if (Config.Server.NerfKnockback)
		{
			modifiers.Knockback *= 0.75f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
	{
		if (Config.Server.NerfKnockback)
		{
			modifiers.Knockback *= 0.75f;
		}
	}
}