using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DAMod.DALib {
	internal static class UI {
		static string LastLine = "";
		public static void SetLoadingMessageDisplayText(string title) {
			if (Main.netMode == 2) {
				string Line = title;
				if (Line != LastLine) {
					Console.WriteLine(Line);
					LastLine = Line;
				}
			}
			else {
				Assembly A = Assembly.GetAssembly(typeof(Mod));
				FieldInfo LoadModsProgressObject = A.GetType("Terraria.ModLoader.UI.Interface").GetField("loadMods", BindingFlags.Static | BindingFlags.NonPublic);
				FieldInfo DisplayTextProperty = A.GetType("Terraria.ModLoader.UI.UILoadMods").GetField("DisplayText", BindingFlags.Instance | BindingFlags.Public);
				DisplayTextProperty.SetValue(LoadModsProgressObject.GetValue(null), title);
			}
		}

		static string LastLinePercent = "";
		public static void SetLoadingMessageProgress(float progress) {
			if (Main.netMode == 2 && progress >= 0) {
				string LinePercent = LastLine + " " + (int)progress + "%";
				if (LinePercent != LastLinePercent) {
					Console.WriteLine(LinePercent);
					LastLinePercent = LinePercent;
				}
			}
			else {
				Assembly A = Assembly.GetAssembly(typeof(Mod));
				FieldInfo LoadModsProgressObject = A.GetType("Terraria.ModLoader.UI.Interface").GetField("loadMods", BindingFlags.Static | BindingFlags.NonPublic);
				PropertyInfo ProgressProperty = A.GetType("Terraria.ModLoader.UI.UILoadMods").GetProperty("Progress", BindingFlags.Instance | BindingFlags.Public);
				ProgressProperty.SetValue(LoadModsProgressObject.GetValue(null), progress);
			}
		}

		public static void SetLoadingMessageSubProgressText(string submessage) {
			if (Main.netMode != 2) {
				Assembly A = Assembly.GetAssembly(typeof(Mod));
				FieldInfo LoadModsProgressObject = A.GetType("Terraria.ModLoader.UI.Interface").GetField("loadMods", BindingFlags.Static | BindingFlags.NonPublic);
				PropertyInfo SubProgressTextProperty = A.GetType("Terraria.ModLoader.UI.UILoadMods").GetProperty("SubProgressText", BindingFlags.Instance | BindingFlags.Public);
				SubProgressTextProperty.SetValue(LoadModsProgressObject.GetValue(null), submessage);
			}
		}

		public static void SetLoadingMessage(string title, float progress = -1, string submessage = "") {
			if (Main.netMode == 2 && progress >= 0) {
				string Line = title + " " + (int)progress + "%";
				if (Line != LastLine) {
					Console.WriteLine(Line);
					LastLine = Line;
				}
			}
			else {
				SetLoadingMessageDisplayText(title);
				SetLoadingMessageProgress(progress);
				SetLoadingMessageSubProgressText(submessage);
			}
		}
	}
}
