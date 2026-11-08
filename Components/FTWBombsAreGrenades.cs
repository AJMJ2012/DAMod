using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components;
public class FTWBombsAreGrenades : ModSystem
{
	static readonly MethodInfo Projectile_ExplodeTiles_Method = typeof(Projectile).GetMethod("ExplodeTiles", BindingFlags.Public | BindingFlags.Instance);
	delegate void Projectile_ExplodeTiles_Delegate(Projectile self, Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ, bool wallSplode);

	static readonly MethodInfo Projectile_NewProjectile_Method = typeof(Projectile).GetMethod("NewProjectile", BindingFlags.Public | BindingFlags.Static, [typeof(IEntitySource), typeof(float), typeof(float), typeof(float), typeof(float), typeof(int), typeof(int), typeof(float), typeof(int), typeof(float), typeof(float), typeof(float)]);
	delegate int Projectile_NewProjectile_Delegate(IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f);
	public override void Load()
	{
		MonoModHooks.Add(Projectile_ExplodeTiles_Method, Override_Projectile_ExplodeTiles);
		MonoModHooks.Add(Projectile_NewProjectile_Method, Override_Projectile_NewProjectile);
	}
	static void Override_Projectile_ExplodeTiles(Projectile_ExplodeTiles_Delegate ExplodeTiles, Projectile self, Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ, bool wallSplode)
	{
		ExplodeTiles(self, compareSpot, radius, minI, maxI, minJ, maxJ, false);
		return;
	}
	static int Override_Projectile_NewProjectile(Projectile_NewProjectile_Delegate NewProjectile, IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
	{
		if (Type == ProjectileID.Bomb && (spawnSource is EntitySource_TileBreak || spawnSource is EntitySource_ShakeTree))
		{
			Type = ProjectileID.HappyBomb; // ModContent.ProjectileType<FTWUnsafeGrenade>();
		}
		return NewProjectile(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
	}
}

// I would base it on Happy Bomb instead of grenades but they explode on contact
// TODO: Fix invisible and damage not syncing in multiplayer
public class FTWUnsafeGrenade : ModProjectile
{
	public override void SetDefaults()
	{
		ProjectileID.Sets.Explosive[Projectile.type] = true;
		Projectile.CloneDefaults(ProjectileID.Grenade);
	}

	// Set some fixed stats on spawn
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.type = ProjectileID.Grenade; // Basically make the game treat it as a grenade
		Projectile.damage = 0;
		Projectile.knockBack = 0f;
		Projectile.timeLeft = 180;
		Projectile.friendly = true;
		Projectile.hostile = true;
	}

	// Add flames to make them more visible
	public override void AI()
	{
		int num21 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width / 2, Projectile.height / 2, DustID.Smoke, 0f, 0f, 100);
		Main.dust[num21].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
		Main.dust[num21].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
		Main.dust[num21].noGravity = true;
		Main.dust[num21].position = Projectile.Center + new Vector2(0f, -Projectile.height / 4).RotatedBy(Projectile.rotation) * 1.1f;
		Dust dust8 = Dust.NewDustDirect(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.Torch, 0f, 0f, 100);
		dust8.scale = 1f + (float)Main.rand.Next(5) * 0.1f;
		dust8.noGravity = true;
		dust8.position = Projectile.Center + new Vector2(0f, -Projectile.height / 4 - 6).RotatedBy(Projectile.rotation) * 1.1f;
	}

	// Use regular bomb stats for explosion
	public override void PrepareBombToBlow()
	{
		Projectile.Resize(128, 128);
		Projectile.damage = 100;
		Projectile.knockBack = 8f;
	}
}