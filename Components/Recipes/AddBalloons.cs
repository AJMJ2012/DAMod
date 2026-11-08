using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Recipes;

class AddBalloons : ModSystem
{
	public override void AddRecipes()
	{
        RecipeGroup.RegisterGroup("Strings", new RecipeGroup(() => Lang.misc[37].Value + " " + "String", [ItemID.BlackString, ItemID.BlueString, ItemID.BrownString, ItemID.CyanString, ItemID.GreenString, ItemID.LimeString, ItemID.OrangeString, ItemID.PinkString, ItemID.PurpleString, ItemID.RainbowString, ItemID.RedString, ItemID.SkyBlueString, ItemID.TealString, ItemID.VioletString, ItemID.YellowString, ItemID.WhiteString]));
        RecipeGroup.RegisterGroup("SillyBalloons", new RecipeGroup(() => Lang.misc[37].Value + " " + "Silly Balloons", [ItemID.SillyBalloonGreen, ItemID.SillyBalloonPink, ItemID.SillyBalloonPurple]));
        RecipeGroup.RegisterGroup("TiedSillyBalloons", new RecipeGroup(() => Lang.misc[37].Value + " " + "Tied Silly Balloons", [ItemID.SillyBalloonTiedGreen, ItemID.SillyBalloonTiedPink, ItemID.SillyBalloonTiedPurple]));
        Recipe.Create(ItemID.ShinyRedBalloon).AddRecipeGroup("Strings").AddRecipeGroup("SillyBalloons").AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
        Recipe.Create(ItemID.ShinyRedBalloon).AddRecipeGroup("TiedSillyBalloons").AddIngredient(ItemID.SoulofFlight, 5).AddTile(TileID.CrystalBall).Register();
	}
}
