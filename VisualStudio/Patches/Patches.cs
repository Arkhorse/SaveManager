namespace SaveManager
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    internal class GameManager_Update
    {
        private static void Postfix()
        {
            Utilities.OnSaveKey();
            Utilities.OnLoadKey();
        }
    }
}
