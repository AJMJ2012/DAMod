using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components;

public static class RecipeSorter
{
	public static Recipe[] originalRecipes;

	public static Recipe[] Sort(Recipe[] recipes)
	{

		return Config.Client.SortRecipes switch
		{
			RecipeSort.Yes => PrimarySort(recipes),
			RecipeSort.Alternate => AlternateSort(recipes),
			_ => originalRecipes,
		};
	}

	static (T[], T[]) Split<T>(T[] source, Func<T, bool> predicate)
	{
		var trueList = source.Where(predicate).ToArray<T>();
		var falseList = source.Where(x => !predicate(x)).ToArray<T>();
		return (trueList, falseList);
	}

	static bool isWeapon(Item i) => i.damage > 0 && i.useStyle > ItemUseStyleID.None && (i.shoot > ProjectileID.None || !i.noMelee) && i.createTile < TileID.Dirt && !isTool(i) || isAmmo(i);
	static bool isAmmo(Item i) => i.ammo > 0 && i.CanFillEmptyAmmoSlot() && !i.notAmmo;
	static bool isTool(Item i) => i.pick > 0 || i.axe > 0 || i.hammer > 0 || i.fishingPole > 0 || i.tileWand > 0 || i.type == ItemID.GravediggerShovel || ItemID.Sets.CatchingTool[i.type];
	static bool isBait(Item i) => i.bait > 0;
	static bool isPaint(Item i) => i.paint > 0 || i.paintCoating > 0;
	static bool isArmor(Item i) => (i.headSlot > 0 || i.bodySlot > 0 || i.legSlot > 0) && !i.vanity && !i.accessory && !isAmmo(i) && !isTool(i);
	static bool isAccessory(Item i) => i.accessory && !i.vanity && !Regex.Match(i.Name, "^Music Box", RegexOptions.IgnoreCase).Success;

	static bool isVanity(Item i) => i.vanity && !isTile(i);
	static bool isDye(Item i) => i.dye > 0;
	static bool isHairDye(Item i) => i.hairDye > 0;

	static bool isEquipment(Item i) => isHook(i) || isMount(i) || isKart(i) || isPet(i) || isLightPet(i);
	static bool isHook(Item i) => i.shoot > ProjectileID.None && Main.projHook[i.shoot];
	static bool isMount(Item i) => i.mountType != -1 && !MountID.Sets.Cart[i.mountType];
	static bool isKart(Item i) => i.mountType != -1 && MountID.Sets.Cart[i.mountType];
	static bool isPet(Item i) => i.buffType > 0 && Main.vanityPet[i.buffType];
	static bool isLightPet(Item i) => i.buffType > 0 && Main.lightPet[i.buffType];


	static bool isPotion(Item i) => (i.buffType > 0 || i.healLife > 0 || i.healMana > 0 || Regex.Match(i.Name, "(^| )Potion( |$)", RegexOptions.IgnoreCase).Success) && !isWeapon(i) && !isEquipment(i) && !isFood(i);
	static bool isFood(Item i) => (ItemID.Sets.IsFood[i.type] || i.buffType == BuffID.WellFed || i.buffType == BuffID.WellFed2 || i.buffType == BuffID.WellFed3);

	static bool isTile(Item i) => (i.createTile >= TileID.Dirt || i.createWall >= 0) && i.tileWand <= 0 && i.consumable;

	static int cmp(Item a, Item b, Func<Item, int> f) => f(a).CompareTo(f(b));
	static int cmp(Item a, Item b, Func<Item, string> f) => f(a).CompareTo(f(b));
	static int cmp(Item a, Item b, Func<Item, bool> f) => f(a).CompareTo(f(b));
	static int cmp(Recipe a, Recipe b, Func<Recipe, bool> f) => f(a).CompareTo(f(b));

	static bool isCurrency(Item i) => i.IsCurrency || i.IsACoin;
	static bool isTorch(Item i) => i.createTile == TileID.Torches || i.createTile > TileID.Dirt && TileID.Sets.Torch[i.createTile] || ItemID.Sets.Torches[i.type];
	static bool isPlatform(Item i) => i.createTile == TileID.Platforms || i.createTile > TileID.Dirt && TileID.Sets.Platforms[i.createTile];
	static bool isIngot(Item i) => Regex.Match(i.Name, " Bar$", RegexOptions.IgnoreCase).Success;
	static bool isVanillaDamageClass(Item i) => i.CountsAsClass(DamageClass.Melee) || i.CountsAsClass(DamageClass.Ranged) || i.CountsAsClass(DamageClass.Magic) || i.CountsAsClass(DamageClass.Summon) || i.CountsAsClass(DamageClass.Throwing);

	public static Recipe[] PrimarySort(Recipe[] recipes)
	{
		// Generic Sorting
		int sortStack(Item i, Item j) => cmp(i, j, x => x.stack);
		int sortDamage(Item i, Item j) => cmp(i, j, x => x.damage);
		int sortRare(Item i, Item j) => cmp(i, j, x => x.rare);
		int sortValue(Item i, Item j) => cmp(i, j, x => x.value);
		int sortName(Item i, Item j) => cmp(i, j, x => x.Name);
		int sortID(Item i, Item j) => cmp(i, j, x => x.type);

		int sortCurrency(Item i, Item j)
		{
			int r = sortID(i, j);
			if (r == 0) r = -sortStack(i, j);
			return r;
		}

		int sortTorch(Item i, Item j)
		{
			int r = sortRare(i, j);
			if (r == 0) r = -sortStack(i, j);
			if (r == 0) r = sortID(i, j);
			return r;
		}

		int sortWeapon(Item i, Item j)
		{
			if (isWeapon(i) || isWeapon(j))
			{
				int r = cmp(i, j, isWeapon);
				if (isVanillaDamageClass(i) || isVanillaDamageClass(j))
				{
					if (r == 0) r = -i.CountsAsClass(DamageClass.Melee).CompareTo(j.CountsAsClass(DamageClass.Melee));
					if (r == 0) r = -i.CountsAsClass(DamageClass.Ranged).CompareTo(j.CountsAsClass(DamageClass.Ranged));
					if (r == 0) r = -i.CountsAsClass(DamageClass.Magic).CompareTo(j.CountsAsClass(DamageClass.Magic));
					if (r == 0) r = -i.CountsAsClass(DamageClass.Summon).CompareTo(j.CountsAsClass(DamageClass.Summon));
					if (r == 0) r = -i.CountsAsClass(DamageClass.Throwing).CompareTo(j.CountsAsClass(DamageClass.Throwing));
				}
				if (!isVanillaDamageClass(i) || !isVanillaDamageClass(j))
				{
					if (r == 0) r = -i.DamageType.Type.CompareTo(j.DamageType.Type);
				}
				if (r == 0) r = isAmmo(i).CompareTo(isAmmo(j));
				if (r == 0) r = i.consumable.CompareTo(j.consumable);
				if (r == 0) r = i.useAmmo.CompareTo(j.useAmmo);
				if (r == 0) r = -i.ammo.CompareTo(j.ammo);
				if (r == 0) r = -i.rare.CompareTo(j.rare);
				if (r == 0) r = -i.damage.CompareTo(j.damage);
				return r;
			}
			return 0;
		}

		int sortAmmo(Item i, Item j)
		{
			int r = i.ammo.CompareTo(j.ammo);
			if (r == 0) r = sortRare(i, j);
			if (r == 0) r = sortDamage(i, j);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortArmor(Item i, Item j)
		{
			int r = j.rare.CompareTo(i.rare);
			if (r == 0) r = i.Name.Split(' ').First().CompareTo(j.Name.Split(' ').First());
			if (r == 0) r = -i.headSlot.CompareTo(j.headSlot);
			if (r == 0) r = -i.bodySlot.CompareTo(j.bodySlot);
			if (r == 0) r = -i.legSlot.CompareTo(j.legSlot);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortAccessories(Item i, Item j)
		{
			int r = 0;
			if (r == 0) r = j.rare.CompareTo(i.rare);
			if (r == 0) r = i.Name.Split(' ').Last().CompareTo(j.Name.Split(' ').Last());
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortTools(Item i, Item j)
		{
			int r = 0;
			if (r == 0) r = -i.pick.CompareTo(j.pick);
			if (r == 0) r = -(i.type == ItemID.GravediggerShovel).CompareTo(j.type == ItemID.GravediggerShovel);
			if (r == 0) r = -i.axe.CompareTo(j.axe);
			if (r == 0) r = -i.hammer.CompareTo(j.hammer);
			if (r == 0) r = -i.fishingPole.CompareTo(j.fishingPole);

			if (r == 0) r = i.Name.Split(' ').First().CompareTo(j.Name.Split(' ').First());
			if (r == 0) r = i.useStyle.CompareTo(j.useStyle);

			if (r == 0) r = -i.tileWand.CompareTo(j.tileWand);

			if (r == 0) r = i.rare.CompareTo(j.rare);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortBait(Item i, Item j)
		{
			int r = i.bait.CompareTo(j.bait);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortPaint(Item i, Item j)
		{
			int r = i.paint.CompareTo(j.paint);
			if (r == 0) r = j.paintCoating.CompareTo(j.paintCoating);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortVanity(Item i, Item j)
		{
			int r = 0;
			if (r == 0) r = i.Name.Split(' ').First().CompareTo(j.Name.Split(' ').First());
			if (r == 0) r = -i.headSlot.CompareTo(j.headSlot);
			if (r == 0) r = -i.faceSlot.CompareTo(j.faceSlot);
			if (r == 0) r = -i.neckSlot.CompareTo(j.neckSlot);
			if (r == 0) r = -i.bodySlot.CompareTo(j.bodySlot);
			if (r == 0) r = -i.legSlot.CompareTo(j.legSlot);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortPotion(Item i, Item j)
		{
			int r = j.healLife.CompareTo(i.healLife);
			if (r == 0) r = j.healMana.CompareTo(i.healMana);
			//if (r == 0) r = sortRare(j, i);
			if (r == 0) r = sortName(i, j);
			return r;
		}


		int sortFood(Item i, Item j)
		{
			int r = i.buffType.CompareTo(j.buffType);
			if (r == 0) r = i.buffTime.CompareTo(j.buffTime);
			if (r == 0) r = sortRare(i, j);
			if (r == 0) r = sortName(i, j);
			return r;
		}

		int sortIngot(Item i, Item j)
		{
			int r = sortRare(i, j);
			if (r == 0) r = sortValue(i, j);
			return r;
		}

		if (recipes == null) return null;

		// Split into subcategories

		// Having null items above real items causes the list to end abruptly.
		(Recipe[] none, recipes) = Split(recipes, delegate (Recipe r) { return r.createItem.IsAir; });
		Array.Sort(none, delegate (Recipe x, Recipe y) { return cmp(x, y, r => r.createItem.IsAir); });

		// Category Sorting
		(Recipe[] currency, recipes) = Split(recipes, delegate (Recipe r) { return isCurrency(r.createItem); });
		Array.Sort(currency, delegate (Recipe x, Recipe y) { return sortCurrency(x.createItem, y.createItem); });

		(Recipe[] torches, recipes) = Split(recipes, delegate (Recipe r) { return isTorch(r.createItem); });
		Array.Sort(torches, delegate (Recipe x, Recipe y) { return sortTorch(x.createItem, y.createItem); });

		(Recipe[] weapon, recipes) = Split(recipes, delegate (Recipe r) { return isWeapon(r.createItem); });
		Array.Sort(weapon, delegate (Recipe x, Recipe y) { return sortWeapon(x.createItem, y.createItem); });

		(Recipe[] ammo, recipes) = Split(recipes, delegate (Recipe r) { return isAmmo(r.createItem); });
		Array.Sort(ammo, delegate (Recipe x, Recipe y) { return sortAmmo(x.createItem, y.createItem); });

		(Recipe[] armor, recipes) = Split(recipes, delegate (Recipe r) { return isArmor(r.createItem); });
		Array.Sort(armor, delegate (Recipe x, Recipe y) { return sortArmor(x.createItem, y.createItem); });

		(Recipe[] accessories, recipes) = Split(recipes, delegate (Recipe r) { return isAccessory(r.createItem); });
		Array.Sort(accessories, delegate (Recipe x, Recipe y) { return sortAccessories(x.createItem, y.createItem); });

		(Recipe[] tools, recipes) = Split(recipes, delegate (Recipe r) { return isTool(r.createItem); });
		Array.Sort(tools, delegate (Recipe x, Recipe y) { return sortTools(x.createItem, y.createItem); });

		(Recipe[] bait, recipes) = Split(recipes, delegate (Recipe r) { return isBait(r.createItem); });
		Array.Sort(bait, delegate (Recipe x, Recipe y) { return sortBait(x.createItem, y.createItem); });

		(Recipe[] paint, recipes) = Split(recipes, delegate (Recipe r) { return isPaint(r.createItem); });
		Array.Sort(paint, delegate (Recipe x, Recipe y) { return sortPaint(x.createItem, y.createItem); });

		(Recipe[] vanity, recipes) = Split(recipes, delegate (Recipe r) { return isVanity(r.createItem); });
		Array.Sort(vanity, delegate (Recipe x, Recipe y) { return sortVanity(x.createItem, y.createItem); });

		(Recipe[] potion, recipes) = Split(recipes, delegate (Recipe r) { return isPotion(r.createItem); });
		Array.Sort(potion, delegate (Recipe x, Recipe y) { return sortPotion(x.createItem, y.createItem); });

		(Recipe[] food, recipes) = Split(recipes, delegate (Recipe r) { return isFood(r.createItem); });
		Array.Sort(food, delegate (Recipe x, Recipe y) { return sortFood(x.createItem, y.createItem); });

		(Recipe[] ingot, recipes) = Split(recipes, delegate (Recipe r) { return isIngot(r.createItem); });
		Array.Sort(ingot, delegate (Recipe x, Recipe y) { return sortIngot(x.createItem, y.createItem); });

		return torches
			.Concat(weapon)
			.Concat(armor)
			.Concat(accessories)
			.Concat(tools)
			.Concat(ammo)
			.Concat(bait)
			.Concat(paint)
			.Concat(vanity)
			.Concat(potion)
			.Concat(food)
			.Concat(ingot)
			.Concat(recipes)
			.Concat(currency)
			.Concat(none) // Must be last
			.ToArray();
	}

	public static Recipe[] AlternateSort(Recipe[] recipes)
	{
		return recipes.OrderBy(r => r.createItem.IsAir ? 1 : -1) // Having null items above real items causes the list to end abruptly.
			// Bottom Forced
			.ThenBy(r => r.createItem.IsCurrency || r.createItem.IsACoin ? 1 : 0)

			// Top Forced
			.ThenBy(r => r.createItem.createTile == TileID.Torches ? -1 : 0)
			.ThenBy(r => r.createItem.ammo > 0 && !r.createItem.notAmmo ? -1 : 0)
			.ThenBy(r => r.createItem.bait > 0 ? -1 : 0)

			// Category Sort
			.ThenBy(r => r.createItem.createTile > 0 && r.createItem.tileWand <= 0 ? 0 : 1)
			.ThenBy(r => r.createItem.createWall > 0 && r.createItem.tileWand <= 0 ? 0 : 1)
			.ThenBy(r => r.createItem.tileWand > 0 ? 0 : 1)
			.ThenBy(r => new List<int> {
					ItemID.EmptyBucket,
					ItemID.WaterBucket,
					ItemID.HoneyBucket,
					ItemID.LavaBucket,
					ItemID.BottomlessBucket,
					ItemID.BottomlessLavaBucket,
					ItemID.SuperAbsorbantSponge,
					ItemID.LavaAbsorbantSponge
				}.Contains(r.createItem.type) ? 0 : 1) // Force these here

			.ThenBy(r => r.createItem.consumable && r.createItem.pick <= 0 && r.createItem.axe <= 0 && r.createItem.hammer <= 0 && r.createItem.fishingPole <= 0 && r.createItem.damage <= 0 && r.createItem.defense <= 0 ? 0 : 1)
			.ThenBy(r => r.createItem.shoot > 0 && r.createItem.pick <= 0 && r.createItem.axe <= 0 && r.createItem.hammer <= 0 && r.createItem.fishingPole <= 0 && r.createItem.damage <= 0 && r.createItem.defense <= 0 ? 0 : 1)

			.ThenBy(r => r.createItem.dye > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.hairDye > 0 ? 0 : 1)

			.ThenBy(r => r.createItem.headSlot > 0 && r.createItem.vanity ? 0 : 1)
			.ThenBy(r => r.createItem.bodySlot > 0 && r.createItem.vanity ? 0 : 1)
			.ThenBy(r => r.createItem.legSlot > 0 && r.createItem.vanity ? 0 : 1)
			.ThenBy(r => r.createItem.vanity && r.createItem.accessory ? 0 : 1)
			.ThenBy(r => r.createItem.vanity ? 0 : 1)

			.ThenBy(r => r.createItem.accessory ? 0 : 1)

			.ThenBy(r => r.createItem.pick > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.axe > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.hammer > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.fishingPole > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.damage > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.headSlot > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.bodySlot > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.legSlot > 0 ? 0 : 1)
			.ThenBy(r => r.createItem.defense > 0 ? 0 : 1)
			// I want unsorted items to go before the above list. But using 1 : 0 on them messes up the order.

			// Equipment Stats Sort
			.ThenBy(r => r.createItem.DamageType.Type)
			.ThenBy(r => r.createItem.pick)
			.ThenBy(r => r.createItem.axe)
			.ThenBy(r => r.createItem.hammer)
			.ThenBy(r => r.createItem.fishingPole)
			.ThenBy(r => r.createItem.useAmmo)
			.ThenBy(r => -r.createItem.useStyle)
			.ThenBy(r => r.createItem.pick > 0 || r.createItem.axe > 0 || r.createItem.hammer > 0 || r.createItem.fishingPole > 0 || r.createItem.damage > 0 || r.createItem.defense > 0 ? r.createItem.rare : 0)
			.ThenBy(r => r.createItem.damage)
			.ThenBy(r => r.createItem.defense)

			// Name and Value Sort
			.ThenBy(r => r.createItem.Name.Split(' ').Last())
			.ThenBy(r => r.createItem.rare)
			.ThenBy(r => r.createItem.value)
			.ThenBy(r => r.createItem.Name)
			.ToArray();
	}
}

