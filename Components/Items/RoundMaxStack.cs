using System;
using Terraria;
using Terraria.ModLoader;

using static DALib.Functions.ItemFunctions;

namespace DAMod.Components.Items;

public class RoundMaxStack : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => lateInstantiation && item.maxStack > 5 && item.GetItemID() != "ModLoader.UnloadedItem";
    public override void SetDefaults(Item item)
    {

        item.maxStack = (int)(Math.Ceiling(item.maxStack / 5f) * 5);
    }
}

