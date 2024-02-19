using Aki.Reflection.Patching;
using EFT;
using EFT.UI;
using HarmonyLib;
using System.Reflection;

namespace SPTBugfixes.Patches
{
    /// <summary>
    /// Prevents a bot with incompatible ammo in its gun from sending the notification to the player's screen.
    /// </summary>
    internal class IncompatibleAmmoNotificationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            var targetMethodName = "ShowUncompatibleNotification";
            var targetType = AccessTools.FirstInner(typeof(Player.FirearmController), t => t.GetMethod(targetMethodName) != null);

            return AccessTools.Method(targetType, targetMethodName);
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player ___player_0)
        {
            if (___player_0.IsAI)
            {
#if DEBUG
                ConsoleScreen.LogWarning($"Bot Id {___player_0.PlayerId} attempted to fire a gun with incompatible ammo loaded");
#endif
                return false;
            }

            return true;
        }
    }
}
