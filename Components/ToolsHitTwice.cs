using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class ToolsHitTwice : ModSystem
{
    static readonly MethodInfo Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool_Method = typeof(Player).GetMethod("ItemCheck_UseMiningTools_ActuallyUseMiningTool", BindingFlags.NonPublic | BindingFlags.Instance);
    delegate void Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool_Delegate(Player instance, Item sItem, out bool canHitWalls, int x, int y);

    public override void Load()
	{
		MonoModHooks.Add(Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool_Method, Override_Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool);
	}

	// Tools hit twice
	static void Override_Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool(Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool_Delegate Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool, Player instance, Item sItem, out bool canHitWalls, int x, int y)
	{
		Tile tile = Main.tile[x, y];
		int type1 = tile.TileType;
		bool active1 = tile.HasTile;
		if (!active1)
		{
			canHitWalls = true;
			return;
		}
        Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool(instance, sItem, out canHitWalls, x, y);
		int type2 = tile.TileType;
		bool active2 = tile.HasTile;
		if (type1 == type2 && active1 == active2)
		{
            Player_ItemCheck_UseMiningTools_ActuallyUseMiningTool(instance, sItem, out canHitWalls, x, y);
		}
	}
}
