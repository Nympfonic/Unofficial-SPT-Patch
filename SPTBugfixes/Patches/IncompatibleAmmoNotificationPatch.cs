using Aki.Reflection.Patching;
using EFT;
using EFT.UI;
using HarmonyLib;
using System;
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
            try
            {
                var targetType = AccessTools.FirstInner(typeof(Player), t => t.GetMethod(targetMethodName) != null);

                return AccessTools.DeclaredMethod(targetType, targetMethodName);
            }
            catch (Exception ex)
            {
                Plugin.ModLogger.LogError($"Could not find target type with method name: {targetMethodName}. {ex}");
                throw;
            }
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player ___player_0)
        {
            if (___player_0.IsAI)
            {
                ConsoleScreen.LogWarning($"Bot Id {___player_0.PlayerId} attempted to fire a gun with incompatible ammo loaded");
                return false;
            }

            return true;
        }
    }
}
