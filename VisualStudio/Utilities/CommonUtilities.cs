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
            MelonLogger.Msg("OnSaveKey");
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {
                
            }
        }

        internal static void OnLoadKey()
        {
			MelonLogger.Msg("OnLoadKey");
			if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {

            }
        }
    }
}
