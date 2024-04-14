using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Windows.Forms;
using System.Diagnostics;
using System;

namespace DAMod;

public class DAMod : Mod {
	public static DAMod instance;
	public ModKeybind lockOnKey;
    bool loaded = false;

	public override void Load() {
		instance = this;
		lockOnKey = KeybindLoader.RegisterKeybind(this, "Lock On", "Q");
        {
            SetSpawnRange(2560,2160); // x1.5,x2.0
			SetSpwanRate(300); // x0.5 (x2)
			SetMaxSpawns(10); // x2
			Main.mapMinimapDefaultScale = 1f;
			Main.mapOverlayScale = 3f;
		}
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			Main.multiplayerNPCSmoothingRange = Config.Client.MultiplayerNPCSmoothingRange;
			Main.teamNamePlateDistance = Config.Client.TeamNamePlateDistance;
		}
		else if (Main.netMode == NetmodeID.Server) {
			Main.npcStreamSpeed = Config.Server.NPCStreamSpeed;
		}
		loaded = true;
	}

	public override void Unload() {
		if (loaded) {
            SetSpawnRange(1920,1080);
			SetSpwanRate(600);
			SetMaxSpawns(5);
			Main.mapMinimapDefaultScale = 1.05f;
			Main.mapOverlayScale = 2.5f;
		}
		instance = null;

		MessageBox.Show("Due to tModLoader not unloading certain IL edits from "+(this.DisplayName)+", tModLoader needs to be restarted.\n\nIf you are attempting to join a server, it is best you skip mod loading before joining.\nHold the 'Shift' key at the splash screen until you are at the main menu.\n\nPress OK to restart.", "tModLoader - " + this.DisplayName);
		Process.Start(Environment.ProcessPath, Environment.CommandLine);
		Environment.Exit(0);
	}

	internal static void SetSpawnRange(int sWidth, int sHeight) {
		typeof(NPC).GetField("sWidth", BindingFlags.Static | BindingFlags.Public).SetValue(null, sWidth);
		typeof(NPC).GetField("sHeight", BindingFlags.Static | BindingFlags.Public).SetValue(null, sHeight);
		typeof(NPC).GetField("spawnRangeX", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)((float)(sWidth / 16) * 0.7));
		typeof(NPC).GetField("spawnRangeY", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)((float)(sHeight / 16) * 0.7));
		typeof(NPC).GetField("safeRangeX", BindingFlags.Static | BindingFlags.Public).SetValue(null, (int)((float)(sWidth / 16) * 0.52));
		typeof(NPC).GetField("safeRangeY", BindingFlags.Static | BindingFlags.Public).SetValue(null, (int)((float)(sHeight / 16) * 0.52));
		typeof(NPC).GetField("activeRangeX", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)((float)sWidth * 2.1));
		typeof(NPC).GetField("activeRangeY", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)((float)sHeight * 2.1));
		typeof(NPC).GetField("townRangeX", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 1920);
		typeof(NPC).GetField("townRangeY", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 1080);
	}

	internal static void SetSpwanRate(int num)
	{
		typeof(NPC).GetField("defaultSpawnRate", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, num);
		typeof(NPC).GetField("spawnRate", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, num);
	}

	internal static void SetMaxSpawns(int num)
	{
		typeof(NPC).GetField("defaultMaxSpawns", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, num);
		typeof(NPC).GetField("maxSpawns", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, num);
	}
}


// TODO:
// Remove Sailfish Boots, Tsunami in a Bottle, Extractinator from Wooden Crates
// Add Band of Regeneration, Shoe Spikes, Extractinator to Iron Crates