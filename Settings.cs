using System.Reflection;
using Il2Cpp;
using ModSettings;
using UnityEngine;

namespace SaveManager
{
    internal class Settings : JsonModSettings
    {
        internal static readonly Settings Instance = new();

        public enum SavePresets { Hotkey, GameBase, Disabled };

        [Section("Mod Controls")]

        [Name("Toggle Mod")]
        [Description("Turn the mod on or off")]
        public bool EnableMod               = true;

        [Name("Save When Exiting Interiors")]
        public bool BuildingExitEnabled     = false;

        [Name("Save Icon Enabled")]
        public bool SaveIconEnabled         = true;

        [Section("Exit Save")]

        [Name("Enable")]
        public bool ExitSaveEnabled         = false;

        [Section("Hotkeys")]

        [Name("Preset")]
        [Description("GameBase means to use the already set hotkey. Disabled means the hotkey wont work")]
        public SavePresets SaveHotKey       = SavePresets.GameBase;

        [Name("Save")]
        public KeyCode SaveKey              = KeyCode.None;

        [Name("Load")]
        public KeyCode LoadKey              = KeyCode.None;

        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            if (field.Name == nameof(EnableMod)             ||
                field.Name == nameof(BuildingExitEnabled)   ||
                field.Name == nameof(SaveIconEnabled)       ||
                field.Name == nameof(SaveKey)               ||
                field.Name == nameof(LoadKey)               ||
                field.Name == nameof(ExitSaveEnabled)       
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
            SetFieldVisible(nameof(ExitSaveEnabled), EnableMod);
            SetFieldVisible(nameof(SaveKey), EnableMod && SaveHotKey == SavePresets.Hotkey);
            SetFieldVisible(nameof(LoadKey), false); //EnableMod); // not working atm
        }

        internal void OnLoad()
        {
            RefreshFieldVisibility();
        }
    }
}
