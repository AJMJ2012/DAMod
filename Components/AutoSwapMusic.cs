using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class AutoSwapMusic : ModPlayer
{
	bool setMusic = false;
	bool inGame = false;

	// Can't set the music here
	public override void OnEnterWorld()
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
		bool swapMusic = (Config.Client.DefaultMusicMode == MusicMode.Original && Main.drunkWorld && !Main.remixWorld) || (Config.Client.DefaultMusicMode == MusicMode.Otherworld && !Main.drunkWorld);
		if (swapMusic)
		{
			typeof(Main).GetField("swapMusic", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, swapMusic);
		}
	}
}
