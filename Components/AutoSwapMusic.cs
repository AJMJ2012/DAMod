using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class AutoSwapMusic : ModPlayer
{
	bool setMusic = false;
	bool inGame = false;

	// Can't set the music here
	public override void OnEnterWorld(Player player)
	{
		inGame = true;
		setMusic = false;
	}

	public override void PostUpdate()
	{
		if (inGame && !setMusic)
		{
			setMusic = true;
			SetMusic();
		}
	}

	public void SetMusic()
	{
		bool swapMusic = Config.Client.MusicMode == MusicMode.Original && Main.drunkWorld || Config.Client.MusicMode == MusicMode.Otherworld && !Main.drunkWorld;
		if (swapMusic)
		{
			typeof(Main).GetField("swapMusic", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, swapMusic);
		}
	}
}
