using System;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

public class LinkUIScale : ModSystem
{
	public override void PostUpdateInput()
	{
		if (Config.Client.LinkUIScale && ((Main.ingameOptionsWindow && !Main.mouseLeft) || !Main.ingameOptionsWindow))
		{
			Main.UIScale = Math.Min(Main.ForcedMinimumZoom * Main.GameZoomTarget, 2f);
			Main.temporaryGUIScaleSlider = Main.UIScale;
		}
	}
}
