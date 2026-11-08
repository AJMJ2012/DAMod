using System;
using System.ComponentModel;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ID;
using System.Reflection;
using DAMod.Components;

namespace DAMod;

public class ClientConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;
	public static ClientConfig Instance;


	[Header("Tooltips")]

	[DefaultValue(false)]
	public bool DetailedTooltips = false;

	//[Label("Extended Tooltips")]
	//public bool ExtendedTooltips => false;


	[Header("Music")]

	[DefaultValue(MusicMode.Default)]
	[DrawTicks]
	public MusicMode DefaultMusicMode = MusicMode.Default;

	[DefaultValue(false)]
	public bool ChangeMusic = false;

	[Header("Music")]
	public bool UsingOtherworldMusic
	{
		get
		{
			return !Main.swapMusic && Main.drunkWorld && !Main.remixWorld || Main.swapMusic && !Main.drunkWorld;
		}
	}

	[Header("Visual")]

	[DefaultValue(33f)]
	[Range(0f, 100f)]
	[Increment(1f)]
	[Slider]
	public float TileLightAbsorption = 33f;


	[Header("Gameplay")]

	[DefaultValue(false)]
	public bool EnableLockOn = false;

	[Header("Gameplay")]
	public bool LockOnAllowed {
		get
		{
			return (Main.netMode == NetmodeID.SinglePlayer) || (Main.netMode == NetmodeID.MultiplayerClient && Config.Server.EnableLockOn);
		}
	}

	[Header("UI")]

	[DefaultValue(false)]
	public bool LinkUIScale = false;

	[DefaultValue(RecipeSort.Yes)]
	public RecipeSort SortRecipes = RecipeSort.Yes;


	[Header("Multiplayer")]

	[DefaultValue(480)]
	[Range(0, 1920)]
	[Increment(120)]
	[Slider]
	public int MultiplayerNPCSmoothingRange = 480;

	[DefaultValue(19200)]
	[Range(0, 19200)]
	[Increment(1920)]
	[Slider]
	public int TeamNamePlateDistance = 19200;


	public override void OnLoaded() => UpdateConfig();
	public override void OnChanged() => UpdateConfig();

	public void UpdateConfig()
	{
		Main.multiplayerNPCSmoothingRange = MultiplayerNPCSmoothingRange;
		Main.teamNamePlateDistance = TeamNamePlateDistance;
		if (ChangeMusic)
		{
			Main.swapMusic = !Main.swapMusic;
		}
		ChangeMusic = false;

		Main.recipe = RecipeSorter.Sort(Main.recipe);
		if (!Main.gameMenu) Recipe.FindRecipes(false);
	}
}

public class ServerConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;
	public static ServerConfig Instance;


	[Header("Networking")]

	[DefaultValue(30)]
	[Range(0, 60)]
	[Increment(1)]
	[Slider]
	public int NPCStreamSpeed = 30;


	[Header("Gameplay")]

	[DefaultValue(false)]
	public bool EnableLockOn = false;

	[DefaultValue(true)]
	public bool NerfCritDamage = true;

	[DefaultValue(true)]
	public bool NerfKnockback = true;

	[DefaultValue(true)]
	public bool NPCsCantSeeYouThroughWalls = true;


	[Header("Spawning")]

	[DefaultValue(2560)]
	[Range(1280, 9600)]
	[Increment(320)]
	[Slider]
	public int SpawnRangeX = 2880;

	[DefaultValue(2160)]
	[Range(720, 5400)]
	[Increment(180)]
	[Slider]
	public int SpawnRangeY = 1620;

	[DefaultValue(300)]
	[Range(0, 3600)]
	[Slider]
	public int SpawnRate = 300;

	[DefaultValue(5)]
	[Range(0, 100)]
	[Slider]
	public int MaxSpawns = 5;


	public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
	{
		return DALib.Auth.IsAdmin(whoAmI, ref message);
	}

	public override void OnLoaded() => UpdateConfig();
	public override void OnChanged() => UpdateConfig();

	public void UpdateConfig()
	{
		Main.npcStreamSpeed = NPCStreamSpeed;
		DAMod.SetSpawnRange(SpawnRangeX, SpawnRangeY);
		DAMod.SetSpwanRate(SpawnRate);
		DAMod.SetMaxSpawns(MaxSpawns);
	}
}

public static class Config
{
	public static ClientConfig Client => ClientConfig.Instance;
	public static ServerConfig Server => ServerConfig.Instance;
}

