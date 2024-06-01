using System.IO;
using UnityEngine;

namespace ExamplePlugin.Util
{
    public static class AssetUtil
    {
        public static AssetBundle bundle;
        public const string bundleName = "prestigemodassets";

        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(ExamplePlugin.SavedInfo.Location), bundleName);
            }
        }

        public static void Init()
        {
            bundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }
}
