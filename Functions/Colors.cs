using Microsoft.Xna.Framework;
using System;

// Bleh, can't extend internal static classes, like System.Math
namespace DAMod.Functions {
	public static class Colors {
		public static string ColorString(string input, Color color) => string.Format("[c/{0}:{1}]", RGBToHex(color), input);
		public static string ColorString(string input, Vector3 color) => string.Format("[c/{0}:{1}]", RGBToHex(color), input);

		public static string RGBToHex(Color color) => string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
		public static string RGBToHex(Vector3 color) => string.Format("{0:X2}{1:X2}{2:X2}", color.X, color.Y, color.Z);

		public static Color ShiftHue(Color rgb, int degrees) => HSLToRGB(ShiftHue(RGBToHSL(rgb), degrees));
		public static Vector3 ShiftHue(Vector3 hsl, int degrees) => hsl + new Vector3(degrees / 180f, 0f, 0f);

		public static Color MinColor(Color color) {
			byte minColor = System.Math.Min(color.R, System.Math.Min(color.G, color.B));
			return new Color(minColor, minColor, minColor);
		}
		public static Color MaxColor(Color color) {
			byte maxColor = System.Math.Max(color.R, System.Math.Max(color.G, color.B));
			return new Color(maxColor, maxColor, maxColor);
		}

		public static Color HSLToRGB(Vector3 hslVector) {
			return HSLToRGB(hslVector.X, hslVector.Y, hslVector.Z);
		}

		public static Color HSLToRGB(float Hue, float Saturation, float Luminosity, byte a = byte.MaxValue) {
			byte r;
			byte g;
			byte b;
			if (Saturation == 0f) {
				r = (byte)System.Math.Round((double)Luminosity * 255.0);
				g = (byte)System.Math.Round((double)Luminosity * 255.0);
				b = (byte)System.Math.Round((double)Luminosity * 255.0);
			}
			else {
				double num3 = Hue;
				double num2 = ((!((double)Luminosity < 0.5)) ? ((double)(Luminosity + Saturation - Luminosity * Saturation)) : ((double)Luminosity * (1.0 + (double)Saturation)));
				double t = 2.0 * (double)Luminosity - num2;
				double c = num3 + 1.0 / 3.0;
				double c2 = num3;
				double c3 = num3 - 1.0 / 3.0;
				c = HueToRGB(c, t, num2);
				c2 = HueToRGB(c2, t, num2);
				double num4 = HueToRGB(c3, t, num2);
				r = (byte)System.Math.Round(c * 255.0);
				g = (byte)System.Math.Round(c2 * 255.0);
				b = (byte)System.Math.Round(num4 * 255.0);
			}
			return new Color(r, g, b, a);
		}

		public static double HueToRGB(double c, double t1, double t2) {
			if (c < 0.0) {
				c += 1.0;
			}
			if (c > 1.0) {
				c -= 1.0;
			}
			if (6.0 * c < 1.0) {
				return t1 + (t2 - t1) * 6.0 * c;
			}
			if (2.0 * c < 1.0) {
				return t2;
			}
			if (3.0 * c < 2.0) {
				return t1 + (t2 - t1) * (2.0 / 3.0 - c) * 6.0;
			}
			return t1;
		}

		public static Vector3 RGBToHSL(Color newColor) {
			float num = (int)newColor.R;
			float num2 = (int)newColor.G;
			float num3 = (int)newColor.B;
			num /= 255f;
			num2 /= 255f;
			num3 /= 255f;
			float val = System.Math.Max(num, num2);
			val = System.Math.Max(val, num3);
			float val2 = System.Math.Min(num, num2);
			val2 = System.Math.Min(val2, num3);
			float num4 = 0f;
			float num5 = (val + val2) / 2f;
			float y;
			if (val == val2) {
				num4 = (y = 0f);
			}
			else {
				float num6 = val - val2;
				y = (((double)num5 > 0.5) ? (num6 / (2f - val - val2)) : (num6 / (val + val2)));
				if (val == num) {
					num4 = (num2 - num3) / num6 + (float)((num2 < num3) ? 6 : 0);
				}
				if (val == num2) {
					num4 = (num3 - num) / num6 + 2f;
				}
				if (val == num3) {
					num4 = (num - num2) / num6 + 4f;
				}
				num4 /= 6f;
			}
			return new Vector3(num4, y, num5);
		}
	}
}
