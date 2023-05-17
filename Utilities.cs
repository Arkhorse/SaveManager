using Il2Cpp;
using KeyboardUtilities;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace SaveManager
{
    internal class Utilities
    {
        internal static float LastSaveTime          = 0f;
        internal static bool CanExitSave            = false;
        internal static bool ExitEnabled            = Settings.Instance.EnableMod && Settings.Instance.ExitSaveEnabled;
        internal static bool IsInside               = GameManager.GetWeatherComponent() != null && GameManager.GetWeatherComponent().IsIndoorEnvironment();
        internal static bool OutsideEnabled         = Settings.Instance.ExitSaveOutside && IsInside;

        internal static bool AutoSaveTimerEnabled   = !GameManager.IsMainMenuActive();
        internal static int SaveTime                = Settings.Instance.AutosaveSeconds;

        #region ENUMS
        internal enum AutosaveHandles { start, stop, reset, StopAndReset }
        #endregion

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
            if (ExitEnabled && CanExitSave && LastSaveTime == 0)
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
        /// Simply attempts to do an autosave
        /// </summary>
        internal static void MaybeAutosave()
        {
#if DEBUG
            MelonLogger.Msg($"Begin MaybeAutosave Debug");
            MelonLogger.Msg($"OutsideEnabled            : {OutsideEnabled}"                     );
            MelonLogger.Msg($"Enabled                   : {Settings.Instance.EnableMod}"        );
            MelonLogger.Msg($"Autosave Enabled          : {Settings.Instance.AutosaveEnabled}"  );
            MelonLogger.Msg($"Autosave Seconds          : {GetMinutesToSeconds(Settings.Instance.AutosaveSeconds)}");
            MelonLogger.Msg($"LastSaveTime              : {LastSaveTime}"                       );
            MelonLogger.Msg($"End MaybeAutosave Debug"                                          );
#endif
            if (!OutsideEnabled && Settings.Instance.EnableMod && Settings.Instance.AutosaveEnabled)
            {
                if (LastSaveTime > GetMinutesToSeconds(Settings.Instance.AutosaveSeconds))
                {
                    GameManager.SaveGameAndDisplayHUDMessage();
                    AutosaveTimer(AutosaveHandles.reset);
                }
            }
        }
        internal static IEnumerator SaveTimer()
        {
            while(AutoSaveTimerEnabled)
            {
                MaybeAutosave();
                LastSaveTime += 1;
                yield return new WaitForSeconds(SaveTime);
            }
        }

        /// <summary>
        /// Handles any use of the AutosaveTime
        /// </summary>
        /// <param name="handle">What you want to do. This is an enum named AutosaveHandles</param>
        internal static void AutosaveTimer(AutosaveHandles handle)
        {
            if (handle == AutosaveHandles.start)
            {
                MelonCoroutines.Start(SaveTimer());
            }
            else if (handle == AutosaveHandles.stop)
            {
                object AutosaveTimer = MelonCoroutines.Start(SaveTimer());
                if (AutosaveTimer != null)
                {
                    MelonCoroutines.Stop(AutosaveTimer);
                }
            }
            else if (handle == AutosaveHandles.StopAndReset)
            {
                object AutosaveTimer = MelonCoroutines.Start(SaveTimer());
                if (AutosaveTimer != null)
                {
                    MelonCoroutines.Stop(AutosaveTimer);
                }
                LastSaveTime = 0;
            }
            else if (handle == AutosaveHandles.reset)
            {
                LastSaveTime = 0;
            }
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
