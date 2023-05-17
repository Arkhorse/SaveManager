using System.Reflection;
using Il2Cpp;
using ModSettings;
using UnityEngine;

namespace SaveManager
{
    internal class Settings : JsonModSettings
    {
        internal static readonly Settings Instance = new();

        [Section("Mod Controls")]
        [Name("Toggle Mod")]
        [Description("Turn the mod on or off")]
        public bool EnableMod                = true;

        [Name("Save When Exiting Interiors")]
        public bool BuildingExitEnabled      = false;

        [Name("Save Icon Enabled")]
        public bool SaveIconEnabled          = true;

        [Section("Hotkeys")]
        [Name("Save")]
        public KeyCode SaveKey               = KeyCode.F5;

        [Name("Load")]
        public KeyCode LoadKey               = KeyCode.F6;

        [Section("Exit Save")]
        [Name("Enable")]
        public bool ExitSaveEnabled          = false;

        [Section("Autosave")]
        [Name("Enable")]
        public bool AutosaveEnabled          = false;

        [Name("Only Outside")]
        public bool ExitSaveOutside = false;

        [Name("Time Between Autosaves")]
        [Description("Time in minutes")]
        [Slider(0, 120)]
        public int AutosaveSeconds           = 0;

        protected override void OnConfirm()
        {
            base.OnConfirm();
            if (!AutosaveEnabled)
            {
                Utilities.AutosaveTimer(Utilities.AutosaveHandles.StopAndReset);
            }
        }

        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            if (field.Name == nameof(EnableMod)             ||
                field.Name == nameof(BuildingExitEnabled)   ||
                field.Name == nameof(SaveIconEnabled)       ||
                field.Name == nameof(SaveKey)               ||
                field.Name == nameof(LoadKey)               ||
                field.Name == nameof(ExitSaveEnabled)       ||
                field.Name == nameof(ExitSaveOutside)       ||
                field.Name == nameof(AutosaveEnabled)       ||
                field.Name == nameof(AutosaveSeconds)
                )
            {
                RefreshFieldVisibility();
            }
            if ( InterfaceManager.GetPanel<Panel_SaveIcon>() != null && EnableMod && field.Name == nameof(SaveIconEnabled) )
            {
                InterfaceManager.GetPanel<Panel_SaveIcon>().Enable(SaveIconEnabled);
            }
        }

        internal void RefreshFieldVisibility()
        {
            SetFieldVisible(nameof(BuildingExitEnabled), EnableMod);
            SetFieldVisible(nameof(SaveIconEnabled), EnableMod);
            SetFieldVisible(nameof(SaveKey), EnableMod);
            SetFieldVisible(nameof(LoadKey), false); //EnableMod); // not working atm
            SetFieldVisible(nameof(ExitSaveEnabled), EnableMod);
            SetFieldVisible(nameof(ExitSaveOutside), EnableMod && ExitSaveEnabled);
            SetFieldVisible(nameof(AutosaveEnabled), EnableMod);
            SetFieldVisible(nameof(AutosaveSeconds), EnableMod && AutosaveEnabled);
        }

        internal void OnLoad()
        {
            RefreshFieldVisibility();
        }
    }
}
