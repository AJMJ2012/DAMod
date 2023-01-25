using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class DetailedTooltips : GlobalItem {
		static Player player => Main.LocalPlayer;
		static int roundDecimals => Config.Client.ExtendedTooltips ? 3 : Config.Client.DetailedTooltips ? 1 : 0;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (Config.Client.DetailedTooltips) {
				int index = 0;
				if ((index = tooltips.FindIndex(t => t.Name == "Speed")) != -1) {
					// Detailed Speed Tooltips
					int useAnimation = Functions.Items.GetUseAnimation(item, player, true);
					float useAnimationInSeconds = useAnimation / 60f;
					float useAnimationInSecondsRound = (float)Math.Round(useAnimationInSeconds, roundDecimals);
					int useTime = Functions.Items.GetUseTime(item, player, true);
					float useTimeInSeconds = useTime / 60f;
					float useTimeInSecondsRound = (float)Math.Round(useTimeInSeconds, roundDecimals);
					int reuseDelay = Functions.Items.GetReuseDelay(item, player, true);
					float reuseDelayInSeconds = reuseDelay / 60f;
					float reuseDelayInSecondsRound = (float)Math.Round(reuseDelayInSeconds, roundDecimals);
					if (!item.channel) {
						tooltips.RemoveAll(t => t.Name == "Speed");
						index--;
						if (!item.channel && item.useStyle == ItemUseStyleID.Swing && ((!item.noUseGraphic && !item.noMelee) || item.DamageType == DamageClass.SummonMeleeSpeed)) {
							tooltips.Insert(++index, new TooltipLine(Mod, "UseAnimation", string.Format("{0}s swing speed", useAnimationInSecondsRound)));
							if (item.shoot > 0 && item.DamageType != DamageClass.SummonMeleeSpeed) {
								tooltips.Insert(++index, new TooltipLine(Mod, "UseTime", string.Format("{0}s fire rate", useTimeInSecondsRound)));
							}
						}
						else if (!item.channel && item.shoot > 0 && (item.useStyle == ItemUseStyleID.Shoot || (item.useStyle == ItemUseStyleID.Swing && (item.noUseGraphic || item.noMelee)))) {
							tooltips.Insert(++index, new TooltipLine(Mod, "UseTime", string.Format("{0}s fire rate", useTimeInSecondsRound)));
						}
						else {
							tooltips.Insert(++index, new TooltipLine(Mod, "UseAnimation", string.Format("{0}s use speed", useAnimationInSecondsRound)));
						}
					}

					if (Functions.Items.IsTool(item)) {
						tooltips.Insert(++index, new TooltipLine(Mod, "ToolSpeed", string.Format("{0}s tool speed", useTimeInSecondsRound)));
					}
					if (reuseDelayInSeconds > 0) {
						tooltips.Insert(++index, new TooltipLine(Mod, "ReuseDelay", string.Format("{0}s reuse delay", reuseDelayInSecondsRound)));
					}

					// Burst tooltip
					int burst = !item.channel ? Math.Max((int)Math.Ceiling((float)useAnimation / (float)useTime), 0) : 0;
					if (burst > 1) {
						tooltips.Insert(++index, new TooltipLine(Mod, "Burst", string.Format("{0} shot burst", burst)));
					}
					if (item.type == ItemID.ClockworkAssaultRifle) {
						tooltips.RemoveAll(t => t.Name == "Tooltip0");
					}

					// Recharge tooltip
					int recharge = !item.channel ? Math.Max(useTime - useAnimation, 0) : 0;
					if (recharge > 1) {
						float rechargeInSeconds = recharge / 60f;
						float rechargeInSecondsRound = (float)Math.Round(rechargeInSeconds, roundDecimals);
						tooltips.Insert(++index, new TooltipLine(Mod, "Recharge", string.Format("{0}s recharge", rechargeInSecondsRound)));
					}
				}

				// Detailed Knockback Tooltips
				if ((index = tooltips.FindIndex(t => t.Name == "Knockback")) != -1) {
					float knockBack = Functions.Items.GetKnockBack(item, player, true);
					if (knockBack > 0) {
						float knockBackRound = (float)Math.Round(knockBack, roundDecimals);
						tooltips[index].Text = string.Format("{0} knockback", knockBackRound);
					}
				}
			}
		}
	}
}