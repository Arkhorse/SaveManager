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

		public static bool AllowedToLoad()
		{
			if (InterfaceManager.GetPanel<Panel_PauseMenu>().IsEnabled() && InterfaceManager.IsPanelEnabled<Panel_ChallengeComplete>()) return false;
			if (GameManager.GetPlayerAnimationComponent().GetState() == PlayerAnimation.State.AnimatedInteraction) return false;
			if (GameManager.GetPlayerManagerComponent().PlayerIsClimbing() && GameManager.GetPlayerClimbRopeComponent().IsSlipping) return false;
			if (InterfaceManager.GetPanel<Panel_Loading>().IsEnabled()) return false;
			if (InterfaceManager.GetPanel<Panel_SaveIcon>().IsIconVisible()) return false;
			if (GameManager.m_DisableSaveLoad) return false;

			return true;
		}

		public static void LOAD()
		{
			Logger.Log($"LOAD Triggered", FlaggedLoggingLevel.Debug);

			SaveSlotInfo? ssi = SaveGameSystem.GetNewestSaveSlotForActiveGame();

			if (ssi == null)
			{
				Logger.Log($"ssi == null", FlaggedLoggingLevel.Debug);
				return;
			}
			else Logger.Log($"ssi was not null", FlaggedLoggingLevel.Debug);

			if (!AllowedToLoad())
			{
				Logger.Log($"Not allowed to load active game, {ssi.m_SaveSlotName}", FlaggedLoggingLevel.Debug);
				return;
			}
			else Logger.Log($"Allowed to save", FlaggedLoggingLevel.Debug);

			if (Settings.Instance.AlternativeLoad)
			{
				Logger.Log($"Using alternative load", FlaggedLoggingLevel.Debug);

				GameManager.LoadSaveGameSlot(ssi.m_SaveSlotName, ssi.m_SaveChangelistVersion);

				return;
			}

			GameManager.LoadSaveGameSlot(ssi);
		}

		public static void SAVE()
		{
			Logger.Log($"SAVE Triggered", FlaggedLoggingLevel.Debug);

			if (!GameManager.IsMainMenuActive())
			{
				Logger.Log($"Is not in main menu", FlaggedLoggingLevel.Debug);
				if (SaveGameSystem.IsRestoreInProgress())
				{
					Logger.Log($"SaveGameSystem.IsRestoreInProgress() == true", FlaggedLoggingLevel.Debug);
					return;
				}
				GameManager.SaveGameAndDisplayHUDMessage();
			}
		}

		public static void UpdateAutosave(bool enabled)
		{
			if (enabled)
			{
				Logger.Log($"Autosave Enabled, starting Coroutine if not already", FlaggedLoggingLevel.Trace);
				coroutine ??= MelonCoroutines.Start(AutoSave());
			}
			else
			{
				Logger.Log($"Autosave Disabled, stopping Coroutine if already running", FlaggedLoggingLevel.Trace);
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
			yield return null;
			yield return new WaitForSecondsRealtime(Settings.Instance.AutoSaveTime * 60);
			SAVE();

			Logger.Log($"Autosave completed with time of {Settings.Instance.AutoSaveTime}", FlaggedLoggingLevel.Trace);

			InterfaceManager.GetPanel<Panel_PauseMenu>().OnDone();
		}
	}
}