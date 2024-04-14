using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using System;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;
class NerfKnockback : ModSystem
{
    static readonly MethodInfo NPC_StrikeNPC_Method = typeof(NPC).GetMethod("StrikeNPC", BindingFlags.Public | BindingFlags.Instance);
    delegate double NPC_StrikeNPC_Delegate(NPC self, int Damage, float knockBack, int hitDirection, bool crit, bool noEffect, bool fromNet);

    public override void Load()
    {
        HookEndpointManager.Modify<NPC_StrikeNPC_Delegate>(NPC_StrikeNPC_Method, IL_NPC_StrikeNPC);
    }

    public override void Unload()
    {
        HookEndpointManager.Unmodify<NPC_StrikeNPC_Delegate>(NPC_StrikeNPC_Method, IL_NPC_StrikeNPC);
    }

    static void IL_NPC_StrikeNPC(ILContext il)
    {
        ILCursor c = new(il);
        /*
	        // float num4 = knockBack * knockBackResist;
	        // sequence point: (line 61748, col 6) to (line 61748, col 47) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\NPC.cs
	        IL_0562: ldarg.2
	        IL_0563: ldarg.0
	        IL_0564: ldfld float32 Terraria.NPC::knockBackResist
	        IL_0569: mul
	        IL_056a: stloc.s 7
        */
        c.GotoNext(MoveType.After,
            i => i.MatchLdarg(2),
            i => i.MatchLdarg(0),
            i => i.MatchLdfld(typeof(NPC), nameof(NPC.knockBackResist)),
            i => i.MatchMul(),
            i => i.MatchStloc(7)
        );
        c.Index -= 1;
        // Replace:
        //     float num4 = knockBack * knockBackResist;
        // Width:
        //     float num4 = knockBack * knockBackResist * 0.75;
        c.Emit(OpCodes.Ldc_R8, 0.75);
        c.Emit(OpCodes.Mul);
    }
}
