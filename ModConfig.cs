using System;
using System.ComponentModel;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ID;
using Terraria.ModLoader;
using Newtonsoft.Json;

namespace DAMod;

public class ConfigEnforcer : ModSystem
{
    public override void PostUpdateEverything()
    {
		// Can't use a property as they are sorted after fields...
        Config.Client.LockOnAllowed = (Main.netMode == NetmodeID.SinglePlayer) || (Main.netMode == NetmodeID.MultiplayerClient && Config.Server.EnableLockOn);
    }
}

[Label("Client Config")]
public class ClientConfig : ModConfig {
	public override ConfigScope Mode => ConfigScope.ClientSide;
	public static ClientConfig Instance;


	[Header("Tooltips")]
	[Label("Detailed Tooltips")]
	[Tooltip("Shows details on some tooltips")]
	[DefaultValue(false)]
	public bool DetailedTooltips = false;

	//[Label("Extended Tooltips")]
	//[Tooltip("Shows extended details on stat based tooltips")]
	//[DefaultValue(false)]
	//public bool ExtendedTooltips => false;


	[Header("Music")]
	[Label("Default Music Mode")]
	[Tooltip("Only applies on load for now")]
    [DefaultValue(MusicMode.Default)]
    [DrawTicks]
    public MusicMode MusicMode = MusicMode.Default;

	[Header("Visual")]
	[Label("Tile Light Absorption %")]
	[DefaultValue(50)]
	[Range(0, 100)]
	[Increment(1)]
	[Slider]
	public int TileLightAbsorption = 50;


	[Header("Gameplay")]
	[Label("Enable Lock On system for keyboard + mouse")]
    [DefaultValue(false)]
    public bool EnableLockOn = false;

	[Label("Allowed by Server")]
	[JsonIgnore]
	public bool LockOnAllowed;


    [Header("UI")]
    [Label("Link UI Scale to Zoom")]
    [DefaultValue(false)]
    public bool LinkUIScale = false;

    [Header("Multiplayer")]

	[Label("NPC Smoothing Range")]
	[DefaultValue(300)]
	[Range(0, 600)]
	public int MultiplayerNPCSmoothingRange = 300;

	[Label("Team Name Plate Distance")]
	[DefaultValue(2000)]
	[Range(0, 16800)]
	public int TeamNamePlateDistance = 2000;
	public override void OnChanged() {
		Main.multiplayerNPCSmoothingRange = MultiplayerNPCSmoothingRange;
		Main.teamNamePlateDistance = TeamNamePlateDistance;
	}
}

[Label("Server Config")]
public class ServerConfig : ModConfig {
	public override ConfigScope Mode => ConfigScope.ServerSide;
	public static ServerConfig Instance;

	[Header("Networking")]
	[Label("NPC Stream Speed")]
	[Tooltip("Server Only")]
	[DefaultValue(30)]
	[Range(1, 60)]
	[Increment(1)]
	[Slider]
	public int NPCStreamSpeed = 30;

	[Header("Gameplay")]
	[Label("Enable Lock On system for keyboard + mouse")]
	[Tooltip("Server Only")]
    [DefaultValue(false)]
    public bool EnableLockOn = false;

	public override void OnChanged() {
		Main.npcStreamSpeed = NPCStreamSpeed;
	}

	public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) {
		return DALib.Auth.IsAdmin(whoAmI, ref message);
	}
}

public static class Config {
	public static ClientConfig Client => ClientConfig.Instance;
	public static ServerConfig Server => ServerConfig.Instance;
}

