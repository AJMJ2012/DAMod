using Microsoft.Xna.Framework;
using StringExtensions;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DAMod {
	public class zModifiedTooltips : GlobalItem {

		static Dictionary<string,Color> ClassColors = new Dictionary<string,Color>(){
			{"Generic",   Functions.Colors.HSLToRGB(new Vector3(000 / 360f, 0.0f, 0.59f))}, // Gray
			{"Melee",     Functions.Colors.HSLToRGB(new Vector3(000 / 360f, 1.0f, 0.80f))}, // Red
			{"Ranged",    Functions.Colors.HSLToRGB(new Vector3(120 / 360f, 1.0f, 0.80f))}, // Green
			{"Magic",     Functions.Colors.HSLToRGB(new Vector3(240 / 360f, 1.0f, 0.80f))}, // Blue
			{"Summon",    Functions.Colors.HSLToRGB(new Vector3(180 / 360f, 1.0f, 0.80f))}, // Cyan

			{"Throwing",  Functions.Colors.HSLToRGB(new Vector3(030 / 360f, 1.0f, 0.80f))}, // Orange

			{"Symphonic", Functions.Colors.HSLToRGB(new Vector3(300 / 360f, 1.0f, 0.80f))}, // Magenta
			{"Radiant",   Functions.Colors.HSLToRGB(new Vector3(060 / 360f, 1.0f, 0.80f))}, // Yellow
		};

		static Player player => Main.LocalPlayer;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			float knockBack = Functions.Items.GetKnockBack(item, player, true);
			int index = 0;

			// Coin Colors
			if ((index = tooltips.FindIndex(t => t.Name == "ItemName")) != -1) {
				if (item.type == ItemID.CopperCoin)   tooltips[index].OverrideColor = Colors.CoinCopper;
				if (item.type == ItemID.SilverCoin)   tooltips[index].OverrideColor = Colors.CoinSilver;
				if (item.type == ItemID.GoldCoin)     tooltips[index].OverrideColor = Colors.CoinGold;
				if (item.type == ItemID.PlatinumCoin) tooltips[index].OverrideColor = Colors.CoinPlatinum;
			}

			// Item Class Tooltip
			if ((index = tooltips.FindIndex(t => t.Name == "Damage")) != -1 && item.DamageType.Type > 0) {
				int damage = Functions.Items.GetDamage(item, player, true);
				if (item.useAmmo > 0) {
					Item ammo = Functions.Items.GetAmmo(item, player);
					if (ammo.type != 0) {
						damage += Functions.Items.GetDamage(ammo, player, true);
					}
				}

				tooltips[index].Text = string.Format("{0}{1}", damage, Lang.tip[55]);

				string damageType = item.DamageType.Type > 1 ? item.DamageType.DisplayName.Replace(" damage", "").FirstCharToUpper() : "Generic";
				if (!ClassColors.TryGetValue(damageType, out Color damageColor)) {
					damageColor = Functions.Colors.HSLToRGB(Functions.Colors.RGBToHSL(Colors._liquidColors[(item.DamageType.Type-1) % Colors._liquidColors.Length]) + new Vector3(0, 0, 1f/3f));
				}
				TooltipLine tooltip = new TooltipLine(Mod, "DamageType", string.Format("[{0}]", damageType));
				tooltip.OverrideColor = damageColor;
				tooltips.Insert(index, tooltip);
			}

			if ((index = tooltips.FindIndex(t => t.Name == "FishingPower")) != -1) {
				tooltips[index].Text = Language.GetTextValue("GameUI.PrecentFishingPower", item.fishingPole + player.fishingSkill);
			}

			if ((index = tooltips.FindIndex(t => t.Name == "Speed")) != -1) {
				// Replace Speed tooltip with Channeled text
				if (item.channel) {
					tooltips[index].Text = "Channeled";
				}
			}

			if ((index = tooltips.FindIndex(t => t.Name == "UseMana")) != -1) {
				tooltips[index].Text = string.Format("{0} mana cost", (int)(item.mana * player.manaCost));
			}


			if (item.DamageType == DamageClass.Default) {
				tooltips.RemoveAll(t => t.Name == "CritChance");
				tooltips.RemoveAll(t => t.Name == "Knockback");
			}


			// Remove favourite description tooltip
			tooltips.RemoveAll(t => t.Name == "FavoriteDesc");

			// Remove worn in vanity tooltip
			tooltips.RemoveAll(t => t.Name == "VanityLegal");


			// Colour Tooltips
			if ((index = tooltips.FindIndex(t => t.Name == "Favorite")) != -1) {
				tooltips[index].OverrideColor = Functions.Colors.ShiftHue(Colors.RarityBlue, 90);
			}
			if ((index = tooltips.FindIndex(t => t.Name == "Social")) != -1) {
				tooltips[index].OverrideColor = Functions.Colors.MinColor(Colors.RarityBlue);
			}
			if ((index = tooltips.FindIndex(t => t.Name == "SocialDesc")) != -1) {
				tooltips[index].OverrideColor = Functions.Colors.MinColor(Colors.RarityBlue);
			}
			if ((index = tooltips.FindIndex(t => t.Name == "Expert")) != -1) {
				tooltips[index].OverrideColor = Main.DiscoColor;
			}
			if ((index = tooltips.FindIndex(t => t.Name == "Master")) != -1) {
				tooltips[index].OverrideColor = new Color((byte)255, (byte)(Main.masterColor * 200), (byte)0);
			}
			if ((index = tooltips.FindIndex(t => t.Name == "Quest")) != -1) {
				tooltips[index].OverrideColor = Colors.RarityAmber;
			}

			// Mod Name tooltip
			if (ModContent.GetModItem(item.type) != null) {
				string modName = ModContent.GetModItem(item.type).Mod.Name;
				TooltipLine tooltip = new TooltipLine(Mod, "ModName", string.Format("[{0}]", modName));
				int num = 0;
				foreach(char c in modName) num += (int) c;
				tooltip.OverrideColor = Functions.Colors.HSLToRGB(new Vector3((num % 360) / 360f, 1.0f, 0.90f));
				tooltips.Add(tooltip);
			}

		//	// Remove thorium class tags
		//	tooltips.RemoveAll(t => t.Name == "BardTag");
		//	tooltips.RemoveAll(t => t.Name == "HealerTag");
		//	tooltips.RemoveAll(t => t.Name == "ThrowerTag");
		}
	}
}
