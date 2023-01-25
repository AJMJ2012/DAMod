using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod {
	public class UseTimeModifier : ModPlayer {
		public override void PreUpdate() {
			// Swinging animations that use reuseDelay to speed up the animation no longer re-animate during that delay
			Item selectedItem = Player.inventory[Player.selectedItem];
			if (selectedItem.TryGetGlobalItem(out ItemInfo itemInfo) && ((selectedItem.useStyle == ItemUseStyleID.Swing || itemInfo.originalUseStyle == ItemUseStyleID.Swing) && selectedItem.reuseDelay > 0)) {
				if ((Player.itemTime > 0 && Player.reuseDelay == 0) || (Player.itemAnimation == 1 && Player.reuseDelay > 0)) {
					itemInfo.originalUseStyle = ItemUseStyleID.Swing;
					selectedItem.useStyle = ItemUseStyleID.HiddenAnimation;
				}
				else {
					itemInfo.originalUseStyle = ItemUseStyleID.HiddenAnimation;
					selectedItem.useStyle = ItemUseStyleID.Swing;
				}
			}
		}
		public override float UseTimeMultiplier(Item item) {
			return base.UseTimeMultiplier(item) * base.UseAnimationMultiplier(item) * Player.GetAttackSpeed(item.DamageType);
		}
	}
}