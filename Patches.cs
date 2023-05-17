using Il2Cpp;
using HarmonyLib;

namespace SaveManager
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    internal class GameManager_Update
    {
        private static void Postfix()
        {
            Utilities.OnExitSave();
            Utilities.OnSaveKey();

            Utilities.MaybeSaveOnExitBuilding();
        }
    }

    [HarmonyPatch(typeof(Panel_PauseMenu), nameof(Panel_PauseMenu.DoQuitGame))]
    internal class Panel_PauseMenu_DoQuitGame
    {
        private static bool Prefix(Panel_PauseMenu __instance)
        {
            if (__instance != null && Settings.Instance.EnableMod && Settings.Instance.ExitSaveEnabled)
            {
                if (Utilities.MaybeSaveGameOnExit())
                {
                    GameManager.OnGameQuit();
                    __instance.OnDone();
                    GameManager.SaveGameAndDisplayHUDMessage();
                    Utilities.CanExitSave = true;
                }
            }
            return true;
        }
    }
}
