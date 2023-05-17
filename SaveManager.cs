using MelonLoader;
using UnityEngine;

namespace SaveManager
{
    public class SaveManager : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Settings.Instance.AddToModSettings("Save Manager");
            Settings.Instance.OnLoad();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName.Contains("Boot") || sceneName.Contains("Menu"))
            {
#if DEBUG
                MelonLogger.Msg($"Scene contains either \"Boot\" or \"Menu\"");
#endif
                if (Settings.Instance.AutosaveEnabled) Utilities.AutosaveTimer(Utilities.AutosaveHandles.stop);
            }
            if (sceneName.Contains("SANDBOX"))
            {
#if DEBUG
                MelonLogger.Msg($"Scene contains either \"SANDBOX\"");
#endif
                if (Settings.Instance.AutosaveEnabled)
                {
                    Utilities.AutosaveTimer(Utilities.AutosaveHandles.start);
                }
            }
        }

        //public override void OnUpdate()
        //{
        //    Utilities.LastSaveTime += Time.deltaTime;
        //}
    }
}