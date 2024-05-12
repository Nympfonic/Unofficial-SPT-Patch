#pragma warning disable S101, S2223, S2696

using Arys.USPTP.Patches;
using BepInEx;
using BepInEx.Logging;

namespace Arys.USPTP
{
    [BepInPlugin("com.Arys.USPTP", "Unofficial SPT Patch", "1.0.0")]
    public class USPTPPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource LogSource;

        private void Awake()
        {
            LogSource = Logger;

            new IncompatibleAmmoNotificationPatch().Enable();
        }
    }
}
