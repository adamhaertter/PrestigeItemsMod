using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace PrestigeItems.Items
{
    internal class PrestigeRevive
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGEREVIVE";

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

            // Set tier to 2 (green)
            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Lunar;
            });

            // Load Assets
            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeRevive_Alt.png");
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeRevive.prefab");

            itemDef.canRemove = true;
            itemDef.hidden = false;

            itemDef.tags = new ItemTag[]
            {
                ItemTag.Damage
            };
        }

        // The game logic for the item's functionality goes in this method
        public static void Hooks()
        {
            // Congrats, you died
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, victimNetworkUser) =>
            { 
            
                orig(self, damageReport, victimNetworkUser);

                CharacterBody playableCharacter = damageReport.victimBody.GetComponent<CharacterBody>();

                if (playableCharacter != null && playableCharacter.inventory)
                {

                }

            };
        }

        // String definitions / key lookup
        private static void AddTokens()
        {
            // Language Tokens
            LanguageAPI.Add(itemId + "", "Refined Shard");
            LanguageAPI.Add(itemId + "_NAME", "Refined Shard");
            LanguageAPI.Add(itemId + "_PICKUP", "Chance to cripple enemies on hit.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", "<style=cIsDamage>10%</style> <style=cStack>(+5% per stack)</style> chance to inflict <style=cLunarObjective>cripple</style> on an enemy.");

            string lore = "10 years ago I saw a woman get dragged away by five men and i did nothing to stop it"; //TODO Write your lore text here to be shown in the logbook.
            LanguageAPI.Add(itemId + "_LORE", lore);
        }

    }
}
