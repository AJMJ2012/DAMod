using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Minimap;
using Terraria.ModLoader;

namespace DAMod.Hooks.MinimapFrameHook {
	class ResetZoom : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(ResetZoomMethod, Override_ResetZoom);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(ResetZoomMethod, Override_ResetZoom); } catch {}
		}

		static MethodInfo ResetZoomMethod => typeof(MinimapFrame).GetMethod("ResetZoom", BindingFlags.NonPublic | BindingFlags.Instance);
		delegate void OrigResetZoom(MinimapFrame instance);

		// Reset minimap zoom to Main.mapMinimapDefaultScale
		static void Override_ResetZoom(OrigResetZoom ResetZoom, MinimapFrame instance) {
			Main.mapMinimapScale = Main.mapMinimapDefaultScale;
		}
	}
}