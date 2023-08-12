
namespace diabetisApp
{
    internal static class GlobalVars
    {
        public static bool haveRead = false;
        public static bool autoClose = false;
        public const string VERSION = "1.0.1";
        public const string UPDATE_URL = "https://github.com/Marius1342/readBloodSugar_dexcom/releases/";
        public const string UPDATE_CHECK_URL = "https://raw.githubusercontent.com/Marius1342/readBloodSugar_dexcom/master/versions.txt";
#if WINDOWS
        public const string PLATFROM = "WINDOWS";
#elif ANDROID
 public const string PLATFROM = "ANDROID";
#else
public const string PLATFROM = "UNKNOWN";
#error Cannot find current platfrom !
#endif
    }
}
