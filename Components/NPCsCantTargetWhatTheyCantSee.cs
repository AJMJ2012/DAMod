using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DAMod.Components;

class NPCsCantTargetWhatTheyCantSee : ModSystem
{
    static readonly MethodInfo NPC_TargetClosest_Method = typeof(NPC).GetMethod("TargetClosest", BindingFlags.Public | BindingFlags.Instance);
    delegate void NPC_TargetClosest_Delegate(NPC self, bool faceTarget);

    public override void Load()
	{
		MonoModHooks.Add(NPC_TargetClosest_Method, Override_NPC_TargetClosest);
	}

	static readonly MethodInfo NPC_TryTrackingTarget_Method = typeof(NPC).GetMethod("TryTrackingTarget", BindingFlags.NonPublic | BindingFlags.Instance);
	static readonly MethodInfo NPC_SetTargetTrackingValues_Method = typeof(NPC).GetMethod("SetTargetTrackingValues", BindingFlags.NonPublic | BindingFlags.Instance);


	static float LerpAngle(float from, float to, float weight)
	{
		double difference = (to - from) % Math.Tau;
		double short_angle_dist = ((2 * difference) % Math.Tau) - difference;
		return (float)(from + short_angle_dist * weight);
	}

	static void Override_NPC_TargetClosest(NPC_TargetClosest_Delegate TargetClosest, NPC self, bool faceTarget)
	{
		if (!Config.Server.NPCsCantSeeYouThroughWalls) {
			TargetClosest(self, faceTarget);
			return;
		}
		float distance = 0f;
		float realDist = 0f;
		bool t = false;
		int tankTarget = -1;
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].ghost)
			{
				int baseAggro2 = Main.player[i].aggro;
				if (!self.noTileCollide && !Collision.CanHit(self, Main.player[i]))
				{
					Main.player[i].aggro = -1000;
				}
				object[] args = [distance, realDist, t, tankTarget, i];
				NPC_TryTrackingTarget_Method.Invoke(self, args);
				distance = (float)args[0];
				realDist = (float)args[1];
				t = (bool)args[2];
				tankTarget = (int)args[3];
				Main.player[i].aggro = baseAggro2;
			}
		}
		bool weirdFlying = self.type == NPCID.EaterofSouls || self.type == NPCID.Corruptor || self.type == NPCID.Crimera || self.type == NPCID.BloodSquid;
		int baseAggro = Main.player[self.target].aggro;
		if (!self.noTileCollide && !Collision.CanHit(self, Main.player[self.target]))
		{
			if (weirdFlying && self.aiStyle == NPCAIStyleID.Flying)
			{
				self.aiStyle = NPCAIStyleID.Bat;
				self.ai[2] = Main.rand.Next(600) - 300;
			}
			Main.player[self.target].aggro = -1000;
		}
		else if (weirdFlying && self.aiStyle == NPCAIStyleID.Bat)
		{
			self.ai[1] = 0;
			self.ai[2] = 0;
			self.aiStyle = NPCAIStyleID.Flying;
		}
		// TODO: Move to new module "Better Corruptor AI"
		if (weirdFlying && self.aiStyle == NPCAIStyleID.Bat)
		{
			self.ai[1] = 500;
			Vector2 direction = Vector2.Zero;
			direction.X = (self.ai[2] > 0 ? 1f : -1f) * Math.Abs(self.velocity.Y);
			direction.Y = (self.ai[2] < -150f || self.ai[2] > 150f ? -1f : 1f) * Math.Abs(self.velocity.X);
			self.rotation = LerpAngle(self.rotation, direction.ToRotation(), self.velocity.Length() / 120f);
		}
		NPC_SetTargetTrackingValues_Method.Invoke(self, [faceTarget, realDist, tankTarget]);
		Main.player[self.target].aggro = baseAggro;
		return;
	}
}

