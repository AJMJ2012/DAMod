using Terraria;
using Terraria.ModLoader;
using System;

namespace DAMod.Components.Items;

public class RoundMaxStack : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation) => lateInstantiation && item.maxStack > 5;
    public override void SetDefaults(Item item)
    {
        item.maxStack = (int)(Math.Ceiling(item.maxStack / 5f) * 5);
    }
}

