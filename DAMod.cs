using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod;

public class DAMod : Mod {
	public static DAMod instance;
	public ModKeybind lockOnKey;
	public ModKeybind reloadSoundEngineKey;
    bool loaded = false;

	public override void Load()
	{
		instance = this;
		lockOnKey = KeybindLoader.RegisterKeybind(this, "LockOn", "Q");
		{
            SetSpawnRange(Config.Server.SpawnRangeX, Config.Server.SpawnRangeY);
			SetSpwanRate(Config.Server.SpawnRate);
			SetMaxSpawns(Config.Server.MaxSpawns);
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
	}

	internal static void SetSpawnRange(int sWidth, int sHeight) {
		typeof(NPC).GetField("sWidth", BindingFlags.Static | BindingFlags.Public).SetValue(null, sWidth);
		typeof(NPC).GetField("sHeight", BindingFlags.Static | BindingFlags.Public).SetValue(null, sHeight);
		typeof(NPC).GetField("spawnRangeX", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)(sWidth / 16 * 0.7));
		typeof(NPC).GetField("spawnRangeY", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)(sHeight / 16 * 0.7));
		typeof(NPC).GetField("safeRangeX", BindingFlags.Static | BindingFlags.Public).SetValue(null, (int)(sWidth / 16 * 0.52));
		typeof(NPC).GetField("safeRangeY", BindingFlags.Static | BindingFlags.Public).SetValue(null, (int)(sHeight / 16 * 0.52));
		typeof(NPC).GetField("activeRangeX", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)(sWidth * 2.1));
		typeof(NPC).GetField("activeRangeY", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, (int)(sHeight * 2.1));
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