using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace PrestigeItems.Items
{
    internal class PrestigeSymbiote
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGESYMBIOTE";

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

            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Lunar; //TODO Might change later
            });

            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeSymbiote_Alt.png"); 
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeSymbiote.prefab");

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
            On.RoR2.HealthComponent.GetHealthBarValues += (orig, self) =>
            {
                HealthComponent.HealthBarValues healthStats = orig(self);
                // TODO Implementation
                return healthStats;
            };

            // TODO Import RecalculateStatsAPI as dependency in main class for calculations
        }



        // String definitions / key lookup
        private static void AddTokens()
        {
            LanguageAPI.Add(itemId + "", "Parasitic Symbiosis");
            LanguageAPI.Add(itemId + "_NAME", "Parasitic Symbiosis");

            // TODO Update these as the implementation changes / add styling
            LanguageAPI.Add(itemId + "_PICKUP", "Half your health is shields, but all your stats are buffed while your shields are up.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", "Convert half your health to regenerating shields, gaining 25% (+20% per stack) maximum health. ALL stats are buffed by 20% (+20% per stack) while you have a shield.");

            string lore = "Lore Text"; //TODO Write your lore text here to be shown in the logbook.
            LanguageAPI.Add(itemId + "_LORE", lore);
        }
    
    }
}
