using MonoMod.RuntimeDetour.HookGen;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace DAMod.Hooks.ItemSortingHook {
    class FillAmmoFromInventory : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(FillAmmoFromInventoryMethod, Override_FillAmmoFromInventory);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(FillAmmoFromInventoryMethod, Override_FillAmmoFromInventory); } catch {}
		}

		static MethodInfo FillAmmoFromInventoryMethod => typeof(ItemSorting).GetMethod("FillAmmoFromInventory", BindingFlags.Public | BindingFlags.Static);
		delegate void OrigFillAmmoFromInventory();

		// Don't refill from favourited ammo
		static void Override_FillAmmoFromInventory(OrigFillAmmoFromInventory FillAmmoFromInventory) {
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			Item[] inventory = Main.LocalPlayer.inventory;
			for (int i = 54; i < 58; i++) {
				ItemSlot.SetGlow(i, 0.31f, chest: false);
				Item item = inventory[i];
				if (item.IsAir) {
					list2.Add(i);
				}
				else if (item.ammo != AmmoID.None) {
					if (!list.Contains(item.type)) {
						list.Add(item.type);
					}
					typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]{inventory, inventory[i], 0, 50});
				}
			}
			if (list2.Count < 1) {
				return;
			}
			for (int j = 0; j < 50; j++) {
				Item item2 = inventory[j];
				if (item2.stack >= 1 && item2.CanFillEmptyAmmoSlot() && list.Contains(item2.type) && !item2.favorited) { // <- Changed
					int num = list2[0];
					list2.Remove(num);
					Utils.Swap(ref inventory[j], ref inventory[num]);
					typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]{inventory, inventory[num], 0, 50});
					if (list2.Count == 0) {
						break;
					}
				}
			}
			if (list2.Count < 1) {
				return;
			}
			for (int k = 0; k < 50; k++) {
				Item item3 = inventory[k];
				if (item3.stack >= 1 && item3.CanFillEmptyAmmoSlot() && item3.FitsAmmoSlot() && !item3.favorited) { // <- Changed
					int num2 = list2[0];
					list2.Remove(num2);
					Utils.Swap(ref inventory[k], ref inventory[num2]);
					typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]{inventory, inventory[num2], 0, 50});
					if (list2.Count == 0) {
						break;
					}
				}
			}
		}
    }
}


