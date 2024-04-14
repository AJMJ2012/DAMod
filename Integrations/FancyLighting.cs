using Terraria.ModLoader;

namespace DAMod.Integrations;

[JITWhenModsEnabled("RecursiveCraft")]
public static class FancyLightingIntegration
{
	public static bool IsSmooth()
	{
		return ModContent.GetInstance<FancyLighting.Config.LightingConfig>().UseSmoothLighting;
	}
}

