namespace SaveManager.Utilities
{
    public class Logging
    {
        public static void LogStarter()                                                     => Melon<SaveManager>.Logger.Msg($"Mod loaded with v{BuildInfo.Version}");
        public static void Log(string message, params object[] parameters)                  => Melon<SaveManager>.Logger.Msg($"{message}", parameters);
        public static void LogWarning(string message, params object[] parameters)           => Melon<SaveManager>.Logger.Warning($"{message}", parameters);
        public static void LogError(string message, params object[] parameters)             => Melon<SaveManager>.Logger.Error($"{message}", parameters);
        public static void LogSeperator(params object[] parameters)                         => Melon<SaveManager>.Logger.Msg("==============================================================================", parameters);
        public static void LogIntraSeparator(string message, params object[] parameters)    => Melon<SaveManager>.Logger.Msg($"=========================   {message}   =========================", parameters);
    }
}