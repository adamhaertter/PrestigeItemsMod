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
        private static List<BuffDef> progressionList = new List<BuffDef>() {
        RoR2Content.Buffs.Slow50, RoR2Content.Buffs.ClayGoo, RoR2Content.Buffs.Slow60, RoR2Content.Buffs.Slow80, RoR2Content.Buffs.Nullified, RoR2Content.Buffs.LunarSecondaryRoot};

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

            // TODO Load your assets
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
            // TODO Write item functionality. Start with the trigger, then write the rest. See other item implementations for examples.
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
