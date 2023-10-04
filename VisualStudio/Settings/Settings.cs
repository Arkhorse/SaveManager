namespace SaveManager
{
    public class Settings : JsonModSettings
    {
        internal static readonly Settings Instance = new();

        [Section("Mod Controls")]

        [Name("Toggle Mod")]
        [Description("Turn the mod on or off")]
        public bool EnableMod                = true;

        [Name("Save Icon Enabled")]
        public bool SaveIconEnabled          = true;

        [Section("Hotkeys")]

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

        protected override void OnConfirm()
        {
            base.OnConfirm();

            if (Instance.AutoSaveEnabled)
            {
                InterfaceManager.GetPanel<Panel_OptionsMenu>().State.m_AutosaveMinutes = Instance.AutoSaveTime;
            }
        }

        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            if (field.Name == nameof(EnableMod)             ||
                field.Name == nameof(SaveIconEnabled)       ||
                field.Name == nameof(SaveKey)               ||
                field.Name == nameof(LoadKey)
                )
            {
                Refresh();
            }
            if ( InterfaceManager.GetPanel<Panel_SaveIcon>() != null && EnableMod && field.Name == nameof(SaveIconEnabled) )
            {
                InterfaceManager.GetPanel<Panel_SaveIcon>().Enable(SaveIconEnabled);
            }
        }

        internal void Refresh()
        {
            SetFieldVisible(nameof(SaveIconEnabled), EnableMod);
            SetFieldVisible(nameof(SaveKey), EnableMod);
            SetFieldVisible(nameof(LoadKey), EnableMod); // not working atm
        }

        internal static void OnLoad()
        {
            Instance.AddToModSettings(BuildInfo.GUIName);
            Instance.Refresh();
            Instance.RefreshGUI();
        }
    }
}
