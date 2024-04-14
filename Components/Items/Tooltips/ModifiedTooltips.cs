using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using DALib.Extensions;
using DALib.Functions;

namespace DAMod.Components.Items.Tooltips;

public class ModifiedTooltips : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => lateInstantiation;

    static Dictionary<string, Color> ClassColors = new(){
        {"Generic",   new Vector4(000 / 360f, 0.0f, 0.59f, 1f).ToRGB()}, // Gray
		{"Melee",    new Vector4(000 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Red
		{"Ranged",  new Vector4(120 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Green
		{"Magic",    new Vector4(240 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Blue
		{"Summon",  new Vector4(180 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Cyan

		{"Throwing",  new Vector4(030 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Orange

		{"Symphonic", new Vector4(300 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Magenta
		{"Radiant",   new Vector4(060 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Yellow

		{"Void Magic",   new Vector4(270 / 360f, 1.0f, 0.80f, 1f).ToRGB()}, // Purple
	};

    static Player player => Main.LocalPlayer;
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        float knockBack = item.GetKnockBack(player, true);
        int index = 0;

        // Coin Colors
        if ((index = tooltips.FindIndex(t => t.Name == "ItemName")) != -1)
        {
            if (item.type == ItemID.CopperCoin) tooltips[index].OverrideColor = Colors.CoinCopper;
            if (item.type == ItemID.SilverCoin) tooltips[index].OverrideColor = Colors.CoinSilver;
            if (item.type == ItemID.GoldCoin) tooltips[index].OverrideColor = Colors.CoinGold;
            if (item.type == ItemID.PlatinumCoin) tooltips[index].OverrideColor = Colors.CoinPlatinum;
        }

        // Item Class Tooltip
        if ((index = tooltips.FindIndex(t => t.Name == "Damage")) != -1 && item.DamageType.Type > 0)
        {
            int damage = item.GetDamage(player, true);
            if (item.useAmmo > 0)
            {
                Item ammo = item.GetAmmo(player);
                if (ammo.type != ItemID.None)
                {
                    damage += ammo.GetDamage(player, true);
                }
            }

            tooltips[index].Text = string.Format("{0}{1}", damage, Lang.tip[55]);

            string damageType = item.DamageType.Type > 1 ? item.DamageType.DisplayName.Replace(" damage", "").FirstCharToUpper() : "Generic";
            // Secrets of the Shadow Void Magic translation fix
			if (item.ModItem != null) if (item.ModItem.Mod.Name == "SOTS" && item.DamageType.DisplayName == "Mods.SOTS.DamageClassName.VoidMagic") damageType = "Void Magic";

			if (!ClassColors.TryGetValue(damageType, out Color damageColor))
            {
                damageColor = ((Colors._liquidColors[(item.DamageType.Type - 1) % Colors._liquidColors.Length]).ToHSL() + new Vector4(0, 0, 1f / 3f, 0f)).ToRGB();
            }
            TooltipLine tooltip = new(Mod, "DamageType", string.Format("[{0}]", damageType))
            {
                OverrideColor = damageColor
            };
            tooltips.Insert(index, tooltip);
        }

        if ((index = tooltips.FindIndex(t => t.Name == "FishingPower")) != -1)
        {
            tooltips[index].Text = Language.GetTextValue("GameUI.PrecentFishingPower", item.fishingPole + player.fishingSkill);
        }

        if ((index = tooltips.FindIndex(t => t.Name == "Speed")) != -1)
        {
            // Replace Speed tooltip with Channeled text
            if (item.channel)
            {
                tooltips[index].Text = "Channeled";
            }
        }

        if ((index = tooltips.FindIndex(t => t.Name == "UseMana")) != -1)
        {
            tooltips[index].Text = string.Format("{0} mana cost", (int)(item.mana * player.manaCost));
        }


        if (item.DamageType == DamageClass.Default)
        {
            tooltips.RemoveAll(t => t.Name == "CritChance");
            tooltips.RemoveAll(t => t.Name == "Knockback");
        }


        // Remove favourite description tooltip
        tooltips.RemoveAll(t => t.Name == "FavoriteDesc");

        // Remove worn in vanity tooltip
        tooltips.RemoveAll(t => t.Name == "VanityLegal");


        // Colour Tooltips
        if ((index = tooltips.FindIndex(t => t.Name == "Favorite")) != -1)
        {
            tooltips[index].OverrideColor = Colors.RarityBlue.ShiftHue(90);
        }
        if ((index = tooltips.FindIndex(t => t.Name == "Social")) != -1)
        {
            tooltips[index].OverrideColor = Colors.RarityBlue.MinColor();
        }
        if ((index = tooltips.FindIndex(t => t.Name == "SocialDesc")) != -1)
        {
            tooltips[index].OverrideColor = Colors.RarityBlue.MinColor();
        }
        if ((index = tooltips.FindIndex(t => t.Name == "Expert")) != -1)
        {
            tooltips[index].OverrideColor = Main.DiscoColor;
        }
        if ((index = tooltips.FindIndex(t => t.Name == "Master")) != -1)
        {
            tooltips[index].OverrideColor = new Color(255, (byte)(Main.masterColor * 200), 0);
        }
        if ((index = tooltips.FindIndex(t => t.Name == "Quest")) != -1)
        {
            tooltips[index].OverrideColor = Colors.RarityAmber;
        }

        // Mod Name tooltip
        if (ModContent.GetModItem(item.type) != null)
        {
            string modName = ModContent.GetModItem(item.type).Mod.Name;
            TooltipLine tooltip = new(Mod, "ModName", string.Format("[{0}]", modName));
            int num = 0;
            foreach (char c in modName) num += c;
            tooltip.OverrideColor = new Vector4(num % 360 / 360f, 1.0f, 0.90f, 1f).ToRGB();
            tooltips.Add(tooltip);
        }

        //	// Remove thorium class tags
        //	tooltips.RemoveAll(t => t.Name == "BardTag");
        //	tooltips.RemoveAll(t => t.Name == "HealerTag");
        //	tooltips.RemoveAll(t => t.Name == "ThrowerTag");
    }
}

