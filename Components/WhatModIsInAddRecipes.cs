using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace DAMod.Components;

class RecipeLoader_AddRecipes : ModSystem
{
    static readonly MethodInfo AddRecipes_Method = typeof(RecipeLoader).GetMethod("AddRecipes", BindingFlags.NonPublic | BindingFlags.Static);
    delegate void AddRecipes_Delegate();

    public override void Load()
	{
		HookEndpointManager.Add(AddRecipes_Method, Override_AddRecipes);
	}

	public override void Unload()
	{
		HookEndpointManager.Remove(AddRecipes_Method, Override_AddRecipes);
	}

	// Show what mods are adding recipes
	static void Override_AddRecipes(AddRecipes_Delegate AddRecipes)
	{
		MethodInfo addRecipesMethod = typeof(Mod).GetMethod("AddRecipes", BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo CurrentModProperty = typeof(RecipeLoader).GetProperty("CurrentMod", BindingFlags.NonPublic | BindingFlags.Static);
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			Mod mod = mods[i];
			DALib.UI.SetLoadingMessage("Adding Recipes: " + mod.Name, i / (float)ModLoader.Mods.Length);
			CurrentModProperty.SetValue(null, mod);
			try
			{
				addRecipesMethod.Invoke(mod, Array.Empty<object>());
				typeof(SystemLoader).GetMethod("AddRecipes", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { mod });
				LoaderUtils.ForEachAndAggregateExceptions(mod.GetContent<ModItem>(), delegate (ModItem item)
				{
					item.AddRecipes();
				});
				LoaderUtils.ForEachAndAggregateExceptions(mod.GetContent<GlobalItem>(), delegate (GlobalItem global)
				{
					global.AddRecipes();
				});
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
			finally
			{
				CurrentModProperty.SetValue(null, null);
			}
		}
		return;
	}
}

