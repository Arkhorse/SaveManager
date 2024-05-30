using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveManager.Patches
{
	[HarmonyPatch(typeof(Panel_SaveIcon), nameof(Panel_SaveIcon.StartSaveIconAnimation))]
	public class Panel_SaveIcon_StartSaveIconAnimation
	{
		public static bool Prefix(Panel_SaveIcon __instance)
		{
			Main.Logger.Log($"Panel_SaveIcon.StartSaveIconAnimation():: Setting: {Settings.Instance.SaveIconEnabled}", FlaggedLoggingLevel.Debug);
			if (Settings.Instance.SaveIconEnabled) return true;

			if (__instance.m_Sprite_IsSaving == null)
			{
				Main.Logger.Log($"m_Sprite_IsSaving is null", FlaggedLoggingLevel.Trace);
				return true;
			}

			if (__instance.m_Label_Saving == null)
			{
				Main.Logger.Log($"m_Label_Saving is null", FlaggedLoggingLevel.Trace);
				return true;
			}

			if (__instance.IsIconVisible() && !Settings.Instance.SaveIconEnabled)
			{
				Main.Logger.Log($"All checks worked, disabling the save icon and label", FlaggedLoggingLevel.Debug);

				__instance.m_Sprite_IsSaving.gameObject.SetActive(false);
				__instance.m_Label_Saving.gameObject.SetActive(false);
			}

			return !Settings.Instance.SaveIconEnabled;
		}

	}
}
