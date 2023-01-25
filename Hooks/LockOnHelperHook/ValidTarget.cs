using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Hooks.LockOnHelperHook {
    class ValidTarget : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(ValidTargetMethod, Override_ValidTarget);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(ValidTargetMethod, Override_ValidTarget); } catch {}
		}

		static MethodInfo ValidTargetMethod => typeof(LockOnHelper).GetMethod("ValidTarget", BindingFlags.NonPublic | BindingFlags.Static);
		delegate bool OrigValidTarget(NPC n);

		// Don't lock on to critters if you can't hurt them
		// Don't lock on to anything if you aren't using a weapon
		static bool Override_ValidTarget(OrigValidTarget ValidTarget, NPC n) {
			if (n == null || !n.active || n.dontTakeDamage || n.friendly || n.isLikeATownNPC || n.life < 1 || n.immortal) {
				return false;
			}
			if (n.aiStyle == 25 && n.ai[0] == 0f) {
				return false;
			}
			if (Main.LocalPlayer.dontHurtCritters && NPCID.Sets.CountsAsCritter[n.type]) {
				return false;
			}
			if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].damage <= 0 || (Main.mouseItem.type != 0 && Main.mouseItem.damage <= 0)) {
				return false;
			}
			return true;
		}
	}
}
