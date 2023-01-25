using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Hooks.SummonDamageClassHook {
	class ShowStatTooltipLine : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(ShowStatTooltipLineMethod, Override_ShowStatTooltipLine);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(ShowStatTooltipLineMethod, Override_ShowStatTooltipLine); } catch {}
		}

		static MethodInfo ShowStatTooltipLineMethod => typeof(SummonDamageClass).GetMethod("ShowStatTooltipLine", BindingFlags.Public | BindingFlags.Instance);
		delegate bool OrigShowStatTooltipLine(SummonDamageClass instance, Player player, string lineName);

		// Show speed for summon weapons
		static bool Override_ShowStatTooltipLine(OrigShowStatTooltipLine ShowStatTooltipLine, SummonDamageClass instance, Player player, string lineName) {
			return lineName != "CritChance";
		}
	}
}