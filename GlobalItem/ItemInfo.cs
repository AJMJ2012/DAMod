using Terraria;
using Terraria.ModLoader;

namespace DAMod {
	public class ItemInfo : GlobalItem {
		public override bool InstancePerEntity { get { return true; } }
		public override ItemInfo Clone(Item item, Item itemClone) => (ItemInfo)this.MemberwiseClone();

		public int originalUseAnimation = -1;
		public int originalUseTime = -1;
		public int originalUseStyle = -1;
	}
}