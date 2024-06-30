using EntityStates.Engi.EngiWeapon;
using PrestigeItems.Util;
using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements.UIR;
using static RoR2.Items.BaseItemBodyBehavior;

namespace PrestigeItems.Items
{
    internal class PrestigeSymbiote
    {
        public static ItemDef itemDef;
        private static String itemId = "PRESTIGESYMBIOTE";
        private static readonly float commonScaling = 0.1f; // 10% mult chance
        private static readonly float baseHealthIncrease = 0.25f; // 25% max health added off the bat 

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
            itemDef.pickupModelPrefab = AssetUtil.LoadModel("PrestigeSymbioteClean.prefab");

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
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                if (sender == null || sender.inventory == null) return;

                int itemCount = sender.inventory.GetItemCount(itemDef);
                if (itemCount > 0)
                {
                    args.healthMultAdd -= 0.5f;
                    args.baseShieldAdd += sender.maxHealth;
                    // TODO Add flat health increase?
                    if (sender.healthComponent.shield > 0f)
                    {
                        args.healthMultAdd += itemCount * commonScaling; // Takes a sec / an action to kick in 
                        args.regenMultAdd += itemCount * commonScaling; // This is kinda useless tbh but oh well
                        args.moveSpeedMultAdd += itemCount * commonScaling;
                        args.damageMultAdd += itemCount * commonScaling;
                        args.attackSpeedMultAdd += itemCount * commonScaling;
                        args.critAdd += itemCount * (commonScaling * 100); 
                        args.armorAdd += itemCount * (commonScaling * 100);
                    }
                }
            };

            // This is intended to give you the proper proportion of health : shields on pickup rather than waiting for the first RecalculateStats call to happen (on action).
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, body) =>
            {
                orig(body);

                if (body.inventory.GetItemCount(itemDef) == 1)
                {
                    float halfHealth = body.maxHealth / 2;
                    body.maxHealth -= halfHealth;
                    body.maxShield += halfHealth;

                    // Adjust health in case we have too much after the initial calculation
                    if (body.healthComponent.health > body.maxHealth)
                        body.healthComponent.health = body.maxHealth;
                    if (body.healthComponent.shield > body.maxShield) 
                        body.healthComponent.shield = body.maxShield;
                }
                body.RecalculateStats();
                };

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
