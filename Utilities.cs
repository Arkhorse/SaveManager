using Il2Cpp;
using KeyboardUtilities;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace SaveManager
{
    internal class Utilities
    {
        internal static bool CanExitSave            = false;
        internal static bool ExitEnabled            = Settings.Instance.EnableMod && Settings.Instance.ExitSaveEnabled;
        internal static bool IsInside               = GameManager.GetWeatherComponent() != null && GameManager.GetWeatherComponent().IsIndoorEnvironment();

        #region ENUMS
        public enum ConvertUnits { minutes, hours };
        #endregion

        #region Save Utils
        /// <summary>
        /// When the save get is pressed, this gets called
        /// </summary>
        internal static void OnSaveKey()
        {
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {
                if (Settings.Instance.SaveHotKey == Settings.SavePresets.Hotkey && KeyboardUtilities.InputManager.GetKeyDown(Settings.Instance.SaveKey))
                {
                    GameManager.SaveGameAndDisplayHUDMessage();
                }
                else if (Settings.Instance.SaveHotKey == Settings.SavePresets.GameBase && Il2Cpp.InputManager.GetQuickSavePressed(Il2Cpp.InputManager.m_CurrentContext))
                {
                    GameManager.SaveGameAndDisplayHUDMessage();
                }
                else if (Settings.Instance.SaveHotKey == Settings.SavePresets.Disabled)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Deals with exit saves
        /// </summary>
        internal static void OnExitSave()
        {
            if (ExitEnabled && CanExitSave)
            {
                CanExitSave = false;
                GameManager.GetLogComponent().WriteLogToFile();
                GameManager.LoadMainMenu();
            }
        }

        /// <summary>
        /// Attempts to save when exiting interiors
        /// </summary>
        /// <returns></returns>
        internal static bool MaybeSaveOnExitBuilding()
        {
            if (Settings.Instance.EnableMod && Settings.Instance.BuildingExitEnabled)
            {
                if (GameManager.m_SceneTransitionData == null || GameManager.GetWeatherComponent() == null) return false;
                return !GameManager.m_SceneTransitionData.m_TeleportPlayerSaveGamePosition && !GameManager.GetWeatherComponent().IsIndoorEnvironment() && GameManager.m_SceneTransitionData.m_SpawnPointName != null;
            }
            return false;
        }

        /// <summary>
        /// Checks if there should be a save when you exit the game
        /// </summary>
        /// <returns>True if all settings allow for the exit save</returns>
        internal static bool MaybeSaveGameOnExit()
        {
            if (ExitEnabled)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Utils
        /// <summary>
        /// Simply converts the input to seconds
        /// </summary>
        /// <param name="convert">enum <c>this.ConvertUnits</c></param>
        /// <param name="units">int of number of units you want to convert</param>
        /// <returns>converted int representing the seconds from the units</returns>
        public static int GetInputToSeconds(ConvertUnits convert, int units)
        {
            #pragma warning disable IDE0066
            switch (convert)
            {
                case ConvertUnits.hours:
                    return units * 60 * 60;
                case ConvertUnits.minutes:
                    return units * 60;
                default:
                    return units;
            }
            #pragma warning restore IDE0066
        }
        #endregion
    }
}
