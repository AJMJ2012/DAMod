using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria.ModLoader;

namespace DAMod.Hooks.SystemLoaderHook {
	class ModifyLightingBrightness : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(ModifyLightingBrightnessMethod, Override_ModifyLightingBrightness);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(ModifyLightingBrightnessMethod, Override_ModifyLightingBrightness); } catch {}
		}

		static MethodInfo ModifyLightingBrightnessMethod => typeof(SystemLoader).GetMethod("ModifyLightingBrightness", BindingFlags.Public | BindingFlags.Static);
		delegate void OrigModifyLightingBrightness(ref float negLight, ref float negLight2);

		// Decrease light bleed through tiles
		static void Override_ModifyLightingBrightness(OrigModifyLightingBrightness ModifyLightingBrightness, ref float negLight, ref float negLight2) {
			negLight2 *= (1f - (Config.Client.TileLightAbsorption / 100f));
			ModifyLightingBrightness(ref negLight, ref negLight2);
		}
    }
}
