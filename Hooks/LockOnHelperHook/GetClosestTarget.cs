using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace DAMod.Hooks.LockOnHelperHook {
    class GetClosestTarget : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(GetClosestTargetMethod, Override_GetClosestTarget);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(GetClosestTargetMethod, Override_GetClosestTarget); } catch {}
		}

		static MethodInfo GetClosestTargetMethod => typeof(LockOnHelper).GetMethod("GetClosestTarget", BindingFlags.NonPublic | BindingFlags.Static);
		delegate void OrigGetClosestTarget(Vector2 position);

		// Get closest target to player when smart cursor is enabled
		static void Override_GetClosestTarget(OrigGetClosestTarget GetClosestTarget, Vector2 position) {
			try {
				GetClosestTarget(Main.SmartCursorIsUsed ? Main.LocalPlayer.Center : position);
			}
			catch {}
		}
	}
}


