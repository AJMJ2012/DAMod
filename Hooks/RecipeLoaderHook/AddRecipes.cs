using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace DAMod.Hooks.RecipeLoaderHook {
	class AddRecipes : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(AddRecipesMethod, Override_AddRecipes);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(AddRecipesMethod, Override_AddRecipes); } catch {}
		}

		static MethodInfo AddRecipesMethod => typeof(RecipeLoader).GetMethod("AddRecipes", BindingFlags.NonPublic | BindingFlags.Static);
		delegate void OrigAddRecipes();

		// Show what mods are adding recipes
		static void Override_AddRecipes(OrigAddRecipes AddRecipes) {
			MethodInfo addRecipesMethod = typeof(Mod).GetMethod("AddRecipes", BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo CurrentModProperty = typeof(RecipeLoader).GetProperty("CurrentMod", BindingFlags.NonPublic | BindingFlags.Static);
			Mod[] mods = ModLoader.Mods;
			for (int i = 0; i < mods.Length; i++) {
				Mod mod = mods[i];
				DALib.UI.SetLoadingMessage("Adding Recipes: " + mod.Name, ((float)i / (float)ModLoader.Mods.Length));
				CurrentModProperty.SetValue(null, mod);
				try {
					addRecipesMethod.Invoke(mod, Array.Empty<object>());
					typeof(SystemLoader).GetMethod("AddRecipes", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[]{mod});
					LoaderUtils.ForEachAndAggregateExceptions(mod.GetContent<ModItem>(), delegate(ModItem item) {
						item.AddRecipes();
					});
					LoaderUtils.ForEachAndAggregateExceptions(mod.GetContent<GlobalItem>(), delegate(GlobalItem global) {
						global.AddRecipes();
					});
				}
				catch (Exception ex) {
					ex.Data["mod"] = mod.Name;
					throw;
				}
				finally {
					CurrentModProperty.SetValue(null, null);
				}
			}
			return;
		}
	}
}
