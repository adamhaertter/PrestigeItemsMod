using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using BepInEx;

namespace PrestigeItems.Items
{
    internal class PrestigeBleed
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGEBLEED";

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

            //itemDef.tier = ItemTier.Tier2; // TODO Replace with own tier. (Common) 

            ItemTierCatalog.availability.CallWhenAvailable(() =>           
             {
                if (itemDef) itemDef.tier = ItemTier.Tier2;
             });



            // TODO Load your assets
            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeBleed_Alt.png");
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeBleed.prefab");

            itemDef.canRemove = true;
            itemDef.hidden = false;

            itemDef.tags = new ItemTag[]
            {
                ItemTag.BrotherBlacklist, ItemTag.Utility // TODO Add item tags like ItemTag.____
            };
        }

        // The game logic for the item's functionality goes in this method
        public static void Hooks()
        {
            // TODO Write item functionality. Start with the trigger, then write the rest. See other item implementations for examples.
            On.RoR2.GlobalEventManager.OnHitEnemy += prestigeBleedEffect;
        }

        private static void prestigeBleedEffect(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            // Doing the dub
            if (damageInfo.procCoefficient <= 0.0 || damageInfo.rejected || !(bool)damageInfo.attacker)
                return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();

            if (attackerBody && victimBody)
            {
                var itemCount = attackerBody.inventory.GetItemCount(itemDef.itemIndex);

                if (itemCount > 0 && RoR2.Util.CheckRoll(10f + (5f * itemCount - 1) * damageInfo.procCoefficient, attackerBody.master))
                {
                    Log.Debug($"Enemy hit with Prestige Bleed in attacker's inventory. BuffList initialized. Conditions met, calculating buff...");

                    victimBody.AddTimedBuff(RoR2Content.Buffs.Cripple, 3);
                    
                }
            }
        }

        // String definitions / key lookup
        private static void AddTokens()
        {
            // TODO Add the text as it appears in game.
            LanguageAPI.Add(itemId + "", "Refined Shard");
            LanguageAPI.Add(itemId + "_NAME", "Refined Shard");
            LanguageAPI.Add(itemId + "_PICKUP", "And his shard was refined.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", "10% chance (+5% per stack) to inflict cripple on an enemy.");

            string lore = "10 years ago I saw a woman get dragged away by five men and i did nothing to stop it"; //TODO Write your lore text here to be shown in the logbook.
            LanguageAPI.Add(itemId + "_LORE", lore);
        }

    }
}
