using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace DAMod {
	[Label("Client Config")]
	public class ClientConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public static ClientConfig Instance;


		[Header("Tooltips")]
		[Label("Detailed Tooltips")]
		[Tooltip("Shows details on some tooltips")]
		[DefaultValue(true)]
		public bool DetailedTooltips = false;


		[Header("Music")]
		[Label("Default Music Mode")]
		[Tooltip("0: Auto\n1: Original\n2: Otherworld")]
		[Range(0, 2)]
		[Increment(1)]
		[DefaultValue(0)]
		[Slider]
		public int MusicMode = 0;


		[Header("Multiplayer")]
		[Label("Auto Join Team")]
		[Tooltip("Multiplayer Only")]
		[DefaultValue(5)]
		[Range(0, 5)]
		[Slider]
		public int AutoJoinTeam = 0;


		[Header("Networking")]
		[Label("Multiplayer NPC Smoothing Range")]
		[Tooltip("Multiplayer Only")]
		[DefaultValue(300)]
		[Range(0, 600)]
		public int MultiplayerNPCSmoothingRange = 300;


		[Label("Team Name Plate Distance")]
		[Tooltip("Multiplayer Only")]
		[DefaultValue(2000)]
		[Range(0, 8400)]
		public int TeamNamePlateDistance = 2000;


		[Header("Visual")]
		[Label("Tile Light Absorption %")]
		[DefaultValue(50)]
		[Range(0, 100)]
		[Increment(1)]
		[Slider]
		public int TileLightAbsorption = 50;


		[Header("Either a cheater, filthy casual, or a noob!")]
		[Label("Enable Lock On system for keyboard + mouse")]
		public bool EnableLockOn = false;

		[Header("Disabled")]
		[Label("Extended Tooltips")]
		[Tooltip("Shows extended details on stat based tooltips")]
		[DefaultValue(false)]
		public bool ExtendedTooltips => false;

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
		public int NPCStreamSpeed = 30;


		public override void OnChanged() {
			Main.npcStreamSpeed = NPCStreamSpeed;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {
			return DALib.Auth.IsAdmin(whoAmI, ref message);
		}
	}

	public static class Config {
		public static ClientConfig Client => ClientConfig.Instance;
		public static ServerConfig Server => ServerConfig.Instance;
	}
}
