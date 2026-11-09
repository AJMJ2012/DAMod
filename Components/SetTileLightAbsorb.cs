using System.Reflection;
using Terraria.ModLoader;

namespace DAMod.Components;

class SystemLoader_ModifyLightingBrightness : ModSystem
{
    static readonly MethodInfo ModifyLightingBrightness_Method = typeof(SystemLoader).GetMethod("ModifyLightingBrightness", BindingFlags.Public | BindingFlags.Static);
    delegate void ModifyLightingBrightness_Delegate(ref float negLight, ref float negLight2);

    public override void Load()
	{
		MonoModHooks.Add(ModifyLightingBrightness_Method, Override_ModifyLightingBrightness);
	}

	// Decrease light bleed through tiles
	static void Override_ModifyLightingBrightness(ModifyLightingBrightness_Delegate ModifyLightingBrightness, ref float negLight, ref float negLight2)
	{
		float b = 1f;
		negLight2 *= b - Config.Client.TileLightAbsorption / 100f;
		ModifyLightingBrightness(ref negLight, ref negLight2);
	}
}
