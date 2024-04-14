using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Minimap;
using Terraria.ModLoader;

namespace DAMod.Components;

class ResetMinimapZoom : ModSystem
{
    static readonly MethodInfo MinimapFrame_ResetZoom_Method = typeof(MinimapFrame).GetMethod("ResetZoom", BindingFlags.NonPublic | BindingFlags.Instance);
    delegate void MinimapFrame_ResetZoom_Delegate(MinimapFrame instance);

    public override void Load()
	{
		HookEndpointManager.Add(MinimapFrame_ResetZoom_Method, Override_MinimapFrame_ResetZoom);
	}

	public override void Unload()
	{
		HookEndpointManager.Remove(MinimapFrame_ResetZoom_Method, Override_MinimapFrame_ResetZoom);
	}

	// Reset minimap zoom to Main.mapMinimapDefaultScale
	static void Override_MinimapFrame_ResetZoom(MinimapFrame_ResetZoom_Delegate ResetZoom, MinimapFrame instance)
	{
		Main.mapMinimapScale = Main.mapMinimapDefaultScale;
	}
}
