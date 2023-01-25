using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI.Gamepad;

namespace DAMod.Hooks.MainHook {
	class DrawInfoAccs : ModSystem {
		public override void Load() {
			HookEndpointManager.Add(DrawInfoAccsMethod, Override_DrawInfoAccs);
		}

		public override void Unload() {
			try { HookEndpointManager.Remove(DrawInfoAccsMethod, Override_DrawInfoAccs); } catch {}
		}

		static MethodInfo DrawInfoAccsMethod => typeof(Main).GetMethod("DrawInfoAccs", BindingFlags.NonPublic | BindingFlags.Instance);
		delegate void OrigDrawInfoAccs(Main self);

		// Right align info text and icons
		// Should use IL
		static void Override_DrawInfoAccs(OrigDrawInfoAccs DrawInfoAccs, Main self) {
			try {
				if (!Main.CanShowInfoAccs) {
					return;
				}
				int num = -1;
				int num12 = -10;
				int num23 = 0;
				string text = "";
				float num25 = 215f;
				int startX = 0;
				if (GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive) {
					startX = -50;
					num25 += 50f;
				}
				Main.InfoDisplayPageHandler(startX, ref text, out var startingDisplay, out var endingDisplay);
				int c = 0;
				for (int i = startingDisplay; i < endingDisplay; i++) {
					string text2 = "";
					string text3 = "";
					InfoDisplay info = ((List<InfoDisplay>)typeof(InfoDisplayLoader).GetField("InfoDisplays", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null))[i];
					if (!InfoDisplayLoader.Active(info) || (Main.LocalPlayer.hideInfo[info.Type] && !Main.playerInventory)) {
						continue;
					}
					if (info == InfoDisplay.Watches) {
						num = 0;
						text3 = Lang.inter[95].Value;
						string textValue = Language.GetTextValue("GameUI.TimeAtMorning");
						double num26 = Main.time;
						if (!Main.dayTime) {
							num26 += 54000.0;
						}
						num26 = num26 / 86400.0 * 24.0;
						double num27 = 7.5;
						num26 = num26 - num27 - 12.0;
						if (num26 < 0.0) {
							num26 += 24.0;
						}
						if (num26 >= 12.0) {
							textValue = Language.GetTextValue("GameUI.TimePastMorning");
						}
						int num28 = (int)num26;
						double num29 = num26 - (double)num28;
						num29 = (int)(num29 * 60.0);
						string text4 = num29.ToString() ?? "";
						if (num29 < 10.0) {
							text4 = "0" + text4;
						}
						if (num28 > 12) {
							num28 -= 12;
						}
						if (num28 == 0) {
							num28 = 12;
						}
						if (Main.LocalPlayer.accWatch == 1) {
							text4 = "00";
						}
						else if (Main.LocalPlayer.accWatch == 2) {
							text4 = ((!(num29 < 30.0)) ? "30" : "00");
						}
						text2 = num28 + ":" + text4 + " " + textValue;
					}
					else if (info == InfoDisplay.WeatherRadio) {
						num = 1;
						text3 = Lang.inter[96].Value;
						text2 = (Main.IsItStorming ? Language.GetTextValue("GameUI.Storm") : (((double)Main.maxRaining > 0.6) ? Language.GetTextValue("GameUI.HeavyRain") : (((double)Main.maxRaining >= 0.2) ? Language.GetTextValue("GameUI.Rain") : ((Main.maxRaining > 0f) ? Language.GetTextValue("GameUI.LightRain") : ((Main.cloudBGActive > 0f) ? Language.GetTextValue("GameUI.Overcast") : ((Main.numClouds > 90) ? Language.GetTextValue("GameUI.MostlyCloudy") : ((Main.numClouds > 55) ? Language.GetTextValue("GameUI.Cloudy") : ((Main.numClouds <= 15) ? Language.GetTextValue("GameUI.Clear") : Language.GetTextValue("GameUI.PartlyCloudy")))))))));
						int num30 = (int)(Main.windSpeedCurrent * 50f);
						if (num30 < 0) {
							text2 += Language.GetTextValue("GameUI.EastWind", Math.Abs(num30));
						}
						else if (num30 > 0) {
							text2 += Language.GetTextValue("GameUI.WestWind", num30);
						}
					}
					else if (info == InfoDisplay.Sextant) {
						num = ((Main.bloodMoon && !Main.dayTime) ? 8 : ((!Main.eclipse || !Main.dayTime) ? 7 : 8));
						text3 = Lang.inter[102].Value;
						if (Main.moonPhase == 0) {
							text2 = Language.GetTextValue("GameUI.FullMoon");
						}
						else if (Main.moonPhase == 1) {
							text2 = Language.GetTextValue("GameUI.WaningGibbous");
						}
						else if (Main.moonPhase == 2) {
							text2 = Language.GetTextValue("GameUI.ThirdQuarter");
						}
						else if (Main.moonPhase == 3) {
							text2 = Language.GetTextValue("GameUI.WaningCrescent");
						}
						else if (Main.moonPhase == 4) {
							text2 = Language.GetTextValue("GameUI.NewMoon");
						}
						else if (Main.moonPhase == 5) {
							text2 = Language.GetTextValue("GameUI.WaxingCrescent");
						}
						else if (Main.moonPhase == 6) {
							text2 = Language.GetTextValue("GameUI.FirstQuarter");
						}
						else if (Main.moonPhase == 7) {
							text2 = Language.GetTextValue("GameUI.WaxingGibbous");
						}
					}
					else if (info == InfoDisplay.FishFinder) {
						bool flag13 = false;
						num = 2;
						text3 = Lang.inter[97].Value;
						for (int j = 0; j < 1000; j++) {
							if (Main.projectile[j].active && Main.projectile[j].owner == Main.myPlayer && Main.projectile[j].bobber) {
								flag13 = true;
								break;
							}
						}
						if (flag13) {
							text2 = Main.LocalPlayer.displayedFishingInfo;
						}
						else {
							PlayerFishingConditions fishingConditions = Main.LocalPlayer.GetFishingConditions();
							text2 = ((fishingConditions.BaitItemType != 2673) ? (Main.LocalPlayer.displayedFishingInfo = Language.GetTextValue("GameUI.FishingPower", fishingConditions.FinalFishingLevel)) : Language.GetTextValue("GameUI.FishingWarning"));
						}
					}
					else if (info == InfoDisplay.MetalDetector) {
						num = 10;
						text3 = Lang.inter[104].Value;
						if (Main.SceneMetrics.bestOre <= 0) {
							text2 = Language.GetTextValue("GameUI.NoTreasureNearby");
						}
						else {
							int baseOption = 0;
							int num2 = Main.SceneMetrics.bestOre;
							if (Main.SceneMetrics.ClosestOrePosition.HasValue) {
								Point value = Main.SceneMetrics.ClosestOrePosition.Value;
								Tile tileSafely = Framing.GetTileSafely(value);
								if (tileSafely.HasTile) {
									MapHelper.GetTileBaseOption(value.Y, tileSafely, ref baseOption);
									num2 = tileSafely.TileType;
									if (TileID.Sets.BasicChest[num2] || TileID.Sets.BasicChestFake[num2]) {
										baseOption = 0;
									}
								}
							}
							text2 = Language.GetTextValue("GameUI.OreDetected", Lang.GetMapObjectName(MapHelper.TileToLookup(num2, baseOption)));
						}
					}
					else if (info == InfoDisplay.LifeformAnalyzer) {
						num = 11;
						text3 = Lang.inter[105].Value;
						int num3 = 1300;
						int num4 = 0;
						int num5 = -1;
						if (Main.LocalPlayer.accCritterGuideCounter <= 0) {
							Main.LocalPlayer.accCritterGuideCounter = 15;
							for (int k = 0; k < 200; k++) {
								if (Main.npc[k].active && Main.npc[k].rarity > num4 && (Main.npc[k].Center - Main.LocalPlayer.Center).Length() < (float)num3) {
									num5 = k;
									num4 = Main.npc[k].rarity;
								}
							}
							Main.LocalPlayer.accCritterGuideNumber = (byte)num5;
						}
						else {
							Main.LocalPlayer.accCritterGuideCounter--;
							num5 = Main.LocalPlayer.accCritterGuideNumber;
						}
						text2 = ((num5 < 0 || num5 >= 200 || !Main.npc[num5].active || Main.npc[num5].rarity <= 0) ? Language.GetTextValue("GameUI.NoRareCreatures") : Main.npc[num5].GivenOrTypeName);
					}
					else if (info == InfoDisplay.Radar) {
						num = 5;
						text3 = Lang.inter[100].Value;
						int num6 = 2000;
						if (Main.LocalPlayer.accThirdEyeCounter == 0) {
							Main.LocalPlayer.accThirdEyeNumber = 0;
							Main.LocalPlayer.accThirdEyeCounter = 15;
							for (int l = 0; l < 200; l++) {
								if (Main.npc[l].active && !Main.npc[l].friendly && Main.npc[l].damage > 0 && Main.npc[l].lifeMax > 5 && !Main.npc[l].dontCountMe && (Main.npc[l].Center - Main.LocalPlayer.Center).Length() < (float)num6) {
									Main.LocalPlayer.accThirdEyeNumber++;
								}
							}
						}
						else {
							Main.LocalPlayer.accThirdEyeCounter--;
						}
						text2 = ((Main.LocalPlayer.accThirdEyeNumber == 0) ? Language.GetTextValue("GameUI.NoEnemiesNearby") : ((Main.LocalPlayer.accThirdEyeNumber != 1) ? Language.GetTextValue("GameUI.EnemiesNearby", Main.LocalPlayer.accThirdEyeNumber) : Language.GetTextValue("GameUI.OneEnemyNearby")));
					}
					else if (info == InfoDisplay.TallyCounter) {
						num = 6;
						text3 = Lang.inter[101].Value;
						int lastCreatureHit = Main.LocalPlayer.lastCreatureHit;
						text2 = ((lastCreatureHit > 0) ? (Lang.GetNPCNameValue(Item.BannerToNPC(lastCreatureHit)) + ": " + NPC.killCount[lastCreatureHit]) : Language.GetTextValue("GameUI.NoKillCount"));
					}
					else if (info == InfoDisplay.DPSMeter) {
						num = 12;
						text3 = Lang.inter[106].Value;
						Main.LocalPlayer.checkDPSTime();
						text2 = ((Main.LocalPlayer.getDPS() != 0) ? Language.GetTextValue("GameUI.DPS", Main.LocalPlayer.getDPS()) : Language.GetTextValue("GameUI.NoDPS"));
					}
					else if (info == InfoDisplay.Stopwatch) {
						num = 9;
						text3 = Lang.inter[103].Value;
						Vector2 vector = Main.LocalPlayer.velocity + Main.LocalPlayer.instantMovementAccumulatedThisFrame;
						if (Main.LocalPlayer.mount.Active && Main.LocalPlayer.mount.IsConsideredASlimeMount && Main.LocalPlayer.velocity.Y != 0f && !Main.LocalPlayer.SlimeDontHyperJump) {
							vector.Y += Main.LocalPlayer.velocity.Y;
						}
						int num7 = (int)(1f + vector.Length() * 6f);
						if (num7 > Main.LocalPlayer.speedSlice.Length) {
							num7 = Main.LocalPlayer.speedSlice.Length;
						}
						float num8 = 0f;
						for (int num9 = num7 - 1; num9 > 0; num9--) {
							Main.LocalPlayer.speedSlice[num9] = Main.LocalPlayer.speedSlice[num9 - 1];
						}
						Main.LocalPlayer.speedSlice[0] = vector.Length();
						for (int m = 0; m < Main.LocalPlayer.speedSlice.Length; m++) {
							if (m < num7) {
								num8 += Main.LocalPlayer.speedSlice[m];
							}
							else {
								Main.LocalPlayer.speedSlice[m] = num8 / (float)num7;
							}
						}
						num8 /= (float)num7;
						int num10 = 42240;
						int num11 = 216000;
						float num13 = num8 * (float)num11 / (float)num10;
						if (!Main.LocalPlayer.merman && !Main.LocalPlayer.ignoreWater) {
							if (Main.LocalPlayer.honeyWet) {
								num13 /= 4f;
							}
							else if (Main.LocalPlayer.wet) {
								num13 /= 2f;
							}
						}
						text2 = Language.GetTextValue("GameUI.Speed", Math.Round(num13));
					}
					else if (info == InfoDisplay.Compass) {
						num = 3;
						text3 = Lang.inter[98].Value;
						int num14 = (int)((Main.LocalPlayer.position.X + (float)(Main.LocalPlayer.width / 2)) * 2f / 16f - (float)Main.maxTilesX);
						text2 = ((num14 > 0) ? Language.GetTextValue("GameUI.CompassEast", num14) : ((num14 >= 0) ? Language.GetTextValue("GameUI.CompassCenter") : Language.GetTextValue("GameUI.CompassWest", -num14)));
					}
					else if (info == InfoDisplay.DepthMeter) {
						num = 4;
						text3 = Lang.inter[99].Value;
						int num15 = (int)((double)((Main.LocalPlayer.position.Y + (float)Main.LocalPlayer.height) * 2f / 16f) - Main.worldSurface * 2.0);
						string text5 = "";
						float num16 = Main.maxTilesX / 4200;
						num16 *= num16;
						int num17 = 1200;
						float num18 = (float)((double)(Main.LocalPlayer.Center.Y / 16f - (65f + 10f * num16)) / (Main.worldSurface / 5.0));
						text5 = ((Main.LocalPlayer.position.Y > (float)((Main.maxTilesY - 204) * 16)) ? Language.GetTextValue("GameUI.LayerUnderworld") : (((double)Main.LocalPlayer.position.Y > Main.rockLayer * 16.0 + (double)(num17 / 2) + 16.0) ? Language.GetTextValue("GameUI.LayerCaverns") : ((num15 > 0) ? Language.GetTextValue("GameUI.LayerUnderground") : ((!(num18 >= 1f)) ? Language.GetTextValue("GameUI.LayerSpace") : Language.GetTextValue("GameUI.LayerSurface")))));
						num15 = Math.Abs(num15);
						text2 = ((num15 != 0) ? Language.GetTextValue("GameUI.Depth", num15) : Language.GetTextValue("GameUI.DepthLevel")) + " " + text5;
					}
					else {
						num = info.Type;
						text2 = info.DisplayValue();
						text3 = info.DisplayName;
					}
					InfoDisplayLoader.ModifyDisplayValue(info, ref text2);
					InfoDisplayLoader.ModifyDisplayName(info, ref text3);
					if (!(text2 != "")) {
						continue;
					}
					int X;
					int Y;
					object[] parameters = new object[]{num23, startX, null, null};
					typeof(Main).GetMethod("GetInfoAccIconPosition", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, parameters);
					X = (int)parameters[2];
					Y = (int)parameters[3];
					if (num >= 0) {
						int num19 = 22;
						if (Main.screenHeight < 650) {
							num19 = 20;
						}
						int padd = 192 + 20; // Added
						Vector2 vector2 = new Vector2(X + padd, Y + 74 + num19 * num23 + 52); // Changed
						int num20 = info.Type;
						Texture2D icon = ModContent.Request<Texture2D>(info.Texture).Value;
						Color color = Color.White;
						bool flag14 = false;
						if (Main.playerInventory) {
							padd = Main.ShouldDrawInfoIconsHorizontally ? 192 - (c++ * 40) + 30 : 0; // Added
							vector2 = new Vector2(X + padd, Y); // Changed
							if ((float)Main.mouseX >= vector2.X && (float)Main.mouseY >= vector2.Y && (float)Main.mouseX <= vector2.X + (float)icon.Width && (float)Main.mouseY <= vector2.Y + (float)icon.Height && !PlayerInput.IgnoreMouseInterface) {
								flag14 = true;
								Main.LocalPlayer.mouseInterface = true;
								if (Main.mouseLeft && Main.mouseLeftRelease) {
									SoundEngine.PlaySound(SoundID.MenuTick);
									Main.mouseLeftRelease = false;
									Main.LocalPlayer.hideInfo[num20] = !Main.LocalPlayer.hideInfo[num20];
								}
								if (!Main.mouseText) {
									text = text3;
									Main.mouseText = true;
								}
							}
							if (Main.LocalPlayer.hideInfo[num20]) {
								color = new Color(80, 80, 80, 70);
							}
						}
						else if ((float)Main.mouseX >= vector2.X && (float)Main.mouseY >= vector2.Y && (float)Main.mouseX <= vector2.X + (float)icon.Width && (float)Main.mouseY <= vector2.Y + (float)icon.Height && !Main.mouseText) {
							num12 = num23;
							if (i >= 7) {
								num12++;
							}
							text = text3;
							Main.mouseText = true;
						}
						UILinkPointNavigator.SetPosition(1558 + num23 - 1, vector2 + icon.Size() * 0.75f);
						Main.spriteBatch.Draw(icon, vector2, new Rectangle(0, 0, icon.Width, icon.Height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
						if (flag14) {
							Main.spriteBatch.Draw(TextureAssets.InfoIcon[13].Value, vector2 - Vector2.One * 2f, null, Main.OurFavoriteColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
						}
						X += 20;
					}
					UILinkPointNavigator.Shortcuts.INFOACCCOUNT = num23;
					if (!Main.playerInventory) {
						Vector2 scale = new Vector2(1f);
						Vector2 vector3 = FontAssets.MouseText.Value.MeasureString(text2);
						if (vector3.X > num25) {
							scale.X = num25 / vector3.X;
						}
						if (scale.X < 0.58f) {
							scale.Y = 1f - scale.X / 3f;
						}
						for (int n = 0; n < 5; n++) {
							int num21 = 0;
							int num22 = 0;
							Color color2 = Color.Black;
							if (n == 0) {
								num21 = -2;
							}
							if (n == 1) {
								num21 = 2;
							}
							if (n == 2) {
								num22 = -2;
							}
							if (n == 3) {
								num22 = 2;
							}
							if (n == 4) {
								color2 = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
							}
							if (i > num12 && i < num12 + 2) {
								color2 = new Color((int)color2.R / 3, (int)color2.G / 3, (int)color2.B / 3, (int)color2.A / 3);
							}
							int num24 = 22;
							if (Main.screenHeight < 650) {
								num24 = 20;
							}
							int padd = (startX - (int)FontAssets.MouseText.Value.MeasureString(text2).X) + (192 - 10 - startX); // Added
							Main.spriteBatch.DrawString(FontAssets.MouseText.Value, text2, new Vector2(X + num21 + padd, Y + 74 + num24 * num23 + num22 + 48), color2, 0f, default(Vector2), scale, SpriteEffects.None, 0f); // Changed
						}
					}
					num23++;
				}
				if (!string.IsNullOrEmpty(text)) {
					if (Main.playerInventory) {
						Main.LocalPlayer.mouseInterface = true;
					}
					self.MouseText(text, 0, 0);
				}
			}
			catch {}
		}
	}
}
