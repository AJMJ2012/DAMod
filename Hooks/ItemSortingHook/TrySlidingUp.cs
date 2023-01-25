using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace DAMod.Hooks.ItemSortingHook {
    class TrySlidingUp : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(TrySlidingUpMethod, Override_TrySlidingUp);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(TrySlidingUpMethod, Override_TrySlidingUp); } catch {}
		}

		static MethodInfo TrySlidingUpMethod => typeof(ItemSorting).GetMethod("TrySlidingUp", BindingFlags.NonPublic | BindingFlags.Static);
		delegate void OrigTrySlidingUp(Item[] inv, int slot, int minimumIndex);

		// Don't refill from favourited items
		static void Override_TrySlidingUp(OrigTrySlidingUp TrySlidingUp, Item[] inv, int slot, int minimumIndex) {
			for (int num = slot; num > minimumIndex; num--) {
				if (inv[num - 1].IsAir && !inv[num].favorited) {
					Utils.Swap(ref inv[num], ref inv[num - 1]);
				}
			}
		}
	}
}
