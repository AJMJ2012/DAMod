using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace DAMod {
	public class BaitPowerTooltip : GlobalItem {
		static Player player => Main.LocalPlayer;

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			int index = 0;
			if (item.useStyle > 0 && (index = tooltips.FindIndex(t => t.Name == "FishingPower")) != -1) {
				Item bait = Functions.Items.GetBait(item, player);
				int baitPower = bait.bait;
				if (baitPower > 0) {
					tooltips.Insert(index+1, new TooltipLine(Mod, "BaitPower", string.Format("{0}% {1}{2}", baitPower, bait.Name, " power")));
					tooltips.RemoveAll(t => t.Name == "NeedsBait");
				}
			}
		}
	}
}
