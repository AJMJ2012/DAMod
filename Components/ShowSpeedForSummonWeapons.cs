using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Components;

class SummonDamageClass_ShowStatTooltipLine : ModSystem
{
    static readonly MethodInfo ShowStatTooltipLine_Method = typeof(SummonDamageClass).GetMethod("ShowStatTooltipLine", BindingFlags.Public | BindingFlags.Instance);
    delegate bool ShowStatTooltipLine_Delegate(SummonDamageClass instance, Player player, string lineName);

    public override void Load()
	{
		MonoModHooks.Add(ShowStatTooltipLine_Method, Override_ShowStatTooltipLine);
	}

	// Show speed for summon weapons
	static bool Override_ShowStatTooltipLine(ShowStatTooltipLine_Delegate ShowStatTooltipLine, SummonDamageClass instance, Player player, string lineName)
	{
		return lineName != "CritChance";
	}
}
