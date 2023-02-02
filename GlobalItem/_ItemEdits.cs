using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using DALib.Checks;

namespace DAMod {
	public class _ItemEdits : GlobalItem {

		public override void SetDefaults(Item item) {
			Projectile projectile = new Projectile();
			projectile.SetDefaults(item.shoot);

			switch (item.type) {
				// Make informational accessories non equipable
				case ItemID.CopperWatch:
				case ItemID.TinWatch:
				case ItemID.SilverWatch:
				case ItemID.TungstenWatch:
				case ItemID.GoldWatch:
				case ItemID.PlatinumWatch:
				case ItemID.DepthMeter:
				case ItemID.Compass:
				case ItemID.GPS:
				case ItemID.FishermansGuide:
				case ItemID.WeatherRadio:
				case ItemID.Sextant:
				case ItemID.FishFinder:
				case ItemID.MetalDetector:
				case ItemID.Stopwatch:
				case ItemID.DPSMeter:
				case ItemID.GoblinTech:
				case ItemID.TallyCounter:
				case ItemID.LifeformAnalyzer:
				case ItemID.Radar:
				case ItemID.REK:
				case ItemID.PDA:
				case ItemID.MechanicalLens:
					item.accessory = false;
					item.Prefix(-3);
					break;
				// Autoreuse for Magic Dagger
				case ItemID.MagicDagger:
					item.autoReuse = true;
					item.useTime += 2;
					item.useAnimation += 2;
					break;
			}

			// Make every accessory allowed to be equipped into vanity slots
			// Remove for 1.4.4
			if (item.accessory) {
				item.canBePlacedInVanityRegardlessOfConditions = true;
			}

			// Convert clouds to Summon damage
			if (projectile.aiStyle == ProjAIStyleID.RainCloud) {
				item.DamageType = DamageClass.Summon;
			}

			// Increase ammo stack
			if (item.IsNormalAmmo()) {
				item.maxStack = 9999;
			}

			// Reduce overall knockback
			item.knockBack /= 2;

			if (!item.channel) {
				// Nailgun is faster but less powerful
				if (item.type == ItemID.NailGun) {
					item.damage = (int)(item.damage / 2f);
					item.knockBack = (int)(item.knockBack / 2f);
					item.useAnimation = (int)(item.useAnimation / 2f);
					item.useTime = (int)(item.useTime / 2f);
				}

				if (item.CountsAsClass(DamageClass.Magic)) {
					// Magnet Sphere is more powerful but slower
					if (item.type == ItemID.MagnetSphere) {
						item.damage = (int)(item.damage * 1.5f);
						item.knockBack = (int)(item.knockBack * 1.5f);
						item.useAnimation = (int)(item.useAnimation * 1.5f);
						item.useTime = (int)(item.useTime * 1.5f);
						item.mana = (int)(item.mana * 1.5f);
					}
					// Magic weapons are faster but less powerful
					else {
						item.damage = (int)(item.damage / 1.5f);
						item.knockBack = (int)(item.knockBack / 1.5f);
						item.useAnimation = (int)(item.useAnimation / 1.5f);
						item.useTime = (int)(item.useTime / 1.5f);
						item.mana = (int)(item.mana / 1.5f);
					}
				}

				if (item.CountsAsClass(DamageClass.Ranged)) {
					// Rocket based ranged weapons are more powerful but slower
					if ((item.useAmmo == AmmoID.Rocket || item.useAmmo == AmmoID.JackOLantern || item.useAmmo == AmmoID.StyngerBolt)) {
						item.damage = (int)(item.damage * 2f);
						item.knockBack = (int)(item.knockBack * 2f);
						item.useAnimation = (int)(item.useAnimation * 2f);
						item.useTime = (int)(item.useTime * 2f);
					}
					// Crossbows and Repeaters are more poweful but slower
					else if ((item.Name.Contains("Repeater") || item.Name.Contains("Crossbow") || item.type == ItemID.ChlorophyteShotbow || item.type == ItemID.StakeLauncher)) {
						item.damage = (int)(item.damage * 2f);
						item.knockBack = (int)(item.knockBack * 2f);
						item.useAnimation = (int)(item.useAnimation * 2f);
						item.useTime = (int)(item.useTime * 2f);
					}
					// Bows are more powerful but slower
					else if (item.useAmmo == AmmoID.Arrow) {
						item.damage = (int)(item.damage * 1.5f);
						item.knockBack = (int)(item.knockBack * 1.5f);
						item.useAnimation = (int)(item.useAnimation * 1.5f);
						item.useTime = (int)(item.useTime * 1.5f);
					}
				}
			}

			// Ammo is has no damage class if it can't be used.
			if (item.IsNonUseableAmmo()) {
				item.DamageType = DamageClass.Default;
			}

			if (item.CountsAsClass(DamageClass.Melee)) {
				item.autoReuse = true;
			}

			// Replace the terrible Space Gun sound
			if (item.UseSound == SoundID.Item157) item.UseSound = SoundID.Item12;

			// Round out item max stacks to next 5
			if (item.maxStack > 5) {
				item.maxStack = (int)(Math.Ceiling(item.maxStack / 5f) * 5);
			}

			// Make throwing animations use swing but also make the swing not slow
			if ((item.useStyle == ItemUseStyleID.Shoot && item.noUseGraphic)) {
				item.useStyle = ItemUseStyleID.Swing;
				int newAnimation = Math.Min(item.useAnimation, 15);
				item.reuseDelay += item.useAnimation - newAnimation;
				item.useAnimation = newAnimation;
			}

			// Tools are only as fast as their animation
			// But elsewhere I made them hit twice
			if (item.IsDestructiveTool()) {
				item.useTime = item.useAnimation;
			}

			// Tools are generic
			if (item.IsDestructiveTool()){
				item.DamageType = DamageClass.Generic;
			}
		}

		// Nerf crit damage
		// TODO: Find a better way to do this
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) {
			if (crit) damage = (int)((float)(damage / 2) * DAMod.critDamageMult);
		}
		public override void ModifyHitPvp(Item item, Player player, Player target, ref int damage, ref bool crit) {
			if (crit) damage = (int)((float)(damage / 2) * DAMod.critDamageMult);
		}
	}
}
