using BepInEx;
using BepInEx.Logging;
using SPTBugfixes.Patches;

namespace SPTBugfixes
{
    [BepInPlugin("com.Arys.SPTBugfixes", "Arys' SPT Bugfixes", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource ModLogger;

        private void Awake()
        {
            ModLogger = Logger;

            new IncompatibleAmmoNotificationPatch().Enable();
        }
    }
}
