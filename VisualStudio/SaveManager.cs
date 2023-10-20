namespace SaveManager
{
    public class SaveManager : MelonMod
    {
        public static string CurrentSaveName { get; set; } = string.Empty;
        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.Instance.LoadKey))
            //{
            //    CommonUtilities.OnLoadKey();
            //}

            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.Instance.SaveKey))
            {
                CommonUtilities.OnSaveKey();
            }
        }
    }
}