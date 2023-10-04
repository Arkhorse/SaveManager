namespace SaveManager
{
    public class SaveManager : MelonMod
    {
        public static string CurrentSaveName { get; set; } = string.Empty;
        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }
    }
}