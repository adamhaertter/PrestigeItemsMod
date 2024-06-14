using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PrestigeItems.Util
{
    public static class AssetUtil
    {
        public static AssetBundle bundle;
        public const string bundleName = "prestigemodassets";

        public static Sprite defaultSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
        public static GameObject defaultModel = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(PrestigeItems.SavedInfo.Location), bundleName);
            }
        }

        public static void Init()
        {
            bundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }

        /// <summary>
        /// Loads the requested asset from the AssetBundle defined in AssetUtil. If the given filename is invalid, it falls back to the game's default Mystery icon.
        /// </summary>
        /// <param name="fileName">The filename of the asset as it appears in the AssetBundle. You must include the file extenstion.</param>
        /// <returns>The loaded asset, if it exists within the AssetBundle. Otherwise, returns null.</returns>
        private static T SafeLoad<T>(string fileName) where T : UnityEngine.Object
        {
            try
            {
                //Log.Debug($"Loading Template asset...");
                var loaded = AssetUtil.bundle.LoadAsset<T>(fileName);

                if (loaded == null)
                {
                    Log.Debug($"Asset {fileName} not found in the AssetBundle. Using fallback asset.");
                }
                return loaded;
                //Log.Debug($"Template asset loaded!");
            }
            catch (Exception e)
            {
                Log.Error($"Error loading asset {fileName}: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Loads the requested Sprite from the AssetBundle.
        /// </summary>
        /// <param name="filename">The filename of the asset as it appears in the AssetBundle.</param>
        /// <returns>The loaded Sprite, if it exists within the AssetBundle. Otherwise, returns the game's default Mystery icon.</returns>
        public static Sprite LoadSprite(string filename)
        {
            if (!filename.Contains("."))
            {
                filename += ".png"; // Default handling if sent with no extension.
            }
            return SafeLoad<Sprite>(filename) ?? defaultSprite;
        }

        /// <summary>
        /// Loads the requested GameObject (3D model) from the AssetBundle.
        /// </summary>
        /// <param name="filename">The filename of the asset as it appears in the AssetBundle.</param>
        /// <returns>The loaded GameObject, if it exists within the AssetBundle. Otherwise, returns the game's default Mystery icon.</returns>
        public static GameObject LoadModel(string filename)
        {
            if (!filename.Contains("."))
            {
                filename += ".prefab"; // Default handling if sent with no extension.
            }
            return SafeLoad<GameObject>(filename) ?? defaultModel;
        }

        /// <summary>
        /// Loads a Sprite from the base game Risk of Rain 2 libraries. You must have the full path for the sprite.
        /// </summary>
        /// <param name="path">The path to the sprite in the game's files. Ex. "RoR2/Base/Common/MiscIcons/texMysteryIcon.png"</param>
        /// <returns>The loaded Sprite from the game.</returns>
        public static Sprite LoadBaseGameSprite(string path)
        {
            return Addressables.LoadAssetAsync<Sprite>(path).WaitForCompletion();
        //public static GameObject defaultModel = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
        }

        /// <summary>
        /// Loads a GameObject (3D Model) from the base game Risk of Rain 2 libraries. You must have the full path for the sprite.
        /// </summary>
        /// <param name="path">The path to the model in the game's files. Ex. "RoR2/Base/Mystery/PickupMystery.prefab"</param>
        /// <returns>The loaded GameObject from the game.</returns>
        public static GameObject LoadBaseGameModel(string path)
        {
            return Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
        }
    }

}
