using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Recipes;

public class AddMusketBall : ModSystem
{
	public override void AddRecipes()
	{
		Recipe.Create(ItemID.MusketBall, 70).AddRecipeGroup("IronBar").AddTile(TileID.Anvils).Register();
	}
}
