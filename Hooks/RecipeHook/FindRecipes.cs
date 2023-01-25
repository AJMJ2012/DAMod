using MonoMod.RuntimeDetour.HookGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Hooks.RecipeHook {
	class FindRecipes : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(FindRecipesMethod, Override_FindRecipes);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(FindRecipesMethod, Override_FindRecipes); } catch {}
		}

		static MethodInfo FindRecipesMethod => typeof(Recipe).GetMethod("FindRecipes", BindingFlags.Public | BindingFlags.Static);
		delegate void OrigFindRecipes(bool canDelayCheck = false);

		// Sort recipes
		static void Override_FindRecipes(OrigFindRecipes FindRecipes, bool canDelayCheck = false) {
			try {
				FindRecipes(canDelayCheck);
				if (ModLoader.HasMod("RecipeBrowser")) { return; } // Sorting screws up Recipe Browser
				// I need exclusive sort categories.
				Main.recipe = Main.recipe.OrderBy(r => r.createItem.IsAir ? 1 : -1) // Having null items above real items causes the list to end abruptly.
					// Bottom Forced
					.ThenBy(r => r.createItem.IsCurrency || r.createItem.IsACoin ? 1 : 0)

					// Top Forced
					.ThenBy(r => r.createItem.createTile == TileID.Torches ? -1 : 0)
					.ThenBy(r => r.createItem.ammo > 0 && !r.createItem.notAmmo ? -1 : 0)
					.ThenBy(r => r.createItem.bait > 0 ? -1 : 0)

					// Category Sort
					.ThenBy(r => r.createItem.createTile > 0 && r.createItem.tileWand <= 0 ? 0 : 1)
					.ThenBy(r => r.createItem.createWall > 0 && r.createItem.tileWand <= 0 ? 0 : 1)
					.ThenBy(r => r.createItem.tileWand > 0 ? 0 : 1)
					.ThenBy(r => new List<int> {
							ItemID.EmptyBucket,
							ItemID.WaterBucket,
							ItemID.HoneyBucket,
							ItemID.LavaBucket,
							ItemID.BottomlessBucket,
							ItemID.BottomlessLavaBucket,
							ItemID.SuperAbsorbantSponge,
							ItemID.LavaAbsorbantSponge
						}.Contains(r.createItem.type) ? 0 : 1) // Force these here

					.ThenBy(r => r.createItem.consumable && r.createItem.pick <= 0 && r.createItem.axe <= 0 && r.createItem.hammer <= 0 && r.createItem.fishingPole <= 0 && r.createItem.damage <= 0 && r.createItem.defense <= 0 ? 0 : 1)
					.ThenBy(r => r.createItem.shoot > 0 && r.createItem.pick <= 0 && r.createItem.axe <= 0 && r.createItem.hammer <= 0 && r.createItem.fishingPole <= 0 && r.createItem.damage <= 0 && r.createItem.defense <= 0 ? 0 : 1)

					.ThenBy(r => r.createItem.dye > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.hairDye > 0 ? 0 : 1)

					.ThenBy(r => r.createItem.headSlot > 0 && r.createItem.vanity ? 0 : 1)
					.ThenBy(r => r.createItem.bodySlot > 0 && r.createItem.vanity ? 0 : 1)
					.ThenBy(r => r.createItem.legSlot > 0 && r.createItem.vanity ? 0 : 1)
					.ThenBy(r => r.createItem.vanity && r.createItem.accessory ? 0 : 1)
					.ThenBy(r => r.createItem.vanity ? 0 : 1)

					.ThenBy(r => r.createItem.accessory ? 0 : 1)

					.ThenBy(r => r.createItem.pick > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.axe > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.hammer > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.fishingPole > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.damage > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.headSlot > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.bodySlot > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.legSlot > 0 ? 0 : 1)
					.ThenBy(r => r.createItem.defense > 0 ? 0 : 1)
					// I want unsorted items to go before the above list. But using 1 : 0 on them messes up the order.

					// Equipment Stats Sort
					.ThenBy(r => r.createItem.DamageType.Type)
					.ThenBy(r => r.createItem.pick)
					.ThenBy(r => r.createItem.axe)
					.ThenBy(r => r.createItem.hammer)
					.ThenBy(r => r.createItem.fishingPole)
					.ThenBy(r => r.createItem.useAmmo)
					.ThenBy(r => -r.createItem.useStyle)
					.ThenBy(r => (r.createItem.pick > 0 || r.createItem.axe > 0 || r.createItem.hammer > 0 || r.createItem.fishingPole > 0 || r.createItem.damage > 0 || r.createItem.defense > 0) ? r.createItem.rare : 0)
					.ThenBy(r => r.createItem.damage)
					.ThenBy(r => r.createItem.defense)

					// Name and Value Sort
					.ThenBy(r => r.createItem.Name.Split(' ').Last())
					.ThenBy(r => r.createItem.rare)
					.ThenBy(r => r.createItem.value)
					.ThenBy(r => r.createItem.Name)
					.ToArray();
			}
			catch {}
		}
	}
}
