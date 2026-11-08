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

	readonly string[] teamName = [
		"None",
		"Red",
		"Green",
		"Blue",
		"Yellow",
		"Pink"
	];

	// Can't set the team here
	public override void OnEnterWorld()
	{
		inGame = true;
		setTeam = false;
	}

	public override void PostUpdate()
	{
		if (inGame && !setTeam)
		{
			setTeam = true;
			SetTeam(team);
		}
	}

	public void SetTeam(int team)
	{
		if (Main.netMode == NetmodeID.MultiplayerClient && team > 0)
		{
			Main.NewText($"Joining Team: {teamName[team]}", Main.teamColor[team]);
			Player.team = team;
			NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, Main.myPlayer);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add("lastTeam", Main.netMode == NetmodeID.MultiplayerClient ? Player.team : team);
	}

	public override void LoadData(TagCompound tag)
	{
		if (tag.ContainsKey("lastTeam"))
		{
			team = tag.GetInt("lastTeam");
		}
	}
}
