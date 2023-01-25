namespace DAMod.Functions {
	public static class Math {
		public static nuint Mix(nuint x, nuint y, nuint a) => (nuint)(x*(1-a)+y*a);
		public static ulong Mix(ulong x, ulong y, ulong a) => (ulong)(x*(1-a)+y*a);
		public static uint Mix(uint x, uint y, uint a) => (uint)(x*(1-a)+y*a);
		public static ushort Mix(ushort x, ushort y, ushort a) => (ushort)(x*(1-a)+y*a);
		public static float Mix(float x, float y, float a) => (float)(x*(1-a)+y*a);
		public static sbyte Mix(sbyte x, sbyte y, sbyte a) => (sbyte)(x*(1-a)+y*a);
		public static short Mix(short x, short y, short a) => (short)(x*(1-a)+y*a);
		public static long Mix(long x, long y, long a) => (long)(x*(1-a)+y*a);
		public static int Mix(int x, int y, int a) => (int)(x*(1-a)+y*a);
		public static double Mix(double x, double y, double a) => (double)(x*(1-a)+y*a);
		public static decimal Mix(decimal x, decimal y, decimal a) => (decimal)(x*(1-a)+y*a);
		public static byte Mix(byte x, byte y, byte a) => (byte)(x*(1-a)+y*a);
		public static nint Mix(nint x, nint y, nint a) => (nint)(x*(1-a)+y*a);

		public static float GetDiffMult(float x, float y) => x == 0 && y == 0 ? 1 : x / y;
		public static double GetDiffMult(double x, double y) => x == 0 && y == 0 ? 1 : x / y;
	}
}
