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
        internal static bool OutsideEnabled         = Settings.Instance.ExitSaveOutside && IsInside;

        #region Save Utils
        /// <summary>
        /// When the save get is pressed, this gets called
        /// </summary>
        internal static void OnSaveKey()
        {
            if (Settings.Instance.EnableMod && Il2Cpp.InputManager.instance != null && !GameManager.IsStoryMode())
            {
                if (KeyboardUtilities.InputManager.GetKeyDown(Settings.Instance.SaveKey)) GameManager.SaveGameAndDisplayHUDMessage();
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
                if (OutsideEnabled) return false;
                return true;
            }
            return false;
        }
        #endregion

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
