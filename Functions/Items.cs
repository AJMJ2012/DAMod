using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.Functions {
	public class Items {

		public static int GetDamage(Item item, Player player, bool calculated = true) {
			int damage = item.damage;
			if (calculated)
				damage = player.GetWeaponDamage(item, true);
			return damage;
		}

		public static int GetCrit(Item item, Player player, bool calculated = true) {
			int crit = item.crit;
			if (calculated)
				crit = player.GetWeaponCrit(item);
			return crit;
		}

		public static int GetUseTime(Item item, Player player, bool calculated = true) {
			int useTime = item.useTime;
			if (calculated)
				useTime = CombinedHooks.TotalUseTime(item.useTime, player, item);
			return useTime;
		}

		public static int GetUseAnimation(Item item, Player player, bool calculated = true) {
			int useAnimation = item.useAnimation;
			if (calculated)
				useAnimation = CombinedHooks.TotalAnimationTime(item.useAnimation, player, item);
			return useAnimation;
		}

		public static int GetReuseDelay(Item item, Player player, bool calculated = true) {
			int reuseDelay = item.reuseDelay;
			return reuseDelay;
		}

		public static float GetKnockBack(Item item, Player player, bool calculated = true) {
			float knockBack = item.knockBack;
			if (calculated)
				knockBack = player.GetWeaponKnockback(item, item.knockBack);
			return knockBack;
		}

		public static int GetMana(Item item, Player player, bool calculated = true) {
			int mana = item.mana;
			if (calculated)
				mana = (int)(item.mana * player.manaCost);
			return mana;
		}

		public static Item GetAmmo(Item item, Player player) => player.ChooseAmmo(item) ?? new Item();

		public static int GetAmmoDamage(Item item, Player player, bool calculated = true) => item.useAmmo > 0 ? GetDamage(GetAmmo(item, player), player, calculated) : 0;

		// TODO: Get projectile count of items.
		public static int GetProjectileCount(Item item) {
			return 1;
		}

		// TODO: Get proper damage of projectiles with reduced damage.
		public static float GetDPS(Item item, Player player, bool melee, bool calculated = true, bool withAmmo = true, bool withCrit = true) {
			int damage = GetDamage(item, player, calculated);
			int ammoDamage = withAmmo ? GetAmmoDamage(item, player, calculated) : 0;
			int useTime = GetUseTime(item, player, calculated);
			int useAnimation = GetUseAnimation(item, player, calculated);
			int crit = withCrit ? GetCrit(item, player, calculated) : 0;
			float critMix = (float)System.Math.Min(Functions.Math.Mix(1.0, DAMod.critDamageMult, crit / 100.0), DAMod.critDamageMult);
			return (damage + ammoDamage) / (((melee ? useAnimation : useTime) + item.reuseDelay) / 60f) * critMix;
		}

		public static int GetBurstCount(Item item, Player player, bool calculated = true) {
			int useTime = GetUseTime(item, player, calculated);
			int useAnimation = GetUseAnimation(item, player, calculated);
			return !item.channel ? System.Math.Max(useAnimation / useTime, 0) : 0;
		}

		public static int GetRechargeTime(Item item, Player player, bool calculated = true) {
			int useTime = GetUseTime(item, player, calculated);
			int useAnimation = GetUseAnimation(item, player, calculated);
			return !item.channel ? System.Math.Max(useTime - useAnimation, 0) : 0;
		}

		public static Projectile GetProjectile(Item item, Player player) {
			Projectile projectile = new Projectile();
			projectile.SetDefaults(item.shoot);
			return projectile;
		}

		public static Item GetBait(Item item, Player player) {
			Item bait = null;
			object[] args = new object[]{item};
			player.GetType().GetMethod("Fishing_GetBait", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(player, args);
			bait = (Item)args[0];
			return bait ?? new Item();
		}

		public static bool IsTool(Item item) {
			return item.pick > 0 || item.axe > 0 || item.hammer > 0;
		}
	}
}