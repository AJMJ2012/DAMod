using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Recipes;

public class AddJumpBottles : ModSystem
{
	public override void AddRecipes()
	{
		Recipe.Create(ItemID.CloudinaBottle).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.Cloud, 20).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
		Recipe.Create(ItemID.BlizzardinaBottle).AddIngredient(ItemID.CloudinaBottle).AddIngredient(ItemID.FrostCore).AddTile(TileID.CrystalBall).Register();
		Recipe.Create(ItemID.BlizzardinaBalloon).AddIngredient(ItemID.CloudinaBalloon).AddIngredient(ItemID.FrostCore).AddTile(TileID.CrystalBall).Register();
		Recipe.Create(ItemID.BlueHorseshoeBalloon).AddIngredient(ItemID.WhiteHorseshoeBalloon).AddIngredient(ItemID.FrostCore).AddTile(TileID.CrystalBall).Register();
		Recipe.Create(ItemID.SandstorminaBottle).AddIngredient(ItemID.CloudinaBottle).AddIngredient(ItemID.AncientBattleArmorMaterial).AddTile(TileID.CrystalBall).Register();
		Recipe.Create(ItemID.SandstorminaBalloon).AddIngredient(ItemID.CloudinaBalloon).AddIngredient(ItemID.AncientBattleArmorMaterial).AddTile(TileID.CrystalBall).Register();
		Recipe.Create(ItemID.YellowHorseshoeBalloon).AddIngredient(ItemID.WhiteHorseshoeBalloon).AddIngredient(ItemID.AncientBattleArmorMaterial).AddTile(TileID.CrystalBall).Register();
        Recipe.Create(ItemID.FartInABalloon).AddIngredient(ItemID.CloudinaBalloon).AddIngredient(ItemID.WhoopieCushion).AddTile(TileID.TinkerersWorkbench).Register();
    }
}
