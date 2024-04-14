using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Gamepad;

namespace DAMod.Components;

class RightAlignInfoIcons : ModSystem
{
    static readonly MethodInfo Main_GetInfoAccIconPosition_Method = typeof(Main).GetMethod("GetInfoAccIconPosition", BindingFlags.NonPublic | BindingFlags.Static);
    delegate void Main_GetInfoAccIconPosition_Delegate(int drawnCount, int StartX, out int X, out int Y);
    static readonly MethodInfo Main_DrawInfoAccs_Method = typeof(Main).GetMethod("DrawInfoAccs", BindingFlags.NonPublic | BindingFlags.Instance);
    delegate void Main_DrawInfoAccs_Delegate(Main self);

    public override void Load()
	{
		HookEndpointManager.Modify<Main_GetInfoAccIconPosition_Delegate>(Main_GetInfoAccIconPosition_Method, IL_Main_GetInfoAccIconPosition);
		HookEndpointManager.Modify<Main_DrawInfoAccs_Delegate>(Main_DrawInfoAccs_Method, IL_Main_DrawInfoAccs);
	}

	public override void Unload()
	{
		HookEndpointManager.Unmodify<Main_GetInfoAccIconPosition_Delegate>(Main_GetInfoAccIconPosition_Method, IL_Main_GetInfoAccIconPosition);
		HookEndpointManager.Unmodify<Main_DrawInfoAccs_Delegate>(Main_DrawInfoAccs_Method, IL_Main_DrawInfoAccs);
	}

	static void IL_Main_GetInfoAccIconPosition(ILContext il)
	{
		ILCursor c = new(il);
		/*
			IL_0007: ldarg.2
			IL_0008: ldsfld int32 Terraria.Main::screenWidth
			IL_000d: ldc.i4 280
			IL_0012: sub
			IL_0013: stind.i4
		*/
		c.GotoNext(
			i => i.MatchLdarg(2),
			i => i.MatchLdsfld(typeof(Main), nameof(Main.screenWidth)),
			i => i.MatchLdcI4(280),
			i => i.MatchSub(),
			i => i.MatchStindI4()
		);
		// Replace:
		//     X = screenWidth - 280;
		// With:
		//     X = screenWidth - 68;
		c.GotoNext(
			i => i.MatchLdcI4(280)
		);
		c.Remove();
		c.Emit(OpCodes.Ldc_I4, 68);
		/*
			// X = screenWidth - 280 + 20 * (drawnCount % 12) - 10;
			// sequence point: (line 36481, col 5) to (line 36481, col 57) in D:\a\tModLoader\tModLoader\src\tModLoader\Terraria\Main.cs
			IL_0043: ldarg.2
			IL_0044: ldsfld int32 Terraria.Main::screenWidth
			IL_0049: ldc.i4 280
			IL_004e: sub
			IL_004f: ldc.i4.s 20
			IL_0051: ldarg.0
			IL_0052: ldc.i4.s 12
			IL_0054: rem
			IL_0055: mul
			IL_0056: add
			IL_0057: ldc.i4.s 10
			IL_0059: sub
			IL_005a: stind.i4
		*/
		c.GotoNext(
			i => i.MatchLdarg(2),
			i => i.MatchLdsfld(typeof(Main), nameof(Main.screenWidth)),
			i => i.MatchLdcI4(280),
			i => i.MatchSub(),
			i => i.MatchLdcI4(20),
			i => i.MatchLdarg(0),
			i => i.MatchLdcI4(12),
			i => i.MatchRem(),
			i => i.MatchMul(),
			i => i.MatchAdd(),
			i => i.MatchLdcI4(10),
			i => i.MatchSub(),
			i => i.MatchStindI4()
		);
		// Replace:
		//     X = screenWidth - 280 + 20 * (drawnCount % 12) - 10;
		// With:
		//     X = screenWidth - 68 - 20 * (drawnCount % 12);
		c.GotoNext(
			i => i.MatchLdcI4(280)
		);
		c.Remove();
		c.Emit(OpCodes.Ldc_I4, 68);
		c.GotoNext(
			i => i.MatchAdd()
		);
		c.Remove();
		c.Emit(OpCodes.Sub);
		c.GotoNext(
			i => i.MatchLdcI4(10),
			i => i.MatchSub()
		);
		c.RemoveRange(2);
	}

	static void IL_Main_DrawInfoAccs(ILContext il)
	{
		ILCursor c = new(il);
		/*
			// spriteBatch.DrawString(FontAssets.MouseText.Value, displayName, new Vector2(X + num27, Y + 74 + num29 * num3 + num28 + 48), color2, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
			IL_128c: ldsfld class [FNA]Microsoft.Xna.Framework.Graphics.SpriteBatch Terraria.Main::spriteBatch
			IL_1291: ldsfld class [ReLogic]ReLogic.Content.Asset`1<class [ReLogic]ReLogic.Graphics.DynamicSpriteFont> Terraria.GameContent.FontAssets::MouseText
			IL_1296: callvirt instance !0 class [ReLogic]ReLogic.Content.Asset`1<class [ReLogic]ReLogic.Graphics.DynamicSpriteFont>::get_Value()
			IL_129b: ldloc.s 9
			IL_129d: ldloc.s 12
			IL_129f: ldloc.s 62
			IL_12a1: add
			IL_12a2: conv.r4
			IL_12a3: ldloc.s 13
			IL_12a5: ldc.i4.s 74
			IL_12a7: add
			IL_12a8: ldloc.s 65
			IL_12aa: ldloc.2
			IL_12ab: mul
			IL_12ac: add
			IL_12ad: ldloc.s 63
			IL_12af: add
			IL_12b0: ldc.i4.s 48
			IL_12b2: add
			IL_12b3: conv.r4
			IL_12b4: newobj instance void [FNA]Microsoft.Xna.Framework.Vector2::.ctor(float32, float32)
			IL_12b9: ldloc.s 64
			IL_12bb: ldc.r4 0.0
			IL_12c0: ldloca.s 36
			IL_12c2: initobj [FNA]Microsoft.Xna.Framework.Vector2
			IL_12c8: ldloc.s 36
			IL_12ca: ldloc.s 14
			IL_12cc: ldc.i4.0
			IL_12cd: ldc.r4 0.0
			IL_12d2: call void [ReLogic]ReLogic.Graphics.DynamicSpriteFontExtensionMethods::DrawString(class [FNA]Microsoft.Xna.Framework.Graphics.SpriteBatch, class [ReLogic]ReLogic.Graphics.DynamicSpriteFont, string, valuetype [FNA]Microsoft.Xna.Framework.Vector2, valuetype [FNA]Microsoft.Xna.Framework.Color, float32, valuetype [FNA]Microsoft.Xna.Framework.Vector2, valuetype [FNA]Microsoft.Xna.Framework.Vector2, valuetype [FNA]Microsoft.Xna.Framework.Graphics.SpriteEffects, float32)
		*/
		c.GotoNext(
			i => i.MatchCall(typeof(DynamicSpriteFontExtensionMethods), nameof(DynamicSpriteFontExtensionMethods.DrawString))
		);
		c.Remove();
		c.Emit(OpCodes.Ldloc, 15); // Vector2 vector3 = FontAssets.MouseText.Value.MeasureString(displayName);
		c.EmitDelegate<Action<SpriteBatch, DynamicSpriteFont, string, Vector2, Color, float, Vector2, Vector2, SpriteEffects, float, Vector2>>((spriteBatch, spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth, textSize) =>
			{
				spriteBatch.DrawString(spriteFont, text, new Vector2(position.X - textSize.X - 30, position.Y), color, rotation, origin, scale, effects, layerDepth);
			}
		);
	}
}

