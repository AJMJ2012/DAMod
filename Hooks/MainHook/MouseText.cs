using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DAMod.Hooks.MainHook {
	class DrawPendingMouseText : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(DrawPendingMouseTextMethod, Override_DrawPendingMouseText);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(DrawPendingMouseTextMethod, Override_DrawPendingMouseText); } catch {}
		}

		static MethodInfo DrawPendingMouseTextMethod => typeof(Main).GetMethod("DrawPendingMouseText", BindingFlags.NonPublic | BindingFlags.Static);
		delegate void OrigDrawPendingMouseText();

		static void Override_DrawPendingMouseText(OrigDrawPendingMouseText DrawPendingMouseText) {
			var _mouseTextCache = Main.instance.GetType().GetField("_mouseTextCache", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.instance);
			bool isValid = (bool)(_mouseTextCache.GetType().GetField("isValid").GetValue(_mouseTextCache) ?? false);
			string cursorText = (string)(_mouseTextCache.GetType().GetField("cursorText").GetValue(_mouseTextCache) ?? "");
			if (isValid && Main.HoverItem.type == 0 && !String.IsNullOrWhiteSpace(cursorText)) {
				int lineAmount;
				string[] array = Utils.WordwrapString(cursorText, FontAssets.MouseText.Value, 460, 10, out lineAmount);
				lineAmount++;
				int num3 = Main.screenWidth;
				int num4 = Main.screenHeight;
				int num5 = Main.mouseX;
				int num6 = Main.mouseY;
				float num7 = 0f;
				for (int l = 0; l < lineAmount; l++) {
					float x = FontAssets.MouseText.Value.MeasureString(array[l]).X;
					if (num7 < x) {
						num7 = x;
					}
				}
				if (num7 > 460f) {
					num7 = 460f;
				}
				bool settingsEnabled_OpaqueBoxBehindTooltips = Main.SettingsEnabled_OpaqueBoxBehindTooltips;
				Vector2 vector = new Vector2(num5, num6) + new Vector2(16f);
				if (settingsEnabled_OpaqueBoxBehindTooltips) {
					vector += new Vector2(8f, 2f);
				}
				if (vector.Y > (float)(num4 - 30 * lineAmount)) {
					vector.Y = num4 - 30 * lineAmount;
				}
				if (vector.X > (float)num3 - num7) {
					vector.X = (float)num3 - num7;
				}
				if (settingsEnabled_OpaqueBoxBehindTooltips) {
					int num8 = 10;
					int num9 = 5;
					Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)vector.X - num8, (int)vector.Y - num9, (int)num7 + num8 * 2, 30 * lineAmount + num9 + num9 / 2), new Color(23, 25, 81, 255) * 0.925f * 0.85f);
				}
				_mouseTextCache.GetType().GetField("X").SetValue(_mouseTextCache, (int)vector.X - 16);
				_mouseTextCache.GetType().GetField("Y").SetValue(_mouseTextCache, (int)vector.Y - 16);
				Main.instance.GetType().GetField("_mouseTextCache", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Main.instance, _mouseTextCache);
			}
			DrawPendingMouseText();
		}
	}
}
