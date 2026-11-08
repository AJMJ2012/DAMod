using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class Recipe_FindRecipes : ModSystem
{
    static readonly MethodInfo FindRecipes_Method = typeof(Recipe).GetMethod("FindRecipes", BindingFlags.Public | BindingFlags.Static);
    delegate void FindRecipes_Delegate(bool canDelayCheck = false);

    public override void Load()
	{
		MonoModHooks.Add(FindRecipes_Method, Override_FindRecipes);
	}

	// Sort recipes
	// TODO: Fix RecipeBrowser conflict
	static void Override_FindRecipes(FindRecipes_Delegate FindRecipes, bool canDelayCheck = false)
	{
		FindRecipes(canDelayCheck);
		if (ModLoader.HasMod("RecipeBrowser")) { return; } // Sorting screws up Recipe Browser
		Main.recipe = RecipeSorter.Sort(Main.recipe);
	}
}