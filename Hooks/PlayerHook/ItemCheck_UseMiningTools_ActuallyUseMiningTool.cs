using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Minimap;
using Terraria.ModLoader;

namespace DAMod.Hooks.PlayerHook {
	class ItemCheck_UseMiningTools_ActuallyUseMiningTool : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(ItemCheck_UseMiningTools_ActuallyUseMiningToolMethod, Override_ItemCheck_UseMiningTools_ActuallyUseMiningTool);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(ItemCheck_UseMiningTools_ActuallyUseMiningToolMethod, Override_ItemCheck_UseMiningTools_ActuallyUseMiningTool); } catch {}
		}

		static MethodInfo ItemCheck_UseMiningTools_ActuallyUseMiningToolMethod => typeof(Player).GetMethod("ItemCheck_UseMiningTools_ActuallyUseMiningTool", BindingFlags.NonPublic | BindingFlags.Instance);
		delegate void OrigItemCheck_UseMiningTools_ActuallyUseMiningTool(Player instance, Item sItem, out bool canHitWalls, int x, int y);

		// Reset minimap zoom to Main.mapMinimapDefaultScale
		static void Override_ItemCheck_UseMiningTools_ActuallyUseMiningTool(OrigItemCheck_UseMiningTools_ActuallyUseMiningTool ItemCheck_UseMiningTools_ActuallyUseMiningTool, Player instance, Item sItem, out bool canHitWalls, int x, int y) {
			Tile tile = Main.tile[x, y];
			int type1 = tile.TileType;
			bool active1 = tile.HasTile;
			if (!active1) {
				canHitWalls = true;
				return;
			}
			ItemCheck_UseMiningTools_ActuallyUseMiningTool(instance, sItem, out canHitWalls, x, y);
			int type2 = tile.TileType;
			bool active2 = tile.HasTile;
			if (type1 == type2 && active1 == active2) {
				ItemCheck_UseMiningTools_ActuallyUseMiningTool(instance, sItem, out canHitWalls, x, y);
			}
		}
	}
}