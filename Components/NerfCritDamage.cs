using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DAMod.Components;

class NerfCritDamage : ModSystem
{
	static readonly MethodInfo NPC_StrikeNPC_Method = typeof(NPC).GetMethod("StrikeNPC", BindingFlags.Public | BindingFlags.Instance);
    delegate double NPC_StrikeNPC_Delegate(NPC self, int Damage, float knockBack, int hitDirection, bool crit, bool noEffect, bool fromNet);
    static readonly MethodInfo Player_Hurt_Method = typeof(Player).GetMethod("Hurt", BindingFlags.Public | BindingFlags.Instance);
    delegate double Player_Hurt_Delegate(Player self, PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter);

    public override void Load()
	{
		HookEndpointManager.Modify<NPC_StrikeNPC_Delegate>(NPC_StrikeNPC_Method, IL_NPC_StrikeNPC);
		HookEndpointManager.Modify<Player_Hurt_Delegate>(Player_Hurt_Method, IL_Player_Hurt);
    }

	public override void Unload()
	{
		HookEndpointManager.Unmodify<NPC_StrikeNPC_Delegate>(NPC_StrikeNPC_Method, IL_NPC_StrikeNPC);
        HookEndpointManager.Unmodify<Player_Hurt_Delegate>(Player_Hurt_Method, IL_Player_Hurt);
    }

	static void IL_NPC_StrikeNPC(ILContext il)
	{
		ILCursor c = new(il);
		/*
			// if (crit)
			IL_0080: ldarg.s crit
			IL_0082: brfalse.s IL_0090

			// num *= 2.0;
			IL_0084: ldloc.1
			IL_0085: ldc.r8 2
			IL_008e: mul
			IL_008f: stloc.1
		*/
		c.GotoNext(MoveType.After,
			i => i.MatchLdarg(4),
			i => i.MatchBrfalse(out _),
			i => i.MatchLdloc(1),
			i => i.MatchLdcR8(2),
			i => i.MatchMul(),
			i => i.MatchStloc(1)
		);
		// Replace:
		//     num *= 2.0;
		// Width:
		//     num *= 1.5;
		c.Index -= 3;
		c.Remove();
		c.Emit(OpCodes.Ldc_R8, 1.5);
	}
	static void IL_Player_Hurt(ILContext il)
	{
		ILCursor c = new(il);
		/*
			// if (Crit)
			IL_01c3: ldloc.s 6
			IL_01c5: conv.r8
			IL_01c6: stloc.s 7
			IL_01c8: ldarg.s Crit
			IL_01ca: brfalse.s IL_01d2

			// num *= 2;
			IL_01cc: ldloc.s 6
			IL_01ce: ldc.i4.2
			IL_01cf: mul
			IL_01d0: stloc.s 6
		*/
		c.GotoNext(MoveType.After,
			i => i.MatchLdloc(6),
			i => i.MatchConvR8(),
			i => i.MatchStloc(7),
			i => i.MatchLdarg(6),
			i => i.MatchBrfalse(out _),
			i => i.MatchLdloc(6),
			i => i.MatchLdcI4(2),
			i => i.MatchMul(),
			i => i.MatchStloc(6)
		);

		// Replace:
		//     num *= 2;
		// Width:
		//     num *= 1.5;
		c.Index -= 3;
		c.Remove();
		c.Emit(OpCodes.Ldc_R8, 1.5);
	}
}