using DALib.Functions;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
// An Experiment
namespace DAMod;

public class DPSTooltip : GlobalItem {
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => lateInstantiation;

	static Player player => Main.LocalPlayer;
	static int roundDecimals => /*Config.Client.ExtendedTooltips ? 3 : */Config.Client.DetailedTooltips ? 1 : 0;

	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		// DPS tooltip for non minion/sentry weapons
		int index = 0;
		if (item.useStyle > 0 && item.damage > 0 && !item.channel && (index = tooltips.FindIndex(t => t.Name == "Damage")) != -1) {
			Projectile projectile = new Projectile();
			if (item.shoot > 0) projectile.SetDefaults(item.shoot);
			if (projectile.minionSlots == 0 && !projectile.sentry) {
				float meleeDPSRound = (float)Math.Round(GetDPS(item, player, true), roundDecimals);
				float projectileDPSRound = (float)Math.Round(GetDPS(item, player, false), roundDecimals);
				tooltips.Insert(index+1, new TooltipLine(Mod, "DPS", string.Format("{0} DPS", !item.noMelee ? item.shoot > 0 ? string.Format("{0} + {1}", meleeDPSRound, projectileDPSRound) : meleeDPSRound : projectileDPSRound)));
			}
		}
		// Remove any existing DPS tooltip from any other mod
		tooltips.RemoveAll(t => t.Mod != Mod.Name && t.Name.Equals("DPS", StringComparison.OrdinalIgnoreCase));
	}

	// TODO: Get proper damage of projectiles with reduced damage.
	public static float GetDPS(Item item, Player player, bool melee, bool calculated = true, bool withAmmo = true, bool withCrit = true) {
		int damage = item.GetDamage(player, calculated);
		int ammoDamage = withAmmo ? item.GetAmmoDamage(player, calculated) : 0;
		int useTime = item.GetUseTime(player, calculated);
		int useAnimation = item.GetUseAnimation(player, calculated);
		int crit = withCrit ? item.GetCrit(player, calculated) : 0;
		float critMix = (float)System.Math.Min(MathFunctions.Mix(1.0, 2.0, crit / 100.0), 2.0);
		return (damage + ammoDamage) / (((melee ? useAnimation : useTime) + item.reuseDelay) / 60f) * critMix;
	}
}

