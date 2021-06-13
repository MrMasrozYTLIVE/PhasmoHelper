using MelonLoader;
using System;

namespace PhasmoHelper
{
    class Debug
    {
        public static void Msg(String text, int mode = 0)
        {
            if (CheatToggles.enableDebug && mode > 0) { 
                if(debugMode1 && mode == 1)
                    MelonLogger.Log($"[+] {text}");
                if (debugMode2 && mode == 2)
                    MelonLogger.Log($"[#] {text}");
                if (debugMode3 && mode == 3)
                    MelonLogger.Log($"[D] {text}");
            }
        }

        public static bool debugMode1 = false;
        public static bool debugMode2 = false;
        public static bool debugMode3 = false;
    }
}
