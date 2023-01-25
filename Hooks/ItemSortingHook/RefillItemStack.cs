using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace DAMod.Hooks.ItemSortingHook {
    class RefillItemStack : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(RefillItemStackMethod, Override_RefillItemStack);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(RefillItemStackMethod, Override_RefillItemStack); } catch {}
		}

		static MethodInfo RefillItemStackMethod => typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.NonPublic | BindingFlags.Static);
		delegate void OrigRefillItemStack(Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex);

		// Don't refill from favourited items
		static void Override_RefillItemStack(OrigRefillItemStack RefillItemStack, Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex) {
			int num = itemToRefill.maxStack - itemToRefill.stack;
			if (num <= 0) {
				return;
			}
			for (int i = loopStartIndex; i < loopEndIndex; i++) {
				Item item = inv[i];
				if (item.stack >= 1 && item.type == itemToRefill.type && !item.favorited) {
					int num2 = item.stack;
					if (num2 > num) {
						num2 = num;
					}
					num -= num2;
					itemToRefill.stack += num2;
					item.stack -= num2;
					if (item.stack <= 0) {
						item.TurnToAir();
					}
					if (num <= 0) {
						break;
					}
				}
			}
		}
	}
}


