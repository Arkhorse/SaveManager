using System.Collections;

using Il2CppInterop.Runtime.Attributes;

using MelonLoader;

namespace SaveManager
{
	public class SaveManager : MelonMod
	{
		public static string CurrentSaveName { get; set; } = string.Empty;
		public static object coroutine;
		public static bool saving;
		public override void OnInitializeMelon()
		{
			Settings.OnLoad();
		}

		public override void OnUpdate()
		{
			if (!Settings.Instance.EnableMod)
			{
				if (coroutine != null) MelonCoroutines.Stop(coroutine);
				return;
			}

			if (!GameManager.IsMainMenuActive())
			{
				if (Settings.Instance.AutoSaveEnabled)
				{
					coroutine ??= MelonCoroutines.Start(AutoSave());
				}

				if (!Settings.Instance.AutoSaveEnabled)
				{
					MelonLogger.Msg("Autosave is not enabled, stopping Coroutine if applicable");
					if (coroutine != null) MelonCoroutines.Stop(coroutine);
				}
				if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.Instance.SaveKey))
				{
					GameManager.SaveGameAndDisplayHUDMessage();
				}

				if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.Instance.LoadKey))
				{
					SaveSlotInfo ssi = SaveGameSystem.GetNewestSaveSlotForActiveGame();
					if (ssi == null) return;

					GameManager.LoadSaveGameSlot(ssi);
				}
			}
		}

		public static void SAVE()
		{
			if (!GameManager.IsMainMenuActive())
			{
				GameManager.SaveGameAndDisplayHUDMessage();
				MelonLogger.Msg("Autosave occured");
			}
		}

		public static IEnumerator AutoSave()
		{
			yield return new WaitForSecondsRealtime(Settings.Instance.AutoSaveTime * 60);
			SAVE();

			MelonLogger.Msg($"Autosave completed with time of {Settings.Instance.AutoSaveTime}");
			yield break;
		}
	}
}