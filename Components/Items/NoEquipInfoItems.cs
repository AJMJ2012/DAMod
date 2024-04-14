using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components.Items;

class NoEquipInfoItems : GlobalItem
{
    public override bool AppliesToEntity(Item item,bool lateInstantiation) => new List<int>(){ItemID.CopperWatch,ItemID.TinWatch,ItemID.SilverWatch,ItemID.TungstenWatch,ItemID.GoldWatch,ItemID.PlatinumWatch,ItemID.DepthMeter,ItemID.Compass,ItemID.GPS,ItemID.FishermansGuide,ItemID.WeatherRadio,ItemID.Sextant,ItemID.FishFinder,ItemID.MetalDetector,ItemID.Stopwatch,ItemID.DPSMeter,ItemID.GoblinTech,ItemID.TallyCounter,ItemID.LifeformAnalyzer,ItemID.Radar,ItemID.REK,ItemID.PDA,ItemID.MechanicalLens}.Contains(item.type);

    public override void SetDefaults(Item item)
    {
        item.accessory = false;
        item.Prefix(-3);
    }
}

