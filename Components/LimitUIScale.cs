using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

public class LimitUIScale : ModSystem
{
	public override void PostUpdateInput()
	{
		if (Main.UIScale < Main.ForcedMinimumZoom)
		{
			Main.UIScale = Main.ForcedMinimumZoom;
			Main.temporaryGUIScaleSlider = Main.UIScale;
		}
	}
}
