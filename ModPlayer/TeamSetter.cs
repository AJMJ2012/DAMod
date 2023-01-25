using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class TeamSetter : ModPlayer {
		bool setTeam = false;
		bool inGame = false;

		public override void OnEnterWorld(Player player) {
			inGame = true;
		}

		public override void PostUpdate() {
			if (inGame && Main.netMode == 1) SetTeam();
		}

		public void SetTeam() {
			if (!setTeam) {
				setTeam = true;
				Player.team = Config.Client.AutoJoinTeam;
				NetMessage.SendData(45, -1, -1, null, Main.myPlayer);
			}
		}
	}
}