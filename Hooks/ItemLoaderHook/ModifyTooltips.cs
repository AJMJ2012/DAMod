using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace DAMod.Hooks.ItemLoaderHook {
	class ModifyTooltips : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(ModifyTooltipsMethod, Override_ModifyTooltips);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(ModifyTooltipsMethod, Override_ModifyTooltips); } catch {}
		}

		static MethodInfo ModifyTooltipsMethod => typeof(ItemLoader).GetMethod("ModifyTooltips", BindingFlags.Public | BindingFlags.Static);
		delegate List<TooltipLine> OrigModifyTooltips(Item item, ref int numTooltips, string[] names, ref string[] text, ref bool[] modifier, ref bool[] badModifier, ref int oneDropLogo, out Color?[] overrideColor);

		// Sort Vanilla Tooltips
		// Add Separators
		// Should use IL
		static List<TooltipLine> Override_ModifyTooltips(OrigModifyTooltips ModifyTooltips, Item item, ref int numTooltips, string[] names, ref string[] text, ref bool[] modifier, ref bool[] badModifier, ref int oneDropLogo, out Color?[] overrideColor) {
			List<TooltipLine> tooltips = new List<TooltipLine>();
			for (int j = 0; j < numTooltips; j++) {
				TooltipLine tooltip = (TooltipLine)(typeof(TooltipLine).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new Type[]{typeof(string), typeof(string)}).Invoke(new object[]{names[j], text[j]}));
				tooltip.IsModifier = modifier[j];
				tooltip.IsModifierBad = badModifier[j];
				if (j == oneDropLogo) {
					tooltip.GetType().GetField("OneDropLogo", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tooltip, true);
				}
				tooltips.Add(tooltip);
			}

			// Sort vanilla tooltips
			tooltips = tooltips.OrderBy(t => t.Name == "ItemName" ? -1 : 0)
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

			item.ModItem?.ModifyTooltips(tooltips);
			if (!item.IsAir) {
				HookList<GlobalItem>.InstanceEnumerator enumerator = ((HookList<GlobalItem>)typeof(ItemLoader).GetField("HookModifyTooltips", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).Enumerate((Instanced<GlobalItem>[])item.GetType().GetField("globalItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item)).GetEnumerator();
				while (enumerator.MoveNext()) {
					enumerator.Current.ModifyTooltips(item, tooltips);
				}
			}

			// Remove any potential blank but still added tooltips
			for (int i = 1; i < tooltips.Count; i++) {
				if (string.IsNullOrWhiteSpace(tooltips[i].Text)) {
					tooltips.RemoveAt(i);
				}
			}
/*
			// Add Separators
			// Currently wonky. Doesn't seem to remove all duplicates. Dunno why.
			{
				AddPostSeparator(ref tooltips, new string[]{"ItemName", "Favorite", "FavoriteDesc", "NoTransfer", "Social", "SocialDesc"});
				AddPostSeparator(ref tooltips, new string[]{"Ammo", "Equipable", "Vanity", "VanityLegal"});
				// Stats will be skipped here. Any above need to be reversed.
				AddPreSeparator(ref tooltips, new string[]{"Consumable", "Placeable", "Material"});
				AddPreSeparator(ref tooltips, new string[]{"Quest", "Expert", "Master"});
				AddPreSeparator(ref tooltips, new string[]{"JourneyResearch", "BestiaryNotes", "OneDropLogo"});
				AddSeparator(ref tooltips, "Tooltip", true);
				AddSeparator(ref tooltips, "SetBonus", false);
				AddSeparator(ref tooltips, "Prefix", true);
				AddPreSeparator(ref tooltips, new string[]{"NeedsBait", "WandConsumes", "EtherianManaWarning"});

				// Remove excess separators
				for (int i = 1; i < tooltips.Count; i++) {
					if (tooltips[i].Name == "Separator" && (tooltips[i-1].Name == "Separator" || i == tooltips.Count-1)) {
						tooltips.RemoveAt(i);
						i = 1;
					}
				}
			}
*/
			numTooltips = tooltips.Count;
			text = new string[numTooltips];
			modifier = new bool[numTooltips];
			badModifier = new bool[numTooltips];
			oneDropLogo = -1;
			overrideColor = new Color?[numTooltips];
			for (int i = 0; i < numTooltips; i++) {
				text[i] = tooltips[i].Text;
				modifier[i] = tooltips[i].IsModifier;
				badModifier[i] = tooltips[i].IsModifierBad;
				if ((bool)tooltips[i].GetType().GetField("OneDropLogo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(tooltips[i])) {
					oneDropLogo = i;
				}
				overrideColor[i] = tooltips[i].OverrideColor;
			}

			return tooltips;
		}

		static void AddPostSeparator(ref List<TooltipLine> tooltips, string[] strings) {
			Array.Reverse(strings);
			int index = -1;
			foreach (string str in strings) {
				if ((index = tooltips.FindIndex(t => t.Name == str)) != -1) {
					tooltips.Insert(++index, new TooltipLine(DAMod.instance, "Separator", " "));
					break;
				}
			}
			Array.Reverse(strings);
			foreach (string str in strings) {
				if ((index = tooltips.FindIndex(t => t.Name == str)) != -1) {
					tooltips.Insert(++index, new TooltipLine(DAMod.instance, "Separator", " "));
					break;
				}
			}
		}

		static void AddPreSeparator(ref List<TooltipLine> tooltips, string[] strings) {
			int index = -1;
			foreach (string str in strings) {
				if ((index = tooltips.FindIndex(t => t.Name == str)) != -1) {
					tooltips.Insert(index, new TooltipLine(DAMod.instance, "Separator", " "));
					break;
				}
			}
			Array.Reverse(strings);
			foreach (string str in strings) {
				if ((index = tooltips.FindIndex(t => t.Name == str)) != -1) {
					tooltips.Insert(index, new TooltipLine(DAMod.instance, "Separator", " "));
					break;
				}
			}
		}

		static void AddSeparator(ref List<TooltipLine> tooltips, string str, bool loose = false) {
			int index = -1;
			if (loose) {
				if ((index = tooltips.FindIndex(t => t.Name.StartsWith(str))) != -1) {
					tooltips.Insert(index, new TooltipLine(DAMod.instance, "Separator", " "));
				}
				if ((index = tooltips.FindLastIndex(t => t.Name.StartsWith(str))) != -1) {
					tooltips.Insert(++index, new TooltipLine(DAMod.instance, "Separator", " "));
				}
			}
			else {
				if ((index = tooltips.FindIndex(t => t.Name == str)) != -1) {
					tooltips.Insert(index, new TooltipLine(DAMod.instance, "Separator", " "));
					tooltips.Insert(++index, new TooltipLine(DAMod.instance, "Separator", " "));
				}
			}
		}
	}
}


