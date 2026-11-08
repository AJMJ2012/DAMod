using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components;

class BetterLockOnTargeting : ModSystem
{
    static readonly MethodInfo LockOnHelper_GetClosestTarget_Method = typeof(LockOnHelper).GetMethod("GetClosestTarget", BindingFlags.NonPublic | BindingFlags.Static);
    delegate void LockOnHelper_GetClosestTarget_Delegate(Vector2 position);
    static readonly MethodInfo LockOnHelper_ValidTarget_Method = typeof(LockOnHelper).GetMethod("ValidTarget", BindingFlags.NonPublic | BindingFlags.Static);
    delegate bool LockOnHelper_ValidTarget_Delegate(NPC n);

    public override void Load()
	{
		MonoModHooks.Add(LockOnHelper_GetClosestTarget_Method, Override_LockOnHelper_GetClosestTarget);
        MonoModHooks.Add(LockOnHelper_ValidTarget_Method, Override_LockOnHelper_ValidTarget);
    }
    // Get closest target to player when smart cursor is enabled
    static void Override_LockOnHelper_GetClosestTarget(LockOnHelper_GetClosestTarget_Delegate GetClosestTarget, Vector2 position)
	{
		GetClosestTarget(Main.SmartCursorIsUsed ? Main.LocalPlayer.Center : position);
    }

    // Don't lock on to critters if you can't hurt them
    // Don't lock on to anything if you aren't using a weapon
    static bool Override_LockOnHelper_ValidTarget(LockOnHelper_ValidTarget_Delegate ValidTarget, NPC n)
	{
		if (Main.LocalPlayer.dontHurtCritters && NPCID.Sets.CountsAsCritter[n.type])
		{
			return false;
        }
		if (Main.LocalPlayer.HeldItem.damage <= 0)
        {
			return false;
        }
        return ValidTarget(n);
    }
}

