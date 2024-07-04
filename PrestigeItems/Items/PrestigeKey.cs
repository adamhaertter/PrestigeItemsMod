using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace PrestigeItems.Items
{
    internal class PrestigeKey
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGEKEY";

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

            // Lunar
            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Lunar;
            });

            // Load your assets
            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeKey_Alt.png"); 
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PretigeKey.prefab");

            itemDef.canRemove = true; 
            itemDef.hidden = false; 

            itemDef.tags = new ItemTag[]
            {
                ItemTag.Utility, ItemTag.ObliterationRelated, ItemTag.Cleansable, ItemTag.AIBlacklist, ItemTag.OnStageBeginEffect
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
            LanguageAPI.Add(itemId + "", "Ethereal Key");
            LanguageAPI.Add(itemId + "_NAME", "Ethereal Key");
            LanguageAPI.Add(itemId + "_PICKUP", "Gain access to an Ethereal Reserve which permits access to a random Hidden Realm. Consumed on use.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", "A hidden cache containing a random teleporter orb (25%/25%/25%/25%) will appear in a random location on each stage. Opening the cache consumes this item. Taking this to the Obelisk may yield an undesired effect.");

            string lore = "Lore Text"; //TODO Write your lore text here to be shown in the logbook.
            LanguageAPI.Add(itemId + "_LORE", lore);
        }
    
    }
}
