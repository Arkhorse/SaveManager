namespace SaveManager
{
    public class SceneUtilities
    {
        public static bool IsScenePlayable(string? sceneName = null)
        {
            string currentScene = string.Empty;
            if (sceneName == null)
            {
                currentScene = GameManager.m_ActiveScene;
            }
            else
            {
                currentScene = sceneName;
            }

            if (!(currentScene == "Empty" || currentScene == "Boot" || currentScene.StartsWith("MainMenu", StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Used to check if the current scene is a base scene (Zone or Region)
        /// </summary>
        /// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
        /// <returns></returns>
        public static bool IsSceneBase(string? sceneName = null)
        {
            string currentScene = string.Empty;

            if (sceneName == null)
            {
                currentScene = GameManager.m_ActiveScene;
            }
            if (sceneName != null)
            {
                currentScene = sceneName;
            }

            bool RegionOrZone = currentScene.Contains("Region", StringComparison.InvariantCultureIgnoreCase) || currentScene.Contains("Zone", StringComparison.InvariantCultureIgnoreCase);

            return RegionOrZone;
        }

        /// <summary>
        /// Used to check if the current scene is an additive scene, like sandbox or DLC scenes added to the base scene
        /// </summary>
        /// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
        /// <returns></returns>
        public static bool IsSceneAdditive(string? sceneName = null)
        {
            string currentScene = string.Empty;

            if (sceneName == null)
            {
                currentScene = GameManager.m_ActiveScene;
            }
            if (sceneName != null)
            {
                currentScene = sceneName;
            }

            bool IsAdditiveScene = currentScene.Contains("SANDBOX", StringComparison.InvariantCultureIgnoreCase) || currentScene.EndsWith("DARKWALKER", StringComparison.InvariantCultureIgnoreCase) || currentScene.EndsWith("DLC01", StringComparison.InvariantCultureIgnoreCase);

            return IsAdditiveScene;
        }
    }
}
