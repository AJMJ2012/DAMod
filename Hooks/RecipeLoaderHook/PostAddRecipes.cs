using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DAMod.Hooks.RecipeLoaderHook {
	class PostAddRecipes : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(PostAddRecipesMethod, Override_PostAddRecipes);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(PostAddRecipesMethod, Override_PostAddRecipes); } catch {}
		}

		static MethodInfo PostAddRecipesMethod => typeof(RecipeLoader).GetMethod("PostAddRecipes", BindingFlags.NonPublic | BindingFlags.Static);
		delegate void OrigPostAddRecipes();

		// Show what mods are setting up
		static void Override_PostAddRecipes(OrigPostAddRecipes PostAddRecipes) {
			MethodInfo postAddRecipesMethod = typeof(Mod).GetMethod("PostAddRecipes", BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo CurrentModProperty = typeof(RecipeLoader).GetProperty("CurrentMod", BindingFlags.NonPublic | BindingFlags.Static);
			Mod[] mods = ModLoader.Mods;
			for (int i = 0; i < mods.Length; i++) {
				Mod mod = mods[i];
				DALib.UI.SetLoadingMessage("Setting Up: " + mod.Name, ((float)i / (float)ModLoader.Mods.Length));
				CurrentModProperty.SetValue(null, mod);
				try {
					postAddRecipesMethod.Invoke(mod, Array.Empty<object>());
					typeof(SystemLoader).GetMethod("PostAddRecipes", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[]{mod});
				}
				catch (Exception ex) {
					ex.Data["mod"] = mod.Name;
					throw;
				}
				finally {
					CurrentModProperty.SetValue(null, null);
				}
			}
			DALib.UI.SetLoadingMessage("Finalizing...");
			return;
		}
	}
}
