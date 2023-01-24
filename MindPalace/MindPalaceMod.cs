using LittleFirstPerson;
using MelonLoader;
using System.IO;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace MindPalace
{
    public class MindPalaceMod : MelonMod
    {
        public static MindPalaceMod instance;
        public static bool initialized = false;

        public static GameObject notePrefab;
        public static GameObject note;

        public override void OnApplicationStart()
        {
            instance = this;
            Bundles.loadAssetBundle();
            Note.LoadPrefab();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (!(sceneName == "FlatscreenUi")) return;
            MelonLogger.Msg("scene: " + sceneName);
            Notes.loadNotesFromStorage();
            initialized = true;
            MelonLogger.Msg("initialized");
        }

        public override void OnUpdate()
        {
            if (!initialized) return;

            Inputs.ProcessInputs();
        }
    }
}
