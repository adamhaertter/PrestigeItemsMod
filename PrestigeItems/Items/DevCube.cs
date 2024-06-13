using R2API;
using RoR2;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using PrestigeItems.Util;

namespace PrestigeItems.Items
{
    internal class DevCube
    {
        public static ItemDef itemDef;
        private static System.Random random = new System.Random();

        internal static void Init()
        {
            GenerateItem();
            AddTokens();

            var displayRules = new ItemDisplayRuleDict(null);
            ItemAPI.Add(new CustomItem(itemDef, displayRules));

            Hooks();
        }

        // Defining the item
        private static void GenerateItem()
        {
            itemDef = ScriptableObject.CreateInstance<ItemDef>();

            itemDef.name = "DEVCUBE";
            itemDef.nameToken = "DEVCUBE_NAME";
            itemDef.pickupToken = "DEVCUBE_PICKUP";
            itemDef.descriptionToken = "DEVCUBE_DESCRIPTION";
            itemDef.loreToken = "DEVCUBE_LORE";

            itemDef.tier = ItemTier.Tier1; // Common

            try
            {
                Log.Debug($"Loading DevCube sprite...");
                itemDef.pickupIconSprite = AssetUtil.bundle.LoadAsset<Sprite>("DevCube.png");
                Log.Debug($"DevCube sprite loaded!");
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
            }

            try { 
            Log.Debug($"Loading DevCube model...");
                itemDef.pickupModelPrefab = AssetUtil.bundle.LoadAsset<GameObject>("DevCube.prefab");
                Log.Debug($"DevCube model loaded!");
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
            }

            //itemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            //itemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
            itemDef.canRemove = true;
            itemDef.hidden = false;

            itemDef.tags = new ItemTag[]
            {
                ItemTag.BrotherBlacklist, ItemTag.Utility, ItemTag.OnKillEffect // No reasoning for me picking these tags, I'm just testing
            };
        }

        // The game logic goes in this method
        public static void Hooks()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += (orig, eventManager, damageReport) =>
            {
                orig(eventManager, damageReport);

                // If a character was killed by the world, we shouldn't do anything.
                if (!damageReport.attacker || !damageReport.attackerBody)
                {
                    return;
                }

                var attacker = damageReport.attackerBody;

                // If the attacker has this item
                if (attacker.inventory)
                {
                    var count = attacker.inventory.GetItemCount(itemDef);
                    if (count > 0)
                    {
                        List<BuffDef> buffList = new List<BuffDef>()
                        {
                            RoR2Content.Buffs.ArmorBoost, 
                            RoR2Content.Buffs.AttackSpeedOnCrit,
                            RoR2Content.Buffs.BanditSkull,
                            RoR2Content.Buffs.AffixHauntedRecipient,
                            RoR2Content.Buffs.Cloak,
                            RoR2Content.Buffs.CloakSpeed,
                            DLC1Content.Buffs.ImmuneToDebuffReady,
                            RoR2Content.Buffs.ElementalRingsReady,
                            RoR2Content.Buffs.ElephantArmorBoost,
                            RoR2Content.Buffs.Energized,
                            RoR2Content.Buffs.FullCrit,
                            RoR2Content.Buffs.Immune,
                            RoR2Content.Buffs.HiddenInvincibility,
                            DLC1Content.Buffs.KillMoveSpeed,
                            RoR2Content.Buffs.LifeSteal,
                            RoR2Content.Buffs.MedkitHeal,
                            RoR2Content.Buffs.NoCooldowns,
                            DLC1Content.Buffs.OutOfCombatArmorBuff,
                            RoR2Content.Buffs.PowerBuff,
                            RoR2Content.Buffs.CrocoRegen,
                            RoR2Content.Buffs.LaserTurbineKillCharge,
                            DLC1Content.Buffs.BearVoidReady,
                            DLC1Content.Buffs.PrimarySkillShurikenBuff,
                            DLC1Content.Buffs.ElementalRingVoidReady,
                            RoR2Content.Buffs.SmallArmorBoost,
                            RoR2Content.Buffs.TeamWarCry,
                            RoR2Content.Buffs.TeslaField,
                            RoR2Content.Buffs.TonicBuff,
                            DLC1Content.Buffs.VoidSurvivorCorruptMode,
                            RoR2Content.Buffs.WarCryBuff,
                            RoR2Content.Buffs.Warbanner,
                            DLC1Content.Buffs.MushroomVoidActive,
                            RoR2Content.Buffs.WhipBoost,
                            RoR2Content.Buffs.AffixRed,
                            RoR2Content.Buffs.AffixHaunted,
                            RoR2Content.Buffs.AffixWhite,
                            RoR2Content.Buffs.AffixPoison,
                            DLC1Content.Buffs.EliteEarth,
                            RoR2Content.Buffs.AffixBlue,
                            RoR2Content.Buffs.AffixLunar,
                            DLC1Content.Buffs.EliteVoid
                        };

                        Log.Debug($"Enemy killed with Dev Cube in attacker's inventory. BuffList initialized. Conditions met, calculating buff...");
                        // Choose a random buff or debuff
                        int randIndex = random.Next(buffList.Count);

                        //randomBuff = buffList[buffList.Count - 1];//RoR2Content.Buffs.AffixLunar;
                        /*
                        for (int i = 0; i < buffList.Count; i++)
                        {
                            if (buffList[i] == null)
                            {
                                Log.Debug($"Buff at index {i} is null.");
                            }
                            else
                            {
                                Log.Debug($"Buff at index {i} is: {buffList[i].name}");
                            }
                        }
                        */
                        
                        Log.Debug($"Applying {buffList[randIndex].name} to attacker.");

                        // Add a random buff or debuff to the attacker for 3 seconds + 1 per item owned
                        attacker.AddTimedBuff(buffList[randIndex], 3 + count);

                    }
                }
            };
        }

        

        // String definitions / key lookup
        private static void AddTokens()
        {
            LanguageAPI.Add("DEVCUBE", "Dev Cube");
            LanguageAPI.Add("DEVCUBE_NAME", "Dev Cube");
            LanguageAPI.Add("DEVCUBE_PICKUP", "Applies a random buff upon killing an enemy.");
            LanguageAPI.Add("DEVCUBE_DESCRIPTION", "<style=cHumanObjective>On kill</style>, grants the holder a <style=cLunarObjective>random buff </style> for <style=cIsUtility>4</style> <style=cStack>(+1 per stack)</style> seconds.");

            string lore = "This is my first custom item. I'll probably use it for testing purposes. Did you read this lore text?";
            LanguageAPI.Add("DEVCUBE_LORE", lore);
        }
    }
}
