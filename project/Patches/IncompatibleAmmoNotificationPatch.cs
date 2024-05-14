using Aki.Reflection.Patching;
using EFT;
using EFT.UI;
using HarmonyLib;
using System;
using System.Reflection;

namespace Arys.USPTP.Patches
{
    /// <summary>
    /// Prevents a bot with incompatible ammo in its gun from sending the notification to the player's screen.
    /// </summary>
    internal class IncompatibleAmmoNotificationPatch : ModulePatch
    {
        private const string _targetMethodName = "ShowUncompatibleNotification";

        protected override MethodBase GetTargetMethod()
        {
            Type targetType = AccessTools.FirstInner(typeof(Player.FirearmController), t => t.GetMethod(_targetMethodName) != null);

            return AccessTools.Method(targetType, _targetMethodName);
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player ___player_0)
        {
            if (___player_0.IsYourPlayer)
            {
                return true;
            }            

#if DEBUG
            string warning = $"Bot Id: {___player_0.PlayerId}, Name: {___player_0.Profile.Nickname} attempted to fire a gun with incompatible ammo loaded";
            ConsoleScreen.LogWarning(warning);
            USPTPPlugin.LogSource.LogWarning(warning);
#endif
            return false;
        }
    }
}
