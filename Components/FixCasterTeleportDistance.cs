using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class FixCasterTeleportDistance : ModSystem
{
    static readonly MethodInfo NPC_AI_AttemptToFindTeleportSpot_Method = typeof(NPC).GetMethod("AI_AttemptToFindTeleportSpot", BindingFlags.Public | BindingFlags.Instance);

    public override void Load()
	{
		MonoModHooks.Modify(NPC_AI_AttemptToFindTeleportSpot_Method, IL_NPC_AI_AttemptToFindTeleportSpot);
	}

	static void IL_NPC_AI_AttemptToFindTeleportSpot(ILContext il)
	{
		ILCursor c = new(il);
		/*
			// if (Math.Abs(num * 16 - targetTileX * 16) + Math.Abs(num2 * 16 - targetTileY * 16) > 2000)
			// sequence point: (line 12283, col 3) to (line 12283, col 93) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\NPC.cs
			IL_002b: ldloc.0
			IL_002c: ldc.i4.s 16
			IL_002e: mul
			IL_002f: ldarg.2
			IL_0030: ldc.i4.s 16
			IL_0032: mul
			IL_0033: sub
			IL_0034: call int32 [System.Runtime]System.Math::Abs(int32)
			IL_0039: ldloc.1
			IL_003a: ldc.i4.s 16
			IL_003c: mul
			IL_003d: ldarg.3
			IL_003e: ldc.i4.s 16
			IL_0040: mul
			IL_0041: sub
			IL_0042: call int32 [System.Runtime]System.Math::Abs(int32)
			IL_0047: add
			IL_0048: ldc.i4 2000
			IL_004d: ble IL_0291
		*/
		c.GotoNext(
			i => i.MatchLdloc(0),
			i => i.MatchLdcI4(16),
			i => i.MatchMul(),
			i => i.MatchLdarg(2),
			i => i.MatchLdcI4(16),
			i => i.MatchMul(),
			i => i.MatchSub(),
			i => i.MatchCall(typeof(Math), nameof(Math.Abs)),
			i => i.MatchLdloc(1),
			i => i.MatchLdcI4(16),
			i => i.MatchMul(),
			i => i.MatchLdarg(3),
			i => i.MatchLdcI4(16),
			i => i.MatchMul(),
			i => i.MatchSub(),
			i => i.MatchCall(typeof(Math), nameof(Math.Abs)),
			i => i.MatchAdd(),
			i => i.MatchLdcI4(2000),
			i => i.MatchBle(out var _)
		);
		// Replace:
		//     if (Math.Abs(num * 16 - targetTileX * 16) + Math.Abs(num2 * 16 - targetTileY * 16) > 2000)
		// With:
		//     if (Math.Abs(num * 16 - targetTileX * 16) + Math.Abs(num2 * 16 - targetTileY * 16) > NPC.activeRangeX)
		c.GotoNext(
			i => i.MatchLdcI4(2000)
		);
		c.Remove();
		c.EmitLdsfld(typeof(NPC).GetField("activeRangeX", BindingFlags.Static | BindingFlags.NonPublic));
	}
}

