using MelonLoader;
using UnityEngine;

namespace SaveManager
{
    public class SaveManager : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Settings.Instance.AddToModSettings("Save Manager");
            Settings.Instance.OnLoad();
        }
    }
}