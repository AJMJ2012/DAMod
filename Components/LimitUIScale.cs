using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DAMod.Components;

public class LimitUIScale : ModSystem
{

	static readonly MethodInfo IngameOptions_Draw_Method = typeof(IngameOptions).GetMethod("Draw", BindingFlags.Public | BindingFlags.Static);
	public override void Load()
	{
		MonoModHooks.Modify(IngameOptions_Draw_Method, IL_IngameOptions_Draw);
	}

	public static void IL_IngameOptions_Draw(ILContext il)
	{
		ILCursor c = new(il);

		/*
			// string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Main.UIScale * 100f));
			// sequence point: (line 535, col 4) to (line 535, col 141) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\IngameOptions.cs
			IL_1340: ldstr "GameUI.UIScale"
			IL_1345: ldsfld float32 Terraria.Main::temporaryGUIScaleSlider
			IL_134a: ldc.r4 100
			IL_134f: mul
			IL_1350: conv.r8
			IL_1351: call float64 [System.Runtime]System.Math::Round(float64)
			IL_1356: box [System.Runtime]System.Double
			IL_135b: call float32 Terraria.Main::get_UIScale()
			IL_1360: ldc.r4 100
			IL_1365: mul
			IL_1366: conv.r8
			IL_1367: call float64 [System.Runtime]System.Math::Round(float64)
			IL_136c: box [System.Runtime]System.Double
			IL_1371: call string Terraria.Localization.Language::GetTextValue(string, object, object)
			IL_1376: stloc.s 43
		*/
		c.GotoNext(
			MoveType.Before,
			i => i.MatchLdstr("GameUI.UIScale"),
			i => i.MatchLdsfld(typeof(Main).GetField("temporaryGUIScaleSlider")),
			i => i.MatchLdcR4(100f),
			i => i.MatchMul(),
			i => i.MatchConvR8(),
			i => i.MatchCall(typeof(Math).GetMethod("Round", [typeof(float)])),
			i => i.MatchBox(typeof(Double)),
			i => i.MatchCall(typeof(Main).GetProperty("UIScale").GetGetMethod()),
			i => i.MatchLdcR4(100f),
			i => i.MatchMul(),
			i => i.MatchConvR8(),
			i => i.MatchCall(typeof(Math).GetMethod("Round", [typeof(float)])),
			i => i.MatchBox(typeof(Double)),
			i => i.MatchCall(typeof(Language).GetMethod("GetTextValue", [typeof(string), typeof(object), typeof(object)])),
			i => i.MatchStloc(43)
		);
		// Replace:
		//     string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Main.UIScale * 100f));
		// With:
		//     string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Math.Min(Main.temporaryGUIScaleSlider * Main.ForcedMinimumZoom), Main.instance.UIScaleMax) * 100f));
		c.GotoNext(
			MoveType.Before,
			i => i.MatchCall(typeof(Main).GetProperty("UIScale").GetGetMethod())
		);
		c.Remove();
		c.EmitLdsfld(typeof(Main).GetField("temporaryGUIScaleSlider"));
		//c.EmitLdsfld(typeof(Main).GetField("ForcedMinimumZoom"));
		//c.EmitMul();
		c.EmitLdsfld(typeof(Main).GetField("instance"));
		c.EmitCallvirt(typeof(Main).GetProperty("UIScaleMax").GetGetMethod());
		c.EmitCall(typeof(Math).GetMethod("Min", [typeof(float), typeof(float)]));

		/*
			// float num15 = DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f));
			// sequence point: (line 548, col 4) to (line 548, col 114) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\IngameOptions.cs
			IL_1421: ldarg.1
			IL_1422: ldloc.s 6
			IL_1424: ldsfld float32 Terraria.Main::temporaryGUIScaleSlider
			IL_1429: ldc.r4 0.5
			IL_142e: sub
			IL_142f: ldc.r4 1.5
			IL_1434: div
			IL_1435: ldc.r4 0.0
			IL_143a: ldc.r4 1
			IL_143f: call float32[FNA]Microsoft.Xna.Framework.MathHelper::Clamp(float32, float32, float32)
			IL_1444: ldc.i4.0
			IL_1445: ldnull
			IL_1446: call float32 Terraria.IngameOptions::DrawValueBar(class [FNA] Microsoft.Xna.Framework.Graphics.SpriteBatch, float32, float32, int32, class Terraria.Utils/ColorLerpMethod)
			IL_144b: stloc.s 44
		*/
		c.GotoNext(
			MoveType.Before,
			i => i.MatchLdarg(1),
			i => i.MatchLdloc(6),
			i => i.MatchLdsfld(typeof(Main).GetField("temporaryGUIScaleSlider")),
			i => i.MatchLdcR4(0.5f),
			i => i.MatchSub(),
			i => i.MatchLdcR4(1.5f),
			i => i.MatchDiv(),
			i => i.MatchLdcR4(0f),
			i => i.MatchLdcR4(1f),
			i => i.MatchCall(typeof(MathHelper).GetMethod("Clamp")),
			i => i.MatchLdcI4(0),
			i => i.MatchLdnull(),
			i => i.MatchCall(typeof(IngameOptions).GetMethod("DrawValueBar")),
			i => i.MatchStloc(44)
		);
		// Replace:
		//     float num15 = DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f))
		// With:
		//     float num15 = DrawValueBar(sb, scale, MathHelper.Clamp(Main.temporaryGUIScaleSlider - 1f, 0f, 1f))
		c.Index += 3;
		c.RemoveRange(4);
		c.EmitLdcR4(1f);
		c.EmitSub();

		/*
			// Main.temporaryGUIScaleSlider = num15 * 1.5f + 0.5f;
			// sequence point: (line 552, col 6) to (line 552, col 57) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\IngameOptions.cs
			IL_147b: ldloc.s 44
			IL_147d: ldc.r4 1.5
			IL_1482: mul
			IL_1483: ldc.r4 0.5
			IL_1488: add
			IL_1489: stsfld float32 Terraria.Main::temporaryGUIScaleSlider
		*/
		c.GotoNext(
			MoveType.Before,
			i => i.MatchLdloc(44),
			i => i.MatchLdcR4(1.5f),
			i => i.MatchMul(),
			i => i.MatchLdcR4(0.5f),
			i => i.MatchAdd(),
			i => i.MatchStsfld(typeof(Main).GetField("temporaryGUIScaleSlider"))
		);
		// Replace:
		//     Main.temporaryGUIScaleSlider = num15 * 1.5f + 0.5f;
		// With:
		//     Main.temporaryGUIScaleSlider = num15 + 1f;
		c.Index++;
		c.RemoveRange(4);
		c.EmitLdcR4(1f);
		c.EmitAdd();

	//	/*
	//		// if (!flag7 && Main.temporaryGUIScaleSliderUpdate && Main.temporaryGUIScaleSlider != -1f)
	//		// sequence point: (line 559, col 4) to (line 559, col 92) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\IngameOptions.cs
	//		IL_14af: ldloc.s 42
	//		IL_14b1: brtrue.s IL_14d6
	//
	//		IL_14b3: ldsfld bool Terraria.Main::temporaryGUIScaleSliderUpdate
	//		IL_14b8: brfalse.s IL_14d6
	//
	//		// Main.UIScale = Main.temporaryGUIScaleSlider;
	//		IL_14ba: ldsfld float32 Terraria.Main::temporaryGUIScaleSlider
	//		IL_14bf: ldc.r4 -1
	//		IL_14c4: beq.s IL_14d6
	//
	//		// sequence point: (line 560, col 5) to (line 560, col 49) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\IngameOptions.cs
	//		IL_14c6: ldsfld float32 Terraria.Main::temporaryGUIScaleSlider
	//		IL_14cb: call void Terraria.Main::set_UIScale(float32)
	//	*/
	//	c.GotoNext(
	//		MoveType.After,
	//		i => i.MatchLdloc(42),
	//		i => i.MatchBrtrue(out var _),
	//		i => i.MatchLdsfld(typeof(Main).GetField("temporaryGUIScaleSliderUpdate")),
	//		i => i.MatchBrfalse(out var _),
	//		i => i.MatchLdsfld(typeof(Main).GetField("temporaryGUIScaleSlider")),
	//		i => i.MatchLdcR4(-1),
	//		i => i.MatchBeq(out var _),
	//		i => i.MatchLdsfld(typeof(Main).GetField("temporaryGUIScaleSlider")),
	//		i => i.MatchCall(typeof(Main).GetProperty("UIScale").GetSetMethod())
	//	);
	//	// Replace:
	//	//     Main.UIScale = Main.temporaryGUIScaleSlider;
	//	// With:
	//	//     Main.UIScale = Main.temporaryGUIScaleSlider * Main.ForcedMinimumZoom;
	//	c.Index--;
	//	c.EmitLdsfld(typeof(Main).GetField("ForcedMinimumZoom"));
	//	c.EmitMul();
	}

	public override void PostUpdateInput()
	{
		if (Main.UIScale < Main.ForcedMinimumZoom)
		{
			Main.UIScale = Main.ForcedMinimumZoom;
		}
		if (Main.temporaryGUIScaleSlider < Main.ForcedMinimumZoom)
		{
			Main.temporaryGUIScaleSlider = Main.ForcedMinimumZoom;
		}
	}
}
