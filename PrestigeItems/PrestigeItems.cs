using BepInEx;
using PrestigeItems.Items;
using R2API;
using RoR2;
using UnityEngine;
using PrestigeItems.Util;

[assembly: HG.Reflection.SearchableAttribute.OptIn] // THIS IS NEEDED FOR BaseItemBodyBehavior TO WORK!!!

namespace PrestigeItems
{
    
    // You don't need this if you're not using R2API in your plugin,
    // it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class PrestigeItems : BaseUnityPlugin
    {
        // The Plugin GUID should be a unique ID for this plugin,
        // which is human readable (as it is used in places like the config).
        // If we see this PluginGUID as it is on thunderstore,
        // we will deprecate this mod.
        // Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "BlueB";
        public const string PluginName = "PrestigeItemsMod";
        public const string PluginVersion = "0.0.1";

        public static PluginInfo SavedInfo { get; private set;}

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            SavedInfo = Info;

            Log.Init(Logger);
            AssetUtil.Init();

            Log.Debug($"Asset Bundle loaded from stream. (allegedly)");

            // Initialize item classes
            DevCube.Init();
            //Boilerplate.Init(); // Disabled for now
            PrestigeBleed.Init();
            PrestigeFungus.Init();
            PrestigeBauble.Init();
            PrestigeSymbiote.Init();
        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {
            /*
            // These are debug controls. I'm disabling them during normal gameplay, but keeping so I can test.
            debugSpawnItem(Boilerplate.myItemDef, KeyCode.F1);
            debugSpawnItem(DevCube.itemDef, KeyCode.F2);
            debugSpawnItem(PrestigeBleed.itemDef, KeyCode.F3);
            debugSpawnItem(PrestigeFungus.itemDef, KeyCode.F4);
            debugSpawnItem(PrestigeBauble.itemDef, KeyCode.F5);
            debugSpawnItem(PrestigeSymbiote.itemDef, KeyCode.F6);
            */
        }

        private void debugSpawnItem(ItemDef itemDef, KeyCode keyTrigger)
        {
            if (Input.GetKeyDown(keyTrigger))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.
                Log.Info($"Player pressed {keyTrigger}. Spawning our custom item {itemDef.name} at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(itemDef.itemIndex), transform.position, transform.forward * 20f);
            }
        }
    }
}
