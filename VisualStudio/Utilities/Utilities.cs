namespace SaveManager
{
    internal class Utilities
    {
        public static bool CanSave()
        {
            return !GameManager.IsMainMenuActive() && !InterfaceManager.IsOverlayActiveCached();
        }

        /// <summary>
        /// When the save get is pressed, this gets called
        /// </summary>
        internal static void OnSaveKey()
        {
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode() && CanSave())
            {
                if (KeyboardUtilities.InputManager.GetKeyDown(Settings.Instance.SaveKey)) GameManager.SaveGameAndDisplayHUDMessage();
            }
        }

        internal static void OnLoadKey()
        {
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {
                if (KeyboardUtilities.InputManager.GetKeyDown(Settings.Instance.LoadKey))
                {
                    GameManager.LoadGame(SaveGameSystem.GetNewestSaveSlotForActiveGame());
                }
            }
        }

        #region Utils
        /// <summary>
        /// Simply converts minutes to seconds
        /// </summary>
        /// <param name="minutes">The int value for the minutes you want to convert</param>
        /// <returns>And int representing the seconds</returns>
        internal static int GetMinutesToSeconds(int minutes)
        {
            return minutes * 60;
        }
        #endregion
    }
}
