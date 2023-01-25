using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace DAMod.Hooks.LockOnHelperHook {
    class CanUseLockonSystem : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(CanUseLockonSystemMethod, Override_CanUseLockonSystem);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(CanUseLockonSystemMethod, Override_CanUseLockonSystem); } catch {}
		}

		static MethodInfo CanUseLockonSystemMethod => typeof(LockOnHelper).GetMethod("CanUseLockonSystem", BindingFlags.Public | BindingFlags.Static);
		delegate bool OrigCanUseLockonSystem();

		// Allow lock on system when using a keyboard and mouse and if enabled
		static bool Override_CanUseLockonSystem(OrigCanUseLockonSystem CanUseLockonSystem) {
			if (DAMod.instance.lockOnKey.JustPressed) PlayerInput.Triggers.JustPressed.LockOn = true;
			return Config.Client.EnableLockOn || CanUseLockonSystem();
		}
    }
}


