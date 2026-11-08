using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class NerfCritDamage : GlobalNPC
{
	public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
	{
		if (Config.Server.NerfCritDamage)
		{
			modifiers.CritDamage *= 0.75f;
		}
	}
}