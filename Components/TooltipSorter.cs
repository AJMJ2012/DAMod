using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace DAMod.Components;

public static class TooltipSorter
{
	public static TooltipLine[] Sort(TooltipLine[] tooltips)
	{
		return Sort(tooltips.ToList()).ToArray();
	}

	// TODO: Create sorting groups
	public static List<TooltipLine> Sort(List<TooltipLine> tooltips)
	{
		return tooltips.OrderBy(t => t.Name == "ItemName" ? -1 : 0)
			.ThenBy(t => t.Name == "Favorite" ? -1 : 0)
			.ThenBy(t => t.Name == "FavoriteDesc" ? -1 : 0)
			.ThenBy(t => t.Name == "NoTransfer" ? -1 : 0)
			.ThenBy(t => t.Name == "Social" ? -1 : 0)
			.ThenBy(t => t.Name == "SocialDesc" ? -1 : 0)

			.ThenBy(t => t.Name == "Ammo" ? -1 : 0)
			.ThenBy(t => t.Name == "Equipable" ? -1 : 0)
			.ThenBy(t => t.Name == "Vanity" ? -1 : 0)
			.ThenBy(t => t.Name == "VanityLegal" ? -1 : 0)

			.ThenBy(t => t.Name == "Damage" ? -1 : 0)
			.ThenBy(t => t.Name == "CritChance" ? -1 : 0)
			.ThenBy(t => t.Name == "Speed" ? -1 : 0)
			.ThenBy(t => t.Name == "Knockback" ? -1 : 0)
			.ThenBy(t => t.Name == "Defense" ? -1 : 0)
			.ThenBy(t => t.Name == "UseMana" ? -1 : 0)
			.ThenBy(t => t.Name == "PickPower" ? -1 : 0)
			.ThenBy(t => t.Name == "AxePower" ? -1 : 0)
			.ThenBy(t => t.Name == "HammerPower" ? -1 : 0)
			.ThenBy(t => t.Name == "FishingPower" ? -1 : 0)
			.ThenBy(t => t.Name == "BaitPower" ? -1 : 0)
			.ThenBy(t => t.Name == "TileBoost" ? -1 : 0)
			.ThenBy(t => t.Name == "HealLife" ? -1 : 0)
			.ThenBy(t => t.Name == "HealMana" ? -1 : 0)
			.ThenBy(t => t.Name == "WellFedExpert" ? -1 : 0)
			.ThenBy(t => t.Name == "BuffTime" ? -1 : 0)
			.ThenBy(t => t.Name == "Affects" && t.Mod == "RealThrowing" ? -1 : 0)

			.ThenBy(t => t.Name == "Consumable" ? -1 : 0)
			.ThenBy(t => t.Name == "Placeable" ? -1 : 0)
			.ThenBy(t => t.Name == "Material" ? -1 : 0)

			.ThenBy(t => t.Name == "Quest" ? -1 : 0)
			.ThenBy(t => t.Name == "Expert" ? -1 : 0)
			.ThenBy(t => t.Name == "Master" ? -1 : 0)

			.ThenBy(t => t.Name == "JourneyResearch" ? -1 : 0)
			.ThenBy(t => t.Name == "BestiaryNotes" ? -1 : 0)
			.ThenBy(t => t.Name == "OneDropLogo" ? -1 : 0)

			.ThenBy(t => t.Name.StartsWith("Tooltip") ? -1 : 0)

			.ThenBy(t => t.Name == "SetBonus" ? -1 : 0)

			.ThenBy(t => t.Name.StartsWith("Prefix") ? -1 : 0)

			.ThenBy(t => t.Name == "NeedsBait" ? -1 : 0)
			.ThenBy(t => t.Name == "WandConsumes" ? -1 : 0)
			.ThenBy(t => t.Name == "EtherianManaWarning" ? -1 : 0)
			.ToList();
	}
}

