using IL.RoR2.Stats;
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
        private static int chestPrice = 25; // Default fallback price
        private static float basePercentGained = 0.04f; // 4%
        private static float percentGainedPerStack = 0.02f; // +2% per stack

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

            itemDef.pickupIconSprite = AssetUtil.LoadSprite("PrestigeFungus_Alt.png");
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeFungus.prefab");

            itemDef.canRemove = true;
            itemDef.hidden = false;

            itemDef.tags = new ItemTag[]
            {
                ItemTag.AIBlacklist, ItemTag.Utility, ItemTag.SprintRelated
            };
        }

        // The game logic for the item's functionality goes in this method
        public static void Hooks()
        {
            Stage.onStageStartGlobal += (stage) =>
            {
                var priceChecks = SceneDirector.FindObjectsOfType<PurchaseInteraction>();
                foreach (var chest in priceChecks)
                {
                    // Get the price of a small chest when you load into a new stage
                    if (chest.name.Contains("Chest1") && chest.costType == CostTypeIndex.Money)
                    {
                        chestPrice = chest.cost; 
                        Log.Debug($"Found chest price at {chestPrice}");
                        break;
                    }
                }
                // If no small chests spawn on the map, retains the price from the previous stage. Defaults to 25.
            };

        }

        // String definitions / key lookup
        private static void AddTokens()
        {
            LanguageAPI.Add(itemId + "", "Charting Fungus");
            LanguageAPI.Add(itemId + "_NAME", "Charting Fungus");
            LanguageAPI.Add(itemId + "_PICKUP", "Gain gold while sprinting.");
            LanguageAPI.Add(itemId + "_DESCRIPTION", $"<style=cIsUtility>Gain {basePercentGained * 100}%</style> <style=cStack>(+{percentGainedPerStack * 100}% per stack)</style> of the price of a <style=cHumanObjective>small chest in gold</style> every second <style=cIsUtility>while sprinting.</style>");

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
                //Log.Debug($"onEnable entered for PrestigeFungus");
            }

            // FixedUpdate runs on game tick update
            private void FixedUpdate()
            {
                if (base.body.isSprinting)
                {
                    sprintTimer -= Time.fixedDeltaTime; // Subtract updateFrames / ticks. Ref: https://docs.unity3d.com/ScriptReference/Time.html
                    if(sprintTimer <= 0f && base.body.moveSpeed > 0f)
                    {
                        //Log.Debug($"PrestigeFungus: The sprintTimer has reached 0.");
                        double scaleFactor = basePercentGained + percentGainedPerStack * (base.body.inventory.GetItemCount(itemDef.itemIndex) - 1);
                        
                        body.master.GiveMoney((uint)(int)Math.Ceiling(scaleFactor * chestPrice)); // Ceiling to ensure we always give at least 1 gold.
                        sprintTimer = 1f; // Reset to 1 second timer.
                    }
                }
            }

        }

    }
}
