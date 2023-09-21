
namespace diabetisApp
{
    internal static class GlobalVars
    {
        public static bool haveRead = false;
        public static bool autoClose = false;
        public const string VERSION = "1.1.4";
        public const string UPDATE_URL = "https://github.com/Marius1342/readBloodSugar_dexcom/releases/";
        public const string UPDATE_CHECK_URL = "https://raw.githubusercontent.com/Marius1342/readBloodSugar_dexcom/master/versions.txt";
#if WINDOWS
        public const string PLATFORM = "WINDOWS";
#elif ANDROID
        public const string PLATFORM = "ANDROID";
#else
public const string PLATFORM = "UNKNOWN";

#endif
        public const string DEV_EMAIL = "km3814837@gmail.com";



    }
}
