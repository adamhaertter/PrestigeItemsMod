using Newtonsoft.Json.Utilities;
using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PrestigeItems.Items
{
    internal class PrestigeBauble
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGEBAUBLE";
        private static int basePercent = 10;
        private static int stackPercent = 10;
        private static List<BuffDef> progressionList;

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

            itemId = itemId.Replace(" ", "").ToUpper(); // Validate that itemId is in all caps, no spaces.

            itemDef.name = itemId;
            itemDef.nameToken = itemId + "_NAME";
            itemDef.pickupToken = itemId + "_PICKUP";
            itemDef.descriptionToken = itemId + "_DESCRIPTION";
            itemDef.loreToken = itemId + "_LORE";

            // Legendary (Red)
            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Tier3;
            });

            // Load assets
            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeBauble_Alt");
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeBauble");

            itemDef.canRemove = true;
            itemDef.hidden = false;

            itemDef.tags = new ItemTag[]
            {
                ItemTag.Utility
            };
        }

        // The game logic for the item's functionality goes in this method
        public static void Hooks()
        {
            Run.onRunStartGlobal += (orig) =>
            {
                // Define the progressionList since the buffs are not available at initialization apparently.
                // TODO Can probably be reworked so it doesn't need to load every run but this is a quick and dirty solution
                progressionList = new List<BuffDef>() 
                {
                    RoR2Content.Buffs.Slow50, 
                    RoR2Content.Buffs.ClayGoo, 
                    RoR2Content.Buffs.Slow60, 
                    RoR2Content.Buffs.Slow80, 
                    RoR2Content.Buffs.Nullified, 
                    RoR2Content.Buffs.LunarSecondaryRoot
                };
                Log.Debug($"PrestigeBauble's Progession List has been initialized.");
            };

            // On hitting an enemy
            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, damageInfo, victim) =>
            {
                orig(self, damageInfo, victim);
                
                CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                CharacterBody victimBody = victim.GetComponent<CharacterBody>();

                // Is there a victim
                if (attackerBody != null && victimBody != null)
                {
                    // Get item count to see if item in inventory, and for chance calculation
                    var itemCount = attackerBody.inventory.GetItemCount(itemDef.itemIndex);

                    // Calculate roll        
                    if (itemCount > 0 && RoR2.Util.CheckRoll((10f + (5f * (itemCount - 1))) * damageInfo.procCoefficient, attackerBody.master))
                    {
                        var rollPercentage = ((basePercent + (stackPercent * (itemCount - 1))) * damageInfo.procCoefficient);
                        //Log.Debug($"PrestigeBauble hits");

                        //Log.Debug($"Victim has {victimBody.GetBuffCount(progressionList[0].buffIndex)} stacks of Red Slow, {victimBody.GetBuffCount(progressionList[5].buffIndex)} stacks of Lunar Root");
                        
                        // If we are at max, apply more lunar root
                        if (victimBody.HasBuff(RoR2Content.Buffs.LunarSecondaryRoot)) 
                        {
                            var timedBuffs = victimBody.timedBuffs;
                            foreach (CharacterBody.TimedBuff buff in timedBuffs)
                            {
                                if (buff.buffIndex == RoR2Content.Buffs.LunarSecondaryRoot.buffIndex)
                                {
                                    Log.Debug($"Time on LunarRoot before new stack: {buff.timer}");
                                }
                            }

                            //Log.Debug($"PrestigeBauble applying another stack of Lunar Root to the victim.");
                            victimBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 3f, 20);
                            
                            // Gather info about how much lunar root is had.
                            timedBuffs = victimBody.timedBuffs;
                            foreach(CharacterBody.TimedBuff buff in timedBuffs)
                            {
                                if(buff.buffIndex == RoR2Content.Buffs.LunarSecondaryRoot.buffIndex)
                                {
                                    Log.Debug($"Time on LunarRoot after new stack: {buff.timer}");
                                }
                            }

                            Log.Debug($"");
                            return;
                        }

                        for (int i = progressionList.Count-2; i >= 0; i--)
                        {
                            // If we are in the middle, upgrade the slow
                            if (victimBody.HasBuff(progressionList[i]))
                            {
                                Log.Debug($"PrestigeBauble upgrading {progressionList[i]} into {progressionList[i+1]}");
                                victimBody.ClearTimedBuffs(progressionList[i].buffIndex);
                                //victimBody.RemoveBuff(progressionList[i].buffIndex);
                                if(i == progressionList.Count-2)
                                {
                                    // Permit stacking for Lunar Root                                    
                                    victimBody.AddTimedBuff(progressionList[i + 1], 3f, 20);
                                } else
                                {
                                    victimBody.AddTimedBuff(progressionList[i + 1], 5f);
                                }
                                return;
                            }
                        }

                        // If they have no slows, give them baby slow
                        victimBody.AddTimedBuff(progressionList[0], 5f);
                        Log.Debug($"PrestigeBauble found no slows, applying {progressionList[0]}");
                    }

                }
            };
        }



        // String definitions / key lookup
        private static void AddTokens()
        {
            // TODO Add the text as it appears in game.
            LanguageAPI.Add(itemId + "", "Promobauble");
            LanguageAPI.Add(itemId + "_NAME", "Promobauble");
            LanguageAPI.Add(itemId + "_PICKUP", "Gain the ability to inflict and promote slowness debuffs on enemies until they are rooted.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", $"{basePercent}% (+{stackPercent}% per stack) chance to promote the slowing debuff on the enemy to the next tier of slowness. Inflicts a low-level slow to enemies who have no slowness debuffs.");

            string lore = "Lore Text"; //TODO Write your lore text here to be shown in the logbook.
            LanguageAPI.Add(itemId + "_LORE", lore);
        }

    }
}
