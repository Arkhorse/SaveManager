namespace SaveManager
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

		[Section("Hotkeys")]

		[Name("Hotkey Preset")]
		public HotkeyPreset hotkeyPreset    = HotkeyPreset.Vanilla;

		[Name("Save")]
		public KeyCode SaveKey               = KeyCode.F5;

		[Name("Load")]
		public KeyCode LoadKey               = KeyCode.F6;

		[Section("Autosave")]

		[Name("Enabled")]
		public bool AutoSaveEnabled = false;

		[Name("Time in minutes")]
		[Slider(0,60,61)]
		public int AutoSaveTime = 0;

		protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
		{
			if ( InterfaceManager.GetPanel<Panel_SaveIcon>() != null && EnableMod && field.Name == nameof(SaveIconEnabled) )
			{
				InterfaceManager.GetPanel<Panel_SaveIcon>().Enable(SaveIconEnabled);
			}
			if (field.Name == nameof(AutoSaveTime))
			{
				MelonCoroutines.Stop(SaveManager.coroutine);
				SaveManager.coroutine = MelonCoroutines.Start(SaveManager.AutoSave());
			}
		}

		internal void DisableSettings()
		{
			SetFieldVisible(nameof(AutoSaveEnabled), false);
			SetFieldVisible(nameof(AutoSaveTime), false);
			SetFieldVisible(nameof(LoadKey), false);
		}

		internal static void OnLoad()
		{
			Instance.AddToModSettings(BuildInfo.GUIName);
			//Instance.DisableSettings();
			Instance.RefreshGUI();
		}
	}
}
