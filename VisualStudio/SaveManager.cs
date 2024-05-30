global using SaveManager.Utilities.Logger;
global using SaveManager.Utilities.Logger.Enums;

using System.Collections;

using Il2CppInterop.Runtime.Attributes;

using MelonLoader;


namespace SaveManager
{
	public class Main : MelonMod
	{
		public static ComplexLogger<Main> Logger = new();
		public static object coroutine;

		public static KeyCode SaveKeyCode
		{
			get
			{
				if (Settings.Instance.Preset == Settings.HotkeyPreset.Vanilla) return KeyCode.F5;
				else return Settings.Instance.SaveKey;
			}
		}

		public static KeyCode LoadKeyCode
		{
			get
			{
				if (Settings.Instance.Preset == Settings.HotkeyPreset.Vanilla) return KeyCode.F6;
				else return Settings.Instance.LoadKey;
			}
		}

		public override void OnInitializeMelon()
		{
			Settings.OnLoad();
		}

		public override void OnUpdate()
		{
			if (!Settings.Instance.EnableMod)
			{
				UpdateAutosave(false);
				return;
			}
			if (!Settings.Instance.AutoSaveEnabled)
			{
				Logger.Log("Autosave is not enabled, stopping Coroutine if applicable", FlaggedLoggingLevel.Debug);
				UpdateAutosave(false);
			}

			if (!GameManager.IsMainMenuActive())
			{
				if (Settings.Instance.AutoSaveEnabled)
				{
					UpdateAutosave(true);
				}

				if (InputManager.GetKeyDown(InputManager.m_CurrentContext, SaveKeyCode))
				{
					SAVE();
				}

				if (InputManager.GetKeyDown(InputManager.m_CurrentContext, LoadKeyCode))
				{
					LOAD();
				}
			}
		}

		public static void LOAD()
		{
			SaveSlotInfo ssi = SaveGameSystem.GetNewestSaveSlotForActiveGame();
			if (!GameManager.AllowedToLoadActiveGame())
			{
				Logger.Log($"Not allowed to load active game, {ssi.m_SaveSlotName}", FlaggedLoggingLevel.Debug);
				return;
			}

			if (ssi == null) return;

			GameManager.LoadSaveGameSlot(ssi);
		}

		public static void SAVE()
		{
			if (!GameManager.IsMainMenuActive())
			{
				if (SaveGameSystem.IsRestoreInProgress()) return;
				GameManager.SaveGameAndDisplayHUDMessage();
			}
		}

		public static void UpdateAutosave(bool enabled)
		{
			if (enabled)
			{
				coroutine ??= MelonCoroutines.Start(AutoSave());
			}
			else
			{
				if (coroutine != null) MelonCoroutines.Stop(coroutine);
			}
		}

		public static void RestartAutosave()
		{
			UpdateAutosave(false);
			UpdateAutosave(true);
		}

		public static IEnumerator AutoSave()
		{
			yield return new WaitForSecondsRealtime(Settings.Instance.AutoSaveTime * 60);
			SAVE();

			Logger.Log($"Autosave completed with time of {Settings.Instance.AutoSaveTime}", FlaggedLoggingLevel.Debug);
			yield break;
		}
	}
}