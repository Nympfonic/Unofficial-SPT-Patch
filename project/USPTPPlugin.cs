#pragma warning disable S101, S2223, S2696

using Aki.Reflection.Patching;
using Arys.USPTP.Patches;
using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;

namespace Arys.USPTP
{
    [BepInPlugin("com.Arys.USPTP", "Unofficial SPT Patch", "1.0.0")]
    [BepInDependency("com.Arys.UnityToolkit")]
    public class USPTPPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource LogSource;

        private static readonly List<ModulePatch> _patches = [
            new IncompatibleAmmoNotificationPatch()
        ];

        private void Awake()
        {
            LogSource = Logger;

            EnablePatches();
        }

        private static void EnablePatches()
        {
            foreach (var patch in _patches)
            {
                patch.Enable();
            }
        }
    }
}
