using MonoMod.RuntimeDetour.HookGen;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace DAMod.Components;

class PreventMovingFavouriteItems : ModSystem
{
    static readonly MethodInfo ItemSorting_FillAmmoFromInventory_Method = typeof(ItemSorting).GetMethod("FillAmmoFromInventory", BindingFlags.Public | BindingFlags.Static);
    delegate void ItemSorting_FillAmmoFromInventory_Delegate();
    static readonly MethodInfo ItemSorting_RefillItemStack_Method = typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.NonPublic | BindingFlags.Static);
    delegate void RefillItemStack_Delegate(Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex);
    static readonly MethodInfo ItemSorting_TrySlidingUp_Method = typeof(ItemSorting).GetMethod("TrySlidingUp", BindingFlags.NonPublic | BindingFlags.Static);
    delegate void ItemSorting_TrySlidingUp_Delegate(Item[] inv, int slot, int minimumIndex);

    public override void Load()
	{
		HookEndpointManager.Add(ItemSorting_FillAmmoFromInventory_Method, Override_ItemSorting_FillAmmoFromInventory);
        HookEndpointManager.Add(ItemSorting_RefillItemStack_Method, Override_ItemSorting_RefillItemStack);
        HookEndpointManager.Add(ItemSorting_TrySlidingUp_Method, Override_ItemSorting_TrySlidingUp);
    }

	public override void Unload()
	{
		HookEndpointManager.Remove(ItemSorting_FillAmmoFromInventory_Method, Override_ItemSorting_FillAmmoFromInventory);
        HookEndpointManager.Remove(ItemSorting_RefillItemStack_Method, Override_ItemSorting_RefillItemStack);
        HookEndpointManager.Remove(ItemSorting_TrySlidingUp_Method, Override_ItemSorting_TrySlidingUp);
    }

	// Don't refill from favourited ammo
	static void Override_ItemSorting_FillAmmoFromInventory(ItemSorting_FillAmmoFromInventory_Delegate ItemSorting_FillAmmoFromInventory)
	{
		List<int> list = new();
		List<int> list2 = new();
		Item[] inventory = Main.LocalPlayer.inventory;
		for (int i = 54; i < 58; i++)
		{
			ItemSlot.SetGlow(i, 0.31f, chest: false);
			Item item = inventory[i];
			if (item.IsAir)
			{
				list2.Add(i);
			}
			else if (item.ammo != AmmoID.None)
			{
				if (!list.Contains(item.type))
				{
					list.Add(item.type);
				}
				typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { inventory, inventory[i], 0, 50 });
			}
		}
		if (list2.Count < 1)
		{
			return;
		}
		for (int j = 0; j < 50; j++)
		{
			Item item2 = inventory[j];
			if (item2.stack >= 1 && item2.CanFillEmptyAmmoSlot() && list.Contains(item2.type) && !item2.favorited)
			{ // <- Changed
				int num = list2[0];
				list2.Remove(num);
				Utils.Swap(ref inventory[j], ref inventory[num]);
				typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { inventory, inventory[num], 0, 50 });
				if (list2.Count == 0)
				{
					break;
				}
			}
		}
		if (list2.Count < 1)
		{
			return;
		}
		for (int k = 0; k < 50; k++)
		{
			Item item3 = inventory[k];
			if (item3.stack >= 1 && item3.CanFillEmptyAmmoSlot() && item3.FitsAmmoSlot() && !item3.favorited)
			{ // <- Changed
				int num2 = list2[0];
				list2.Remove(num2);
				Utils.Swap(ref inventory[k], ref inventory[num2]);
				typeof(ItemSorting).GetMethod("RefillItemStack", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { inventory, inventory[num2], 0, 50 });
				if (list2.Count == 0)
				{
					break;
				}
			}
		}
	}

	// Don't refill from favourited items
	static void Override_ItemSorting_RefillItemStack(RefillItemStack_Delegate RefillItemStack, Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex)
	{
		int num = itemToRefill.maxStack - itemToRefill.stack;
		if (num <= 0)
		{
			return;
		}
		for (int i = loopStartIndex; i < loopEndIndex; i++)
		{
			Item item = inv[i];
			if (item.stack >= 1 && item.type == itemToRefill.type && !item.favorited)
			{
				int num2 = item.stack;
				if (num2 > num)
				{
					num2 = num;
				}
				num -= num2;
				itemToRefill.stack += num2;
				item.stack -= num2;
				if (item.stack <= 0)
				{
					item.TurnToAir();
				}
				if (num <= 0)
				{
					break;
				}
			}
		}
	}

	// Don't slide up favourited ammo
	static void Override_ItemSorting_TrySlidingUp(ItemSorting_TrySlidingUp_Delegate TrySlidingUp, Item[] inv, int slot, int minimumIndex)
	{
		for (int num = slot; num > minimumIndex; num--)
		{
			if (inv[num - 1].IsAir && !inv[num].favorited)
			{
				Utils.Swap(ref inv[num], ref inv[num - 1]);
			}
		}
	}
}
