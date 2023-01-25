using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod {
	public class DAMod : Mod {
		public static DAMod instance;
		public ModKeybind lockOnKey;
		public ModKeybind resourcePackKey;
		bool loaded = false;

		public static float critDamageMult = 1.5f;

		public override void Load() {
			instance = this;
			lockOnKey = KeybindLoader.RegisterKeybind(this, "Lock On", "Q");
			resourcePackKey = KeybindLoader.RegisterKeybind(this, "Open Resource Packs", "F12");
			{
				SetSpawnRange(2560,2160); // x1.5,x2.0
				SetSpwanRate(300); // x0.5
				Main.mapMinimapDefaultScale = 1f;
				Main.mapOverlayScale = 3f;
			}
			if (Main.netMode == 1) {
				Main.multiplayerNPCSmoothingRange = Config.Client.MultiplayerNPCSmoothingRange;
				Main.teamNamePlateDistance = Config.Client.TeamNamePlateDistance;
			}
			else if (Main.netMode == 2) {
				Main.npcStreamSpeed = Config.Server.NPCStreamSpeed;
			}
			loaded = true;
		}

		public override void Unload() {
			if (loaded) {
				SetSpawnRange(1920,1080);
				SetSpwanRate(600);
				Main.mapMinimapDefaultScale = 1.05f;
				Main.mapOverlayScale = 2.5f;
			}
			instance = null;
		}

		internal void SetSpawnRange(int sWidth, int sHeight) {
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

		internal void SetSpwanRate(int rate) {
			typeof(NPC).GetField("defaultSpawnRate", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, rate);
			typeof(NPC).GetField("spawnRate", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, rate);
		}
	}
}

// TODO:
// Remove Sailfish Boots, Tsunami in a Bottle, Extractinator from Wooden Crates
// Add Band of Regeneration, Shoe Spikes, Extractinator to Iron Crates