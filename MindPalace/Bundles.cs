using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MindPalace
{
    public static class Bundles
    {

        public static Il2CppAssetBundle mindPalaceBundle;
        public static string assetBundlePath = @"Mods\MindPalace.unity3d";

        public static void loadAssetBundle()
        {
            string[] filePaths = Directory.GetFiles(@"Mods", "*.unity3d",
                                         SearchOption.TopDirectoryOnly);
            MelonLogger.Msg(string.Join(",", filePaths));

            MelonLogger.Msg("Loading assetbundle: " + assetBundlePath);
            mindPalaceBundle = Il2CppAssetBundleManager.LoadFromFile(assetBundlePath);

            if (mindPalaceBundle == null)
            {
                MelonLogger.Msg("Failed to load assetbundle: " + assetBundlePath);
                return;
            }

            MelonLogger.Msg("Asset bundle loaded!");
        }
    }
}
