using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DAMod.Components;

class AutoSetTeam : ModPlayer
{
	bool setTeam = false;
	bool inGame = false;
	int team = 0;

	// Can't set the team here
	public override void OnEnterWorld(Player player)
	{
		if (Main.netMode != NetmodeID.MultiplayerClient) return;
		inGame = true;
		setTeam = false;
	}

	public override void PostUpdate()
	{
		if (Main.netMode != NetmodeID.MultiplayerClient) return;
		if (inGame && !setTeam)
		{
			setTeam = true;
			SetTeam(team);
		}
	}

	public void SetTeam(int team)
	{
		if (Main.netMode != NetmodeID.MultiplayerClient) return;
		if (team > 0)
		{
			Player.team = team;
			NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, Main.myPlayer);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		if (Main.netMode != NetmodeID.MultiplayerClient) return;
		tag.Add("lastTeam", Player.team);
	}

	public override void LoadData(TagCompound tag)
	{
		if (tag.ContainsKey("lastTeam"))
		{
			team = tag.GetInt("lastTeam");
		}
	}
}
