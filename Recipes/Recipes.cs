using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class Recipes : ModSystem {
		public override void AddRecipes() {
			Recipe.Create(ItemID.MusketBall, 70).AddRecipeGroup("IronBar").AddTile(TileID.Anvils).Register();

			// Balloons
			Recipe.Create(ItemID.ShinyRedBalloon).AddIngredient(ItemID.WhiteString).AddIngredient(ItemID.SillyBalloonGreen).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.ShinyRedBalloon).AddIngredient(ItemID.WhiteString).AddIngredient(ItemID.SillyBalloonPink).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.ShinyRedBalloon).AddIngredient(ItemID.WhiteString).AddIngredient(ItemID.SillyBalloonPurple).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.ShinyRedBalloon).AddIngredient(ItemID.SillyBalloonTiedGreen).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.ShinyRedBalloon).AddIngredient(ItemID.SillyBalloonTiedPink).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.ShinyRedBalloon).AddIngredient(ItemID.SillyBalloonTiedPurple).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();

			// Bottles
			Recipe.Create(ItemID.CloudinaBottle).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.Cloud, 20).AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.BlizzardinaBottle).AddIngredient(ItemID.CloudinaBottle).AddIngredient(ItemID.FrostCore).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.BlizzardinaBalloon).AddIngredient(ItemID.CloudinaBalloon).AddIngredient(ItemID.FrostCore).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.BlueHorseshoeBalloon).AddIngredient(ItemID.WhiteHorseshoeBalloon).AddIngredient(ItemID.FrostCore).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.SandstorminaBottle).AddIngredient(ItemID.CloudinaBottle).AddIngredient(ItemID.AncientBattleArmorMaterial).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.SandstorminaBalloon).AddIngredient(ItemID.CloudinaBalloon).AddIngredient(ItemID.AncientBattleArmorMaterial).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.YellowHorseshoeBalloon).AddIngredient(ItemID.WhiteHorseshoeBalloon).AddIngredient(ItemID.AncientBattleArmorMaterial).AddTile(TileID.CrystalBall).Register();
			Recipe.Create(ItemID.FartInABalloon).AddIngredient(ItemID.CloudinaBalloon).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}