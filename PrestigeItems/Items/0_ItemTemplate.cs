using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PrestigeItems.Items
{
    internal class ItemTemplate
    {
        public static ItemDef itemDef;
        private static String itemId = "";

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

            // TODO Replace with own tier (Common)
            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Tier1;
            });

            // TODO Load your assets
            itemDef.pickupIconSprite = AssetUtil.LoadSprite(""); 
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("");

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
        }



        // String definitions / key lookup
        private static void AddTokens()
        {
            // TODO Add the text as it appears in game.
            LanguageAPI.Add(itemId + "", "Item Name");
            LanguageAPI.Add(itemId + "_NAME", "Item Name");
            LanguageAPI.Add(itemId + "_PICKUP", "Pickup Flavor Text / Basic Description");
            LanguageAPI.Add(itemId + "_DESCRIPTION", "A Detailed Description of the Item's Mechanics.");

            string lore = "Lore Text"; //TODO Write your lore text here to be shown in the logbook.
            LanguageAPI.Add(itemId + "_LORE", lore);
        }
    
    }
}
