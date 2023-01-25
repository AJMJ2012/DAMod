using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class MusicSetter : ModPlayer {
		bool setMusic = false;
		bool inGame = false;

		public override void OnEnterWorld(Player player) {
			if (Main.netMode == 0) SetMusic();
			inGame = true;
		}

		public override void PostUpdate() {
			if (inGame && Main.netMode == 1) SetMusic();
		}

		public void SetMusic() {
			if (!setMusic) {
				setMusic = true;
				bool swapMusic = (Config.Client.MusicMode == 1 && Main.drunkWorld) || (Config.Client.MusicMode == 2 && !Main.drunkWorld);
				if (swapMusic) {
					typeof(Main).GetField("swapMusic", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, swapMusic);
				}
			}
		}
	}
}