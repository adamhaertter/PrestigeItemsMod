using PrestigeItems.Util;
using R2API;
using RoR2;
using RoR2.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RoR2.Items.BaseItemBodyBehavior;

namespace PrestigeItems.Items
{
    internal class PrestigeFungus
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGEFUNGUS";

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

            // Uncommon Tier (Green)
            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Tier2;
            });

            // TODO Load your assets
            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeFungus_Alt.png");
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeFungus.prefab");

            itemDef.canRemove = true;
            itemDef.hidden = false;

            itemDef.tags = new ItemTag[]
            {
                ItemTag.AIBlacklist, ItemTag.Utility, ItemTag.SprintRelated // TODO Add item tags like ItemTag.____
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
            LanguageAPI.Add(itemId + "", "Charting Fungus");
            LanguageAPI.Add(itemId + "_NAME", "Charting Fungus");
            LanguageAPI.Add(itemId + "_PICKUP", "Gain money while you sprint.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", "Gain 4% (+2% per stack) of the price of a small chest every second while sprinting.");

            string lore = "Hello :D \n\n I like money :D"; //TODO Write some real lore maybe
            LanguageAPI.Add(itemId + "_LORE", lore);
        }

        public class PrestigeFungusBehavior : BaseItemBodyBehavior
        {
            [ItemDefAssociation(useOnServer = true, useOnClient = false)]
            public static ItemDef GetItemDef() => itemDef;
            private float sprintTimer = 1f; // 1 second

            private void onEnable()
            {
                Log.Debug($"onEnable entered for PrestigeFungus");
            }

            // FixedUpdate runs on game tick update
            private void FixedUpdate()
            {
                if (base.body.isSprinting)
                {
                    sprintTimer -= Time.fixedDeltaTime; // Subtract updateFrames / ticks. Ref: https://docs.unity3d.com/ScriptReference/Time.html
                    if(sprintTimer <= 0f && base.body.moveSpeed > 0f)
                    {
                        Log.Debug($"PrestigeFungus: The sprintTimer has reached 0.");
                        // TODO Apply gold.
                        
                        body.master.GiveMoney(1);
                        sprintTimer = 1f; // Reset to 1 second timer.
                    }
                }
            }

        }

    }
}
