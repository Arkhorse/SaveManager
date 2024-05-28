﻿namespace SaveManager
{
	public class Settings : JsonModSettings
	{
		internal static readonly Settings Instance = new();
		public enum HotkeyPreset { Vanilla, Custom }

		[Section("Mod Controls")]

		[Name("Toggle Mod")]
		[Description("Turn the mod on or off")]
		public bool EnableMod                = true;

		[Name("Save Icon Enabled")]
		[Description("Allows you to disable the 'Saving' icon on the right of the screen")]
		public bool SaveIconEnabled          = true;

		[Name("Debug")]
		public bool DEBUG = false;

		[Section("Hotkeys")]

		[Name("Hotkey Preset")]
		public HotkeyPreset Preset    = HotkeyPreset.Vanilla;

		[Name("Save")]
		public KeyCode SaveKey               = KeyCode.F5;

		[Name("Load")]
		public KeyCode LoadKey               = KeyCode.F6;

		[Section("Autosave")]

		[Name("Enabled")]
		public bool AutoSaveEnabled = false;

		[Name("Time in minutes")]
		[Description("Set this to how long you want the autosave to wait between saves. A value of 0 disables the autosave")]
		[Slider(0,60,61)]
		public int AutoSaveTime = 15;

		protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
		{
			if (field.Name == nameof(EnableMod) || field.Name == nameof(AutoSaveEnabled))
			{
				Main.UpdateAutosave(false);
			}
			if ( InterfaceManager.GetPanel<Panel_SaveIcon>() != null && EnableMod && field.Name == nameof(SaveIconEnabled) )
			{
				InterfaceManager.GetPanel<Panel_SaveIcon>().Enable(SaveIconEnabled);
			}
			if (field.Name == nameof(AutoSaveTime))
			{
				if (Instance.AutoSaveTime == 0)
				{
					AutoSaveEnabled = false;
					Main.UpdateAutosave(false);
				}
				Main.RestartAutosave();
			}
			if (field.Name == nameof(DEBUG))
			{
				if (Instance.DEBUG) Main.Logger.AddLevel(FlaggedLoggingLevel.Debug);
				else Main.Logger.RemoveLevel(FlaggedLoggingLevel.Debug);
			}
		}

		internal static void OnLoad()
		{
			Instance.AddToModSettings(BuildInfo.GUIName);
			Instance.RefreshGUI();
		}
	}
}
