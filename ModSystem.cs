using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.Initializers;
using Terraria.ModLoader;
using Terraria.UI;

namespace DAMod {
	public class DAModSystem : ModSystem {
		public UIResourcePackSelectionMenu ResourcePackMenu;
		public override void PostUpdateInput() {
			if (!Main.gameMenu && DAMod.instance.resourcePackKey.JustPressed) {
				UIState lastState = Main.InGameUI.CurrentState;
				ResourcePackMenu = new UIResourcePackSelectionMenu(lastState, Main.AssetSourceController, AssetInitializer.CreateResourcePackList(Main.instance.Services));
				Main.inFancyUI = true;
				Main.InGameUI.SetState(ResourcePackMenu);
			}

			if (Main.UIScale < Main.ForcedMinimumZoom) {
				Main.UIScale = Main.ForcedMinimumZoom;
				Main.temporaryGUIScaleSlider = Main.UIScale;
			}
		}
	}
}