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
        private static float commonScaling = 0.1f; // 10% mult chance
        private static float baseHealthIncrease = 0.25f; // 25% max health added off the bat

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
            /*
            On.RoR2.HealthComponent.GetHealthBarValues += (orig, self) =>
            {
                HealthComponent.HealthBarValues healthStats = orig(self);
                if (self.body.inventory.GetItemCount(itemDef) > 0)
                {
                    healthStats.healthFraction = 0.5f;
                    healthStats.shieldFraction = 0.5f; // Half health is shields
                    self.shield = 0.5f;
                    // This doesn't register as having shields in game. Other shield pickups make it work, but otherwise it just is visually blue and doesn't function rn
                }
                return healthStats;
            };
            */

            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                if (sender == null || sender.inventory == null) return;

                int itemCount = sender.inventory.GetItemCount(itemDef);
                if (itemCount > 0)
                {
                    // TODO 2 issues: Sprinting instantly restores health, and the shield addition is not exactly 50% like i want. But it functions lol
                    args.healthMultAdd += itemCount * commonScaling;
                    args.baseShieldAdd += sender.healthComponent.fullHealth / 2;
                    sender.maxShield = sender.maxHealth / 2;
                    sender.maxHealth = sender.maxHealth / 2;
                    //args.baseShieldAdd += args.baseHealthAdd / 2;
                    if (sender.healthComponent.shield > 0f)
                    {                        
                        args.regenMultAdd += itemCount * commonScaling;
                        args.moveSpeedMultAdd += itemCount * commonScaling;
                        args.damageMultAdd += itemCount * commonScaling;
                        args.attackSpeedMultAdd += itemCount * commonScaling;
                        args.critAdd += args.critAdd * (1 + itemCount * commonScaling);
                        args.armorAdd += args.armorAdd * (1 + itemCount * commonScaling);

                        Log.Debug($"Stats recalculated with 1 Symbiote: ItemCount {itemCount}, Health {args.healthMultAdd}, Regen {args.regenMultAdd}, Damage {args.damageMultAdd}, AttackSpeed {args.attackSpeedMultAdd}, Crit {args.critAdd}, Armor {args.armorAdd}");
                    }
                }
            };

            // TODO Manual integration with Transcendence to bypass our shield management and just use theirs. Or avoid!

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
        /*
        public class SymbioteBehavior : RoR2.Items.BaseItemBodyBehavior
        {
            [ItemDefAssociation(useOnServer = true, useOnClient = false)]
            public static ItemDef GetItemDef() => itemDef;
            private bool hasShield = false;

            private void onEnable()
            {
                RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
                {
                    if (sender == null || sender.inventory == null) return;

                    int itemCount = sender.inventory.GetItemCount(itemDef);
                    if (itemCount > 0 & sender.GetComponent<HealthComponent>().shield > 0f
                    )
                    {
                        args.healthMultAdd += itemCount * commonScaling;
                        args.regenMultAdd += itemCount * commonScaling;
                        args.moveSpeedMultAdd += itemCount * commonScaling;
                        args.damageMultAdd += itemCount * commonScaling;
                        args.attackSpeedMultAdd += itemCount * commonScaling;
                        args.critAdd += args.critAdd * (1 + itemCount * commonScaling);
                        args.armorAdd += args.armorAdd * (1 + itemCount * commonScaling);
                    }
                };
            }

            // FixedUpdate runs on game tick update
            private void FixedUpdate()
            {
                if (base.body.healthComponent.shield > 0f) {
                    hasShield = true;
                } else
                {
                    hasShield = false;
                }
            }

            public bool HasShield()
            {
                return hasShield;
            }

        }
        */
    }
}
