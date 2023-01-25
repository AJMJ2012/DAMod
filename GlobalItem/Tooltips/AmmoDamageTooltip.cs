using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class AmmoDamageTooltip : GlobalItem {
		static Player player => Main.LocalPlayer;
/*
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			int index = 0;
			Item ammo = Functions.Items.GetAmmo(item, player);
			if (ammo.type != 0) {
				if (item.useAmmo > 0 && (index = tooltips.FindIndex(t => t.Name == "Damage")) != -1) {
					int ammoDamage = Functions.Items.GetDamage(ammo, player, true);
					if (ammoDamage > 0) {
						tooltips.Insert(index+1, new TooltipLine(Mod, "AmmoDamage", string.Format("{0} {1} {2}", ammoDamage, ammo.Name, "damage")));
					}
				}
			}
			else {
				if ((index = tooltips.FindIndex(t => t.Name == "Knockback")) != -1 || (index = tooltips.FindIndex(t => t.Name == "Speed")) != -1 || (index = tooltips.FindIndex(t => t.Name == "Damage")) != -1) {
					int useAmmo = item.useAmmo;
					if (useAmmo == AmmoID.Dart)
						useAmmo = ItemID.PoisonDart;
					Item ammoItem = new Item();
					ammoItem.SetDefaults(useAmmo);
					tooltips.Insert(index+1, new TooltipLine(Mod, "RequiresAmmo", string.Format("{0} {1} {2}", "Requires", ammoItem.Name, "for ammo")));
				}
			}
		}
	}
*/

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			int index = 0;
			if (item.useAmmo > 0) {
				if ((index = tooltips.FindIndex(t => t.Name == "OneDropLogo")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "BuffTime")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "WellFedExpert")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "EtherianManaWarning")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Tooltip3")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Tooltip2")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Tooltip1")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Material")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Consumable")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Ammo")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Placeable")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "UseMana")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "HealMana")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "HealLife")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "TileBoost")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "HammerPower")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "AxePower")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "PickPower")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Defense")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Vanity")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Quest")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "WandConsumes")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Equipable")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "BaitPower")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "NeedsBait")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "FishingPower")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Knockback")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Speed")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "CritChance")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Damage")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "SocialDesc")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Social")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "FavoriteDesc")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "Favorite")) != -1 || 
					(index = tooltips.FindIndex(t => t.Name == "ItemName")) != -1) {
					Item ammo = Functions.Items.GetAmmo(item, player);
					if (ammo.type != 0) {
						int ammoDamage = Functions.Items.GetDamage(ammo, player, true);
						if (ammoDamage > 0) {
							TooltipLine tooltip = new TooltipLine(Mod, "AmmoDamage", string.Format("+{0} {1} {2}", ammoDamage, ammo.Name, "damage"));
							tooltip.IsModifier = true;
							tooltips.Insert(index+1, tooltip);
						}
					}
					else {
						int useAmmo = item.useAmmo;
						if (useAmmo == AmmoID.Dart)
							useAmmo = ItemID.PoisonDart;
						Item ammoItem = new Item();
						ammoItem.SetDefaults(useAmmo);
						TooltipLine tooltip = new TooltipLine(Mod, "RequiresAmmo", string.Format("{0} {1} {2}", "Requires", ammoItem.Name, "for ammo"));
						tooltip.IsModifier = true;
						tooltip.IsModifierBad = true;
						tooltips.Insert(index+1, tooltip);
					}
				}
			}
		}
	}
}
