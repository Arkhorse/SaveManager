using SaveManager.Utilities;

namespace SaveManager
{
    internal class CommonUtilities
    {
        /// <summary>
        /// When the save get is pressed, this gets called
        /// </summary>
        internal static void OnSaveKey()
        {
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {
                GameManager.SaveGameAndDisplayHUDMessage();
            }
        }

        internal static void OnLoadKey()
        {
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {
                SaveSlotInfo ssi = SaveGameSlotHelper.GetCurrentSaveSlotInfo();
                string slotname = ssi.m_SaveSlotName;
                int version = ssi.m_SaveChangelistVersion;

                GameManager.LoadSaveGameSlot(slotname, version);
            }
        }
    }
}
